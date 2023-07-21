
namespace SharpWoxel.states
{
    class StateManager
    {
        private Stack<State> _states;

        public StateManager()
        {
            _states = new Stack<State>();
        }

        public void Add(State state)
        {
            if (Size() > 0) 
            {
                GetCurrentState().Pause();
            }

            state.Setup();
            _states.Push(state);
        }

        public void Pop()
        {
            if ( _states.Count > 0 )
            {
                var state = _states.Pop();
                state.OnExit();
                
                if (Size() > 0)
                    GetCurrentState().Resume();
            }
            else throw new InvalidOperationException("[State]: Stack is empty");
        }

        public int Size()
        {
            return _states.Count;
        }

        public State GetCurrentState()
        {
            if (_states.Count > 0)
            {
                return _states.Peek();
            }
            else throw new InvalidOperationException("[State]: Stack is empty");
        }
    }
}
