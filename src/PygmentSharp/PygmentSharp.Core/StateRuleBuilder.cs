using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using JetBrains.Annotations;

namespace PygmentSharp.Core
{
    public class StateRuleBuilder
    {
        public RegexOptions DefaultRegexOptions { get; set; }

        public StateRuleBuilder()
        {
            DefaultRegexOptions = RegexOptions.None;
        }

        public FluentStateRuleBuilder NewRuleSet()
        {
            return new FluentStateRuleBuilder(DefaultRegexOptions);
        }

        public class FluentStateRuleBuilder
        {
            private readonly RegexOptions _defaultRegexOptions;
            private readonly List<StateRule> _rules = new List<StateRule>();
            public StateRule[] Build() => _rules.ToArray();

            public FluentStateRuleBuilder(RegexOptions defaultRegexOptions)
            {
                _defaultRegexOptions = defaultRegexOptions;
            }

            public FluentStateRuleBuilder Add([RegexPattern]string regex, TokenType tokenType, string stateName)
            {
                _rules.Add(new StateRule(CreateRegex(regex), tokenType, Parse(stateName)));
                return this;
            }

            public FluentStateRuleBuilder Add([RegexPattern]string regex, TokenType tokenType, params string[] rules)
            {
                _rules.Add(new StateRule(CreateRegex(regex), tokenType,
                    new CombinedAction(rules.Select(Parse).ToArray())));
                return this;
            }

            public FluentStateRuleBuilder Add([RegexPattern]string regex, TokenType tokenType)
            {
                _rules.Add(new StateRule(CreateRegex(regex), tokenType, new NoopAction()));
                return this;
            }

            public FluentStateRuleBuilder Default(params string[] states)
            {
                _rules.Add(new StateRule(CreateRegex(""), TokenTypes.Token,
                    new CombinedAction(states.Select(Parse).ToArray())));
                return this;
            }

            public FluentStateRuleBuilder Include(StateRule[] existing, params StateRule[] newRules)
            {
                _rules.AddRange(existing);
                _rules.AddRange(newRules);
                return this;
            }

            public FluentStateRuleBuilder ByGroups([RegexPattern]string regex, params GroupProcessor[] processors)
            {
                _rules.Add(new StateRule(CreateRegex(regex), TokenTypes.Token,
                    new GroupAction(processors)));
                return this;
            }

            public FluentStateRuleBuilder ByGroups([RegexPattern]string regex, string newState, params GroupProcessor[] processors)
            {
                _rules.Add(new StateRule(CreateRegex(regex), TokenTypes.Token,
                    new GroupAction(Parse(newState), processors)));
                return this;
            }

            public FluentStateRuleBuilder ByGroups([RegexPattern]string regex, string[] newStates, params GroupProcessor[] processors)
            {
                var actions = newStates.Select(Parse).ToArray();
                _rules.Add(new StateRule(CreateRegex(regex), TokenTypes.Token,
                    new GroupAction(new CombinedAction(actions), processors)));
                return this;
            }

            public FluentStateRuleBuilder Using<T>([RegexPattern]string regex) where T : Lexer, new()
            {
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
                return new Regex(@"\G(?:" + regex + ")", _defaultRegexOptions);
            }
        }
    }
}