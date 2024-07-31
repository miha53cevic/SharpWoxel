using OpenTK.Mathematics;
using SharpWoxel.GLObjects;
using SharpWoxel.util;

namespace SharpWoxel.entities;

internal abstract class Entity
{
    public Vector3 Position { get; set; } = Vector3.Zero;
    public Vector3 Rotation { get; set; } = Vector3.Zero;
    public Vector3 Scale { get; set; } = Vector3.One;

    public abstract void Render(Shader shader, Camera camera);
    public abstract void Update(double deltaTime);
}