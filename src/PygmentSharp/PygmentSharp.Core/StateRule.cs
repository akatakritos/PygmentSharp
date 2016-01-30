using System.Linq;
using System.Text.RegularExpressions;

namespace PygmentSharp.Core
{
    public class StateRule
    {
        public Regex Regex { get; }
        public TokenType TokenType { get; }
        public StateAction Action { get; }

        private StateRule(Regex regex, TokenType tokenType, StateAction action)
        {
            Regex = regex;
            TokenType = tokenType;
            Action = action;
        }

        public static StateRule Create(string regex, TokenType tokenType, string stateName)
        {
            return new StateRule(CreateRegex(regex), tokenType, Parse(stateName));
        }
        public static StateRule Create(string regex, TokenType tokenType, params string[] rules)
        {
            return new StateRule(CreateRegex(regex), tokenType,
                new CombinedAction(rules.Select(Parse).ToArray()));
        }

        public static StateRule Create(string regex, TokenType tokenType)
        {
            return new StateRule(CreateRegex(regex), tokenType, new NoopAction());
        }

        public static StateRule Default(params string[] states)
        {
            return new StateRule(CreateRegex(""), TokenTypes.Token,
                new CombinedAction(states.Select(Parse).ToArray()));
        }

        private static StateAction Parse(string name)
        {
            if (name == "#push")
                return new PushAgainAction();
            if (name == "#pop")
                return new PopAction();

            return new PushStateAction(name);
        }

        public static Regex CreateRegex(string regex)
        {
            return new Regex(regex, RegexOptions.Compiled);
        }

    }
}