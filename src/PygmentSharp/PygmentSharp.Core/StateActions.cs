using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PygmentSharp.Core
{
    public abstract class StateAction
    {
        public abstract IEnumerable<Token> Execute(RegexLexerContext context);
    }

    public class GroupAction : StateAction
    {
        public StateChangingAction Action { get; set; }
        public GroupProcessor[] Processors { get; }

        public GroupAction(params GroupProcessor[] processors)
        {
            Action = new NoopAction();
            Processors = processors;
        }

        public GroupAction(StateChangingAction action, params GroupProcessor[] processors)
            : this(processors)
        {
            Action = action ?? new NoopAction();
        }

        public override IEnumerable<Token> Execute(RegexLexerContext context)
        {
            for(int i = 1; i < context.Match.Groups.Count; i++)
            {
                var group = context.Match.Groups[i];
                var tokens = Processors[i-1].GetTokens(context, group.Value);
                foreach (var token in tokens)
                    yield return token;
            }

            Action.Apply(context.StateStack);
        }
    }

    public class LexerAction : StateAction
    {
        public Lexer Lexer { get; }

        public LexerAction(Lexer lexer)
        {
            Lexer = lexer;
        }

        public override IEnumerable<Token> Execute(RegexLexerContext context)
        {
            int offset = context.Position;

            var tokens = Lexer.GetTokens(context.Match.Value);
            foreach (var token in tokens)
                yield return token.Offset(offset);

            context.Position += context.Match.Length;
        }
    }

    public abstract class StateChangingAction : StateAction
    {
        public abstract void Apply(Stack<string> stateStack);

        public override IEnumerable<Token> Execute(RegexLexerContext context)
        {
            if(context.Match.Value != "")
                yield return new Token(context.Position, context.RuleTokenType, context.Match.Value);

            Apply(context.StateStack);
            context.Position += context.Match.Length;
        }
    }

    public class PushStateAction : StateChangingAction
    {
        public string DestinationState { get; }

        public PushStateAction(string destinationState)
        {
            DestinationState = destinationState;
        }

        public override void Apply(Stack<string> stateStack)
        {
            stateStack.Push(DestinationState);
        }
    }

    public class CombinedAction : StateChangingAction
    {
        public IReadOnlyCollection<StateChangingAction> Actions { get; }
        public CombinedAction(params StateChangingAction[] actions)
        {
            Actions = actions;
        }

        public override void Apply(Stack<string> stateStack)
        {
            foreach (var action in Actions)
            {
                action.Apply(stateStack);
            }
        }
    }

    public class NoopAction : StateChangingAction
    {
        public override void Apply(Stack<string> stateStack)
        {
            // this line intentionally left blank
        }
    }

    public class PushAgainAction : StateChangingAction
    {
        public override void Apply(Stack<string> stateStack)
        {
            stateStack.Push(stateStack.Peek());
        }
    }

    public class PopAction : StateChangingAction
    {
        public override void Apply(Stack<string> stateStack)
        {
            stateStack.Pop();
        }
    }

}