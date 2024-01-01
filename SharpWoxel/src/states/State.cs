
namespace SharpWoxel.States;

abstract class State
{
    protected Game _gameRef;

    public State(Game game)
    {
        _gameRef = game;
    }

    public abstract void Setup();
    public abstract void OnUpdateFrame(double deltaTime); // fixed updates
    public abstract void OnRenderFrame(double deltaTime); // rendering (as fast as possible)
    public abstract void Pause();
    public abstract void Resume();
    public abstract void OnExit();
}
