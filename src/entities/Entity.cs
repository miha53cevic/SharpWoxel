using glObjects;
using OpenTK.Mathematics;
using SharpWoxel.util;

namespace SharpWoxel.entities
{
    abstract class Entity
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }

        public Entity()
        {
            Position = Vector3.Zero;
            Rotation = Vector3.Zero;
            Scale = Vector3.One;
        }

        public abstract void Render(Shader shader, Camera camera);
        public abstract void Update(double deltaTime);
    }
}
