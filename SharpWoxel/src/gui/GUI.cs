using OpenTK.Mathematics;

namespace SharpWoxel.Gui;

class GUI
{
    private static GUI _instance = new GUI();
    private Matrix4 _ortho = Matrix4.Identity;
    private bool _initialized;

    static GUI()
    {
    }

    private GUI()
    {
        _initialized = false;
    }

    public static void Init(int screenWidth, int screenHeight)
    {
        // jer je bottom 0 a top height, (0,0) je doljnji lijevi kut, a (width,height) gornji desni (opet OpenTK ne radi s obrnutim idk why)
        _instance._ortho = Matrix4.CreateOrthographicOffCenter(0.0f, (float)screenWidth, 0.0f, (float)screenHeight, -1.0f, 1.0f);
        _instance._initialized = true;
    }

    public static GUI GetInstance()
    {
        if (!_instance._initialized)
            throw new Exception("Called GetInstance() before Init()");
        return _instance;
    }

    public static Rect CreateRect()
    {
        return new Rect(_instance._ortho);
    }
}
