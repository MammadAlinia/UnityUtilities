
namespace StateMachine
{
    // Define witch state we're moving to based ob which condition
    public interface ITransition
    {
        IState To { get; }
        IPredicate Condition { get; }
    }
}