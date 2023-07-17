using OpenTK.Mathematics;

namespace SharpWoxel.util
{
    static class Maths
    {
        public static Matrix4 CreateTransformationMatrix(Vector3 translation, Vector3 rotation, Vector3 scale)
        {
            Matrix4 mat = Matrix4.Identity;
            mat *= Matrix4.CreateTranslation(translation);
            mat *= Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(rotation.X));
            mat *= Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(rotation.Y));
            mat *= Matrix4.CreateRotationZ((float)MathHelper.DegreesToRadians(rotation.Z));
            mat *= Matrix4.CreateScale(scale);
            return mat;
        }

        public static Matrix4 CreateViewMatrix(Camera camera)
        {
            return camera.GetViewMatrix();
        }

        public static Matrix4 CreateProjectionMatrix(float fov, float aspect, float depthNear, float depthFar)
        {
            return Matrix4.CreatePerspectiveFieldOfView(fov, aspect, depthNear, depthFar);
        }

        public static Matrix4 CreateProjectionMatrix(Camera camera)
        {
            return camera.GetProjectionMatrix();
        }

        public static Matrix4 CreateMVPMatrix(Matrix4 projection, Matrix4 view, Matrix4 model)
        {
            // MVP = Projection * View * Model(transformation matrix) ako je column based (opengl)
            // OpenTK je row based pa mora biti transposed unutar glUniformMatrix4() i mnozi se
            // Model * View * Projection
            return model * view * projection;
        }

        public static Matrix4 CreateMVPMatrix(Camera camera, Matrix4 model)
        {
            // MVP = Projection * View * Model(transformation matrix) ako je column based (opengl)
            // OpenTK je row based pa mora biti transposed unutar glUniformMatrix4() i mnozi se
            // Model * View * Projection
            return model * camera.GetViewMatrix() * camera.GetProjectionMatrix();
        }

        public static int IndexFrom3D(int x, int y, int z, int ySize, int zSize)
        {
            // Index = ((x * YSIZE + y) * ZSIZE) + z;
            return ((x * ySize + y) * zSize) + z;
        }
    }
}
