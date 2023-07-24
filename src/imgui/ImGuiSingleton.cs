using ImGuiNET;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;

namespace SharpWoxel.imgui
{
    class ImGuiSingleton
    {
        private ImGuiController? _controller;
        private List<Action<ImGuiController>> _renderFunctions;
        private static readonly ImGuiSingleton _instance = new ImGuiSingleton();

        static ImGuiSingleton()
        {
        }

        public ImGuiSingleton()
        {
            _renderFunctions = new List<Action<ImGuiController>>();
        }

        public static ImGuiSingleton GetInstance() { return _instance; }

        public void Load(int width, int height)
        {
            _controller = new ImGuiController(width, height);
        }

        public void Render()
        {
            foreach (var renderFunction in _renderFunctions)
            {
                renderFunction(_controller!);
            }
            ImGui.ShowDemoWindow();
            _controller?.Render();
            ImGuiController.CheckGLError("End of frame");
        }

        public void AddRenderFunction(Action<ImGuiController> renderFunction)
        {
            _renderFunctions.Add(renderFunction);
        }

        public void ClearRenderFunctions()
        {
            _renderFunctions.Clear();
        }

        public void Update(GameWindow gameWindow, double deltaTime)
        {
            _controller?.Update(gameWindow, (float)deltaTime);
        }

        public void OnResize(int width, int height)
        {
            _controller?.WindowResized(width, height);
        }

        public void OnTextInput(char c)
        {
            _controller?.PressChar((char)c);
        }

        public void OnMouseScroll(Vector2 offset)
        {
            _controller?.MouseScroll(offset);
        }

        public void Destroy()
        {
            _controller?.DestroyDeviceObjects();
        }
    }
}
