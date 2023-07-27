using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SharpWoxel.player;
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

        public PlayingState(Game game) 
            : base(game)
        {
            _paused = false;
            _camera = new Camera(new Vector3(0, 0, 0), (float)game.RenderResolution.X / (float)game.RenderResolution.Y);
            _playerController = new PlayerController(_camera);
            Terrain flatTerrain = new FlatTerrain(new Vector3i(2, 1, 2), new Vector3i(32, 32, 32));
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
            _playerController.RenderInventory(ShaderLoader.GetInstance().GetShader("gui"));
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
