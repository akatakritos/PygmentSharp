using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using JetBrains.Annotations;

using PygmentSharp.Core.Utils;

namespace PygmentSharp.Core
{
    public class StateRuleBuilder
    {
        public RegexOptions DefaultRegexOptions { get; set; } = RegexOptions.None;

        public FluentStateRuleBuilder NewRuleSet()
        {
            return new FluentStateRuleBuilder(DefaultRegexOptions);
        }

        public class FluentStateRuleBuilder
        {
            private const int ListDefaultCapacity = 16;

            private readonly RegexOptions _defaultRegexOptions;
            private readonly List<StateRule> _rules = new List<StateRule>(ListDefaultCapacity);

            public StateRule[] Build() => _rules.ToArray();

            public FluentStateRuleBuilder(RegexOptions defaultRegexOptions)
            {
                _defaultRegexOptions = defaultRegexOptions;
            }

            public FluentStateRuleBuilder Add([RegexPattern,NotNull]string regex, [NotNull]TokenType tokenType, [NotNull]string nextState)
            {
                Argument.EnsureNotNull(regex, nameof(regex));
                Argument.EnsureNotNull(tokenType, nameof(tokenType));
                Argument.EnsureNotNull(nextState, nameof(nextState));

                _rules.Add(new StateRule(CreateRegex(regex), tokenType, Parse(nextState)));
                return this;
            }

            public FluentStateRuleBuilder Add([RegexPattern]string regex, TokenType tokenType, params string[] nextStates)
            {
                Argument.EnsureNotNull(regex, nameof(regex));
                Argument.EnsureNotNull(tokenType, nameof(tokenType));
                Argument.EnsureNoNullElements(nextStates, nameof(nextStates));

                _rules.Add(new StateRule(CreateRegex(regex), tokenType,
                    new CombinedAction(nextStates.Select(Parse).ToArray())));
                return this;
            }

            public FluentStateRuleBuilder Add([RegexPattern]string regex, TokenType tokenType)
            {
                Argument.EnsureNotNull(regex, nameof(regex));
                Argument.EnsureNotNull(tokenType, nameof(tokenType));

                _rules.Add(new StateRule(CreateRegex(regex), tokenType, new NoopAction()));
                return this;
            }

            public FluentStateRuleBuilder Default(params string[] states)
            {
                Argument.EnsureNoNullElements(states, nameof(states));

                _rules.Add(new StateRule(CreateRegex(""), TokenTypes.Token,
                    new CombinedAction(states.Select(Parse).ToArray())));
                return this;
            }

            public FluentStateRuleBuilder Include(IEnumerable<StateRule> existing)
            {
                Argument.EnsureNotNull(existing, nameof(existing));

                _rules.AddRange(existing);
                return this;
            }

            public FluentStateRuleBuilder ByGroups([RegexPattern]string regex, params GroupProcessor[] processors)
            {
                Argument.EnsureNotNull(regex, nameof(regex));
                Argument.EnsureNotNull(processors, nameof(processors));
                Argument.EnsureNoNullElements(processors, nameof(processors));

                _rules.Add(new StateRule(CreateRegex(regex), TokenTypes.Token,
                    new GroupAction(processors)));
                return this;
            }

            public FluentStateRuleBuilder ByGroups([RegexPattern]string regex, string newState, params GroupProcessor[] processors)
            {
                Argument.EnsureNotNull(regex, nameof(regex));
                Argument.EnsureNotNull(newState, nameof(newState));
                Argument.EnsureNotNull(processors, nameof(processors));
                Argument.EnsureNoNullElements(processors, nameof(processors));

                _rules.Add(new StateRule(CreateRegex(regex), TokenTypes.Token,
                    new GroupAction(Parse(newState), processors)));
                return this;
            }

            public FluentStateRuleBuilder ByGroups([RegexPattern]string regex, string[] newStates, params GroupProcessor[] processors)
            {
                Argument.EnsureNotNull(regex, nameof(regex));
                Argument.EnsureNotNull(newStates, nameof(newStates));
                Argument.EnsureNoNullElements(newStates, nameof(newStates));
                Argument.EnsureNotNull(processors, nameof(processors));
                Argument.EnsureNoNullElements(processors, nameof(processors));

                var actions = newStates.Select(Parse).ToArray();
                _rules.Add(new StateRule(CreateRegex(regex), TokenTypes.Token,
                    new GroupAction(new CombinedAction(actions), processors)));
                return this;
            }

            public FluentStateRuleBuilder Using<T>([RegexPattern]string regex) where T : Lexer, new()
            {
                Argument.EnsureNotNull(regex, nameof(regex));

                var lexer = new T();
                _rules.Add(new StateRule(CreateRegex(regex), TokenTypes.Token, new LexerAction(lexer)));
                return this;
            }

            private StateChangingAction Parse(string name)
            {
                if (name == "#push")
                    return new PushAgainAction();
                if (name == "#pop")
                    return new PopAction();

                return new PushStateAction(name);
            }

            private Regex CreateRegex(string regex)
            {
                // wrap in non capturing group and apply the \G prefix, which means
                // to start looking at a provided index
                return new Regex(@"\G(?:" + regex + ")", _defaultRegexOptions);
            }
        }
    }
}