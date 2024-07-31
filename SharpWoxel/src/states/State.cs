namespace SharpWoxel.states;

internal abstract class State(Game game)
{
    protected readonly Game GameRef = game;

    public abstract void Setup();
    public abstract void OnUpdateFrame(double deltaTime); // fixed updates
    public abstract void OnRenderFrame(double deltaTime); // rendering (as fast as possible)
    public abstract void Pause();
    public abstract void Resume();
    public abstract void OnExit();
}