using glObjects;
using OpenTK.Mathematics;
using SharpWoxel.util;

namespace SharpWoxel.entities
{
    interface IEntity
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Scale { get; set; }
        public void Render(Shader shader, Camera camera);
        public void Update(double deltaTime);
    }
}
