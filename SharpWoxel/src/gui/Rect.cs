using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using SharpWoxel.GLObjects;
using SharpWoxel.mesh;

namespace SharpWoxel.gui;

internal class Rect(Matrix4 projection)
{
    public Vector2i Position { get; set; } = Vector2i.Zero;
    public Vector2i Size { get; set; } = Vector2i.One;
    public float Rotation { get; set; } = 0.0f;

    public readonly RectMesh Mesh = new RectMesh();

    public void CenterOnPosition()
    {
        Position = (Position.X - Size.X / 2, Position.Y - Size.Y / 2);
    }

    // Note: Starting position is bottom left cornor and is regarded as (0,0)
    public void Render(Shader shader)
    {
        // OpenTK - Scale -> Rotation -> Transform
        var model = Matrix4.Identity;
        model *= Matrix4.CreateScale(Size.X, Size.Y, 1.0f);
        model *= Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Rotation));
        model *= Matrix4.CreateTranslation(new Vector3(Position.X, Position.Y, 0.0f));

        shader.Use();
        // OpenTK is row-based, so we multiply like this instead of projection*model which would be in c++
        shader.SetMatrix4(shader.GetUniformLocation("modelProjection"), model * projection);

        // Disable depth check (causes overlapping on transparent textures)
        GL.Disable(EnableCap.DepthTest);
        Mesh.Render();
        GL.Enable(EnableCap.DepthTest);
    }
}