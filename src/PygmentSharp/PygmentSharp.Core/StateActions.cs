using System.Collections.Generic;

namespace PygmentSharp.Core
{
    public abstract class StateAction
    {
        public abstract void Apply(Stack<string> stateStack);
    }
    public class PushStateAction : StateAction
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

    public class CombinedAction : StateAction
    {
        public IReadOnlyCollection<StateAction> Actions { get; }
        public CombinedAction(params StateAction[] actions)
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

    public class NoopAction : StateAction
    {
        public override void Apply(Stack<string> stateStack)
        {
            // this line intentionally left blank
        }
    }

    public class PushAgainAction : StateAction
    {
        public override void Apply(Stack<string> stateStack)
        {
            stateStack.Push(stateStack.Peek());
        }
    }

    public class PopAction : StateAction
    {
        public override void Apply(Stack<string> stateStack)
        {
            stateStack.Pop();
        }
    }
}