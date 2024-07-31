using OpenTK.Mathematics;

namespace SharpWoxel.gui;

internal class Gui
{
    private static readonly Gui Instance = new();
    private bool _initialized;
    private Matrix4 _ortho = Matrix4.Identity;

    static Gui()
    {
    }

    private Gui()
    {
        _initialized = false;
    }

    public static void Init(int screenWidth, int screenHeight)
    {
        // jer je bottom 0 a top height, (0,0) je doljnji lijevi kut, a (width,height) gornji desni (opet OpenTK ne radi s obrnutim idk why)
        Instance._ortho =
            Matrix4.CreateOrthographicOffCenter(0.0f, screenWidth, 0.0f, screenHeight, -1.0f, 1.0f);
        Instance._initialized = true;
    }

    public static Gui GetInstance()
    {
        if (!Instance._initialized)
            throw new Exception("Called GetInstance() before Init()");
        return Instance;
    }

    public static Rect CreateRect()
    {
        return new Rect(Instance._ortho);
    }
}