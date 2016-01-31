using System.Collections.Generic;

namespace PygmentSharp.Core
{
    public abstract class StateAction
    {

        public abstract Token? Execute(RegexLexerContext context);
    }

    public abstract class StateChangingAction : StateAction
    {
        public abstract void Apply(Stack<string> stateStack);

        public override Token? Execute(RegexLexerContext context)
        {
            Apply(context.StateStack);

            var token = context.Match.Value == ""
                ? default(Token?)
                : new Token(context.Position, context.RuleTokenType, context.Match.Value);

            context.Position += context.Match.Length;

            return token;
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