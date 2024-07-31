using OpenTK.Graphics.OpenGL4;
using SharpWoxel.GLObjects;
using SharpWoxel.mesh;
using SharpWoxel.util;

namespace SharpWoxel.entities;

internal class ChunkEntity : Entity
{
    public static readonly TextureAtlas TexAtlas; // all the chunks use the same atlas
    public readonly ChunkMesh Mesh = new([], [], [], BufferUsageHint.DynamicDraw);

    static ChunkEntity()
    {
        TexAtlas = new TextureAtlas("../../../res/textureAtlas.png", 2048, 256);
    }

    public override void Render(Shader shader, Camera camera)
    {
        shader.Use();
        TexAtlas.Use(TextureUnit.Texture0);

        var model = Maths.CreateTransformationMatrix(
            Position,
            Rotation,
            Scale
        );
        shader.SetMatrix4(shader.GetUniformLocation("mvp"), Maths.CreateMvpMatrix(camera, model));
        Mesh.Render();
    }

    public override void Update(double deltaTime)
    {
    }
}