using StateMachine;

namespace UnityUtilities.Runtime.Patterns.StateMachine
{
    public abstract class BaseState : IState
    {
     
        protected BaseState()
        {
        }

        public virtual void Init()
        {
            
        }
        public virtual void OnEnter()
        {
            //noop
        }

        public virtual void Update()
        {
            //noop
        }

        public virtual void FixedUpdate()
        {
            //noop
        }

        public virtual void OnExit()
        {
            //noop
        }
    }
}