using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using JetBrains.Annotations;

namespace PygmentSharp.Core
{
    public class StateRuleBuilder
    {
        public StateRuleBuilder()
        {
            DefaultRegexOptions = RegexOptions.None;
        }

        public StateRule Create([RegexPattern]string regex, TokenType tokenType, string stateName)
        {
            return new StateRule(CreateRegex(regex), tokenType, Parse(stateName));
        }
        public StateRule Create([RegexPattern]string regex, TokenType tokenType, params string[] rules)
        {
            return new StateRule(CreateRegex(regex), tokenType,
                new CombinedAction(rules.Select(Parse).ToArray()));
        }

        public StateRule Create([RegexPattern]string regex, TokenType tokenType)
        {
            return new StateRule(CreateRegex(regex), tokenType, new NoopAction());
        }

        public StateRule Default(params string[] states)
        {
            return new StateRule(CreateRegex(""), TokenTypes.Token,
                new CombinedAction(states.Select(Parse).ToArray()));
        }

        public StateRule[] Include(StateRule[] existing, params StateRule[] newRules)
        {
            return existing.Concat(newRules).ToArray();
        }

        private StateChangingAction Parse(string name)
        {
            if (name == "#push")
                return new PushAgainAction();
            if (name == "#pop")
                return new PopAction();

            return new PushStateAction(name);
        }

        public StateRule ByGroups([RegexPattern]string regex,  params GroupProcessor[] processors)
        {
            return new StateRule(CreateRegex(regex), TokenTypes.Token,
                new GroupAction(processors));
        }

        public StateRule ByGroups([RegexPattern]string regex, string newState, params GroupProcessor[] processors)
        {

            return new StateRule(CreateRegex(regex), TokenTypes.Token,
                new GroupAction(Parse(newState), processors));
        }

        public StateRule ByGroups([RegexPattern]string regex, string[] newStates, params GroupProcessor[] processors)
        {
            var actions = newStates.Select(Parse).ToArray();
            return new StateRule(CreateRegex(regex), TokenTypes.Token,
                new GroupAction(new CombinedAction(actions), processors));
        }


        public Regex CreateRegex(string regex)
        {
            return new Regex(@"\G(?:" + regex + ")", DefaultRegexOptions);
        }

        public RegexOptions DefaultRegexOptions { get; set; }

        public StateRule Using<T>([RegexPattern]string regex) where T:Lexer,new()
        {
            var lexer = new T();
            return new StateRule(CreateRegex(regex), TokenTypes.Token, new LexerAction(lexer));
        }
    }
}