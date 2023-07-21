using glObjects;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SharpWoxel.entities;
using SharpWoxel.player;
using SharpWoxel.util;
using SharpWoxel.world;
using SharpWoxel.world.terrain;

namespace SharpWoxel.states
{
    class PlayingState : State
    {
        private Shader _basicShader;
        private Camera _camera;
        private SimpleEntity _testEntity;
        private PlayerController _playerController;
        private WorldModel _world;

        public PlayingState(Game game) 
            : base(game)
        {
            _basicShader = new Shader("../../../shaders/basic.vert", "../../../shaders/basic.frag");
            _camera = new Camera(new Vector3(0, 0, 0), (float)game.Size.X / (float)game.Size.Y);
            _testEntity = new SimpleEntity("../../../res/test.png");
            _playerController = new PlayerController(_camera);
            Terrain flatTerrain = new FlatTerrain(new Vector3i(2, 1, 2), new Vector3i(32, 32, 32));
            Terrain testTerrain = new TestTerrain(new Vector3i(3, 3, 3), new Vector3i(32, 32, 32));
            _world = new WorldModel(testTerrain);
        }
        public override void Setup()
        {
            _playerController.Camera.Position = (1.5f * 32.0f, 1.5f * 32.0f, 1.5f * 32.0f);
            _testEntity.SetVerticies(Cube.verticies, BufferUsageHint.StaticDraw);
            _testEntity.SetTextureCoords(Cube.textureCoordinates, BufferUsageHint.StaticDraw);
            _testEntity.SetIndicies(Cube.indicies, BufferUsageHint.StaticDraw);
            _testEntity.Position = _playerController.Camera.Position;
            _testEntity.Position += (0, 0, -3);
            _world.GenerateWorld();
        }

        public override void OnUpdateFrame(double deltaTime)
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

            // Pause the game
            var input = _gameRef.KeyboardState;
            if (input.IsKeyPressed(Keys.Escape))
            {
                _gameRef.GetStateManager().Add(new PausedState(_gameRef));
            }

            // Update
            _playerController.Update(deltaTime, _gameRef.KeyboardState, _gameRef.MouseState);
        }

        public override void OnRenderFrame(double deltaTime)
        {
            _testEntity.Render(_basicShader, _playerController.Camera);
            _world.Render(_basicShader, _playerController.Camera);
        }

        public override void Pause()
        {
            
        }

        public override void Resume()
        {
            
        }

        public override void OnExit()
        {
            _basicShader.Dispose();
        }
    }
}
