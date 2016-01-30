using System.Collections.Generic;

namespace PygmentSharp.Core
{
    public abstract class RegexLexer : Lexer
    {
        protected override IEnumerable<Token> GetTokensUnprocessed(string text)
        {
            var rules = GetStateRules();
            int pos = 0;
            var stateStack = new Stack<string>(50);
            stateStack.Push("root");
            var currentStateRules = rules[stateStack.Peek()];

            while (true)
            {
                bool found = false;
                foreach (var rule in currentStateRules)
                {
                    var m = rule.Regex.Match(text, pos);
                    if (m.Success)
                    {
                        if (m.Value != "")
                            yield return new Token(pos, rule.TokenType, m.Value);
                        rule.Action.Apply(stateStack);
                        pos += m.Length;
                        currentStateRules = rules[stateStack.Peek()];
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    if (pos >= text.Length)
                        break;

                    if (text[pos] == '\n')
                    {
                        stateStack.Clear();
                        stateStack.Push("root");
                        currentStateRules = rules["root"];
                        yield return new Token(pos, TokenTypes.Text, "\n");
                        pos++;
                        continue;
                    }

                    yield return new Token(pos, TokenTypes.Error, text[pos].ToString());
                    pos++;
                }
            }


        }

        protected abstract IDictionary<string, StateRule[]> GetStateRules();
    }
}