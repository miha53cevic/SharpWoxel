using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using SharpWoxel.GLObjects;

namespace SharpWoxel.gui;

internal class Rect
{
    // Verticies and texture coords are the same in the current order
    public static readonly float[] Verticies =
    [
        0f, 1f,
        0f, 0f,
        1f, 0f,
        1f, 1f
    ];

    public static readonly uint[] Indicies =
    [
        0, 1, 3,
        3, 1, 2
    ];

    private readonly Ebo _ebo;
    private readonly Matrix4 _projection;
    private readonly Vbo _textureCoordinatesVbo;
    private readonly Vao _vao;
    private readonly Vbo _verticiesVbo;

    public Rect(Matrix4 projection)
    {
        _vao = new Vao();
        _verticiesVbo = new Vbo();
        _textureCoordinatesVbo = new Vbo();
        _ebo = new Ebo();
        _projection = projection;

        PrepRenderData();
    }

    public Vector2i Position { get; set; } = Vector2i.Zero;
    public Vector2i Size { get; set; } = Vector2i.One;
    public float Rotation { get; set; } = 0.0f;

    private void PrepRenderData()
    {
        _vao.Bind();
        _verticiesVbo.SetBufferData(Verticies, BufferUsageHint.StaticDraw);
        _ebo.SetElementBufferData(Indicies, BufferUsageHint.StaticDraw);
        _vao.Unbind();

        _vao.DefineVertexAttribPointer(_verticiesVbo, 0, 2, 2 * sizeof(float), 0); // verticies
        _vao.DefineVertexAttribPointer(_verticiesVbo, 1, 2, 2 * sizeof(float),
            0); // textureCoordinates for normal texture are the same as verticies
    }

    // Set custom texture coordinates when using TextureAtlas 
    public void SetTextureCoordinates(float[] textureCoordinates)
    {
        _vao.Bind();
        _textureCoordinatesVbo.SetBufferData(textureCoordinates, BufferUsageHint.StaticDraw);
        _vao.Unbind();

        _vao.DefineVertexAttribPointer(_textureCoordinatesVbo, 1, 2, 2 * sizeof(float), 0);
    }

    public void SetDefaultTextureCoordinates()
    {
        _vao.DefineVertexAttribPointer(_verticiesVbo, 1, 2, 2 * sizeof(float), 0);
    }

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
        shader.SetMatrix4(shader.GetUniformLocation("modelProjection"), model * _projection);

        // Disable depth check (causes overlapping on transparent textures)
        GL.Disable(EnableCap.DepthTest);

        _vao.Bind();
        GL.DrawElements(BeginMode.Triangles, _ebo.Size, DrawElementsType.UnsignedInt, 0);
        _vao.Unbind();

        GL.Enable(EnableCap.DepthTest);
    }
}