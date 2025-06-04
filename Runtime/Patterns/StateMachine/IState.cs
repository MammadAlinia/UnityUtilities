namespace StateMachine
{
    public interface IState
    {
        void Init();
        void OnEnter();
        void Update();
        void FixedUpdate();
        void OnExit();
    }
}