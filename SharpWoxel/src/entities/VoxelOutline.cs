using SharpWoxel.GLObjects;
using SharpWoxel.mesh;
using SharpWoxel.util;

namespace SharpWoxel.entities;

internal class VoxelOutline : Entity
{
    private static readonly CubeOutlineMesh Mesh = new();

    public VoxelOutline()
    {
        Scale *= (1.01f, 1.01f, 1.01f);
    }

    public override void Render(Shader shader, Camera camera)
    {
        shader.Use();
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