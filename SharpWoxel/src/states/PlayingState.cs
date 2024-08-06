using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SharpWoxel.entities;
using SharpWoxel.player;
using SharpWoxel.player.inventory;
using SharpWoxel.util;
using SharpWoxel.world;
using SharpWoxel.world.terrain;

namespace SharpWoxel.states;

internal class PlayingState : State
{
    private readonly InventoryRenderer _inventoryRenderer;
    private readonly PlayerController _playerController;
    private readonly VoxelOutline _voxelOutline;
    private readonly WorldModel _world;
    private bool _paused;

    public PlayingState(Game game)
        : base(game)
    {
        _paused = false;
        var camera = new Camera(new Vector3(0, 0, 0), game.RenderResolution.X / (float)game.RenderResolution.Y);

        var playerInventory = new PlayerInventory(8);
        _playerController = new PlayerController(camera, playerInventory);
        _inventoryRenderer = new InventoryRenderer(playerInventory);
        _inventoryRenderer.SetRenderPosition((GameRef.RenderResolution.X / 2, 64));
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
        GameRef.CursorState = GameRef.IsFocused ? CursorState.Grabbed : CursorState.Normal;
    }

    public override void OnUpdateFrame(double deltaTime)
    {
        // Only render the scene when paused, don't do updates
        if (_paused) return;

        HandleWindowFocus();

        // Pause the game
        var input = GameRef.KeyboardState;
        if (input.IsKeyPressed(Keys.Escape))
        {
            StateManager.GetInstance().Add(new PausedState(GameRef));
            return;
        }

        // Update
        _playerController.Update(deltaTime, GameRef.KeyboardState, GameRef.MouseState);

        // Update voxelOutline Position
        UpdateLookAtVoxel();

        HandlePlayerBlockInteraction();
    }

    private void HandlePlayerBlockInteraction()
    {
        var mouseState = GameRef.MouseState;

        if (mouseState.IsButtonPressed(MouseButton.Button1))
        {
            if (_voxelOutline.Hidden) return;
            _world.BreakBlock((Vector3i)_voxelOutline.Position);
        }

        if (mouseState.IsButtonPressed(MouseButton.Button2))
        {
            var selectedItem = _playerController.PlayerInventory.GetSelected().Item;

            if (_voxelOutline.Hidden || selectedItem == null) return;
            _world.PlaceBlock((Vector3i)_voxelOutline.Position, selectedItem);
        }
    }

    private void UpdateLookAtVoxel()
    {
        var blockPosition = _world.CastRayToFirstBlock(_playerController.Camera);
        if (blockPosition.HasValue)
        {
            var blockPos = blockPosition.Value;
            _voxelOutline.Position = (blockPos.X, blockPos.Y, blockPos.Z);
            _voxelOutline.Hidden = false;
        }
        else _voxelOutline.Hidden = true;
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