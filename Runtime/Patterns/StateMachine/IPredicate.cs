namespace StateMachine
{
    // A Predicate is a function that test a condition and then return a boolean value - true or false

    public interface IPredicate
    {
        bool Evaluate();
    }
}