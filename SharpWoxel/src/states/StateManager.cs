﻿
namespace SharpWoxel.States;

class StateManager
{
    private static StateManager _instance = new StateManager();
    private List<State> _states;

    static StateManager()
    {
    }

    private StateManager()
    {
        _states = new List<State>();
    }

    public static StateManager GetInstance() { return _instance; }

    public void Add(State state, bool replace = false)
    {
        if (replace)
        {
            if (Size() > 0)
            {
                throw new Exception("Trying to replace scene with empty stack");
            }
            _states.Last().OnExit();
            state.Setup();
            _states[Size() - 1] = state;
        }
        else
        {
            if (Size() > 0)
            {
                _states.Last().Pause();
            }

            state.Setup();
            _states.Add(state);
        }
        Console.WriteLine(string.Format("[StateManager]: Adding state {0} (replace={1})", state.GetType(), replace));
    }

    public void Pop()
    {
        if (_states.Count > 0)
        {
            Console.WriteLine(string.Format("[StateManager]: Removing state {0}", _states.Last().GetType()));

            _states.Last().OnExit();
            _states.Remove(_states.Last());

            if (Size() > 0)
                _states.Last().Resume();
        }
        else throw new InvalidOperationException("[State]: Stack is empty");
    }

    public int Size()
    {
        return _states.Count;
    }

    public List<State> GetActiveStates()
    {
        if (_states.Count > 0)
        {
            // Send back copy, so when we remove an element it doesn't break
            return _states.ToList();
        }
        else throw new InvalidOperationException("[State]: Stack is empty");
    }

    public State GetCurrentState()
    {
        if (_states.Count > 0)
        {
            return _states.Last();
        }
        else throw new InvalidOperationException("[State]: Stack is empty");
    }

    public void Destroy()
    {
        _states.Clear();
    }
}
