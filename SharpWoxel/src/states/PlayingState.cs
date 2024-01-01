using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SharpWoxel.Entities;
using SharpWoxel.Player;
using SharpWoxel.Player.Inventory;
using SharpWoxel.Util;
using SharpWoxel.World;
using SharpWoxel.World.Terrain;

namespace SharpWoxel.States;

class PlayingState : State
{
    private bool _paused;
    private Camera _camera;
    private PlayerController _playerController;
    private WorldModel _world;
    private InventoryRenderer _inventoryRenderer;
    private VoxelOutline _voxelOutline;

    public PlayingState(Game game)
        : base(game)
    {
        _paused = false;
        _camera = new Camera(new Vector3(0, 0, 0), (float)game.RenderResolution.X / (float)game.RenderResolution.Y);

        var playerInventory = new PlayerInventory(8);
        _playerController = new PlayerController(_camera, playerInventory);
        _inventoryRenderer = new InventoryRenderer(playerInventory);
        _inventoryRenderer.SetRenderPosition((_gameRef.RenderResolution.X / 2, 64));
        _inventoryRenderer.CenterOnPosition();

        _voxelOutline = new VoxelOutline();

        BaseTerrain testTerrain = new TestTerrain(new Vector3i(3, 3, 3), new Vector3i(32, 32, 32));
        _world = new WorldModel(testTerrain);
    }
    public override void Setup()
    {
        _playerController.Camera.Position = (1.5f * 32.0f, 1.5f * 32.0f, 1.5f * 32.0f);
        _world.GenerateWorld();
    }

    private void HandleWindowFocus()
    {
        // Lock cursor (and hide it) to the screen if the window is in focus
        if (_gameRef.IsFocused)
        {
            _gameRef.CursorState = CursorState.Grabbed;
        }
        else
        {
            _gameRef.CursorState = CursorState.Normal;
            return; // exit function if window is not in focus
        }
    }

    public override void OnUpdateFrame(double deltaTime)
    {
        // Only render the scene when paused, don't do updates
        if (_paused) return;

        HandleWindowFocus();

        // Pause the game
        var input = _gameRef.KeyboardState;
        if (input.IsKeyPressed(Keys.Escape))
        {
            StateManager.GetInstance().Add(new PausedState(_gameRef));
            return;
        }

        // Update
        _playerController.Update(deltaTime, _gameRef.KeyboardState, _gameRef.MouseState);

        // Update voxelOutline Position
        var blockPosition = _world.CastRayFirstBlockIntersection(_playerController.Camera);
        if (blockPosition.HasValue)
        {
            var blockPos = blockPosition.Value;
            _voxelOutline.Position = ((float)blockPos.X, (float)blockPos.Y, (float)blockPos.Z);
        }
    }

    public override void OnRenderFrame(double deltaTime)
    {
        _world.Render(ShaderLoader.GetInstance().GetShader("basic"), _playerController.Camera);
        _inventoryRenderer.Render(ShaderLoader.GetInstance().GetShader("gui"));
        _voxelOutline.Render(ShaderLoader.GetInstance().GetShader("voxelOutline"), _playerController.Camera);
    }

    public override void Pause()
    {
        _paused = true;
    }

    public override void Resume()
    {
        _paused = false;
    }

    public override void OnExit()
    {
    }
}
