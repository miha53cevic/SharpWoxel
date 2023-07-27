using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SharpWoxel.player;
using SharpWoxel.player.inventory;
using SharpWoxel.util;
using SharpWoxel.world;
using SharpWoxel.world.terrain;

namespace SharpWoxel.states
{
    class PlayingState : State
    {
        private bool _paused;
        private Camera _camera;
        private PlayerController _playerController;
        private WorldModel _world;
        private InventoryRenderer _inventoryRenderer;

        public PlayingState(Game game) 
            : base(game)
        {
            _paused = false;
            _camera = new Camera(new Vector3(0, 0, 0), (float)game.RenderResolution.X / (float)game.RenderResolution.Y);

            var playerInventory = new Inventory(8);
            _playerController = new PlayerController(_camera, playerInventory);
            _inventoryRenderer = new InventoryRenderer(playerInventory);
            _inventoryRenderer.SetInventoryPosition((_gameRef.RenderResolution.X / 2, 64));
            _inventoryRenderer.CenterOnPosition();
            
            Terrain testTerrain = new TestTerrain(new Vector3i(3, 3, 3), new Vector3i(32, 32, 32));
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
            }

            // Update
            _playerController.Update(deltaTime, _gameRef.KeyboardState, _gameRef.MouseState);
        }

        public override void OnRenderFrame(double deltaTime)
        {
            _world.Render(ShaderLoader.GetInstance().GetShader("basic"), _playerController.Camera);
            _inventoryRenderer.Render(ShaderLoader.GetInstance().GetShader("gui"));
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
}
