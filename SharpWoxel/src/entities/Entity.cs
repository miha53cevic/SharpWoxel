using OpenTK.Mathematics;
using SharpWoxel.GLObjects;
using SharpWoxel.util;

namespace SharpWoxel.entities;

internal abstract class Entity
{
    public Vector3 Position { get; set; } = Vector3.Zero;
    public Vector3 Rotation { get; set; } = Vector3.Zero;
    public Vector3 Scale { get; set; } = Vector3.One;
    public bool Hidden = false;

    public void Render(Shader shader, Camera camera)
    {
        if (Hidden) return;
        SetupRender(shader, camera);
    }

    public abstract void Update(double deltaTime);

    protected abstract void SetupRender(Shader shader, Camera camera);
}