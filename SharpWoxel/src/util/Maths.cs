using OpenTK.Mathematics;

namespace SharpWoxel.util;

internal static class Maths
{
    public static Matrix4 CreateTransformationMatrix(Vector3 translation, Vector3 rotation, Vector3 scale)
    {
        // OpenTK je obrnut opet radi Row-Based, ide scale, rotation, translation
        var mat = Matrix4.Identity;
        mat *= Matrix4.CreateScale(scale);
        mat *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(rotation.X));
        mat *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotation.Y));
        mat *= Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rotation.Z));
        mat *= Matrix4.CreateTranslation(translation);

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

    public static Matrix4 CreateMvpMatrix(Matrix4 projection, Matrix4 view, Matrix4 model)
    {
        // MVP = Projection * View * Model(transformation matrix) ako je column based (opengl)
        // OpenTK je row based pa mora biti transposed unutar glUniformMatrix4() i mnozi se
        // Model * View * Projection
        return model * view * projection;
    }

    public static Matrix4 CreateMvpMatrix(Camera camera, Matrix4 model)
    {
        // MVP = Projection * View * Model(transformation matrix) ako je column based (opengl)
        // OpenTK je row based pa mora biti transposed unutar glUniformMatrix4() i prvo transponiraj svaku pa mnozi
        // Model * View * Projection
        var view = camera.GetViewMatrix();
        var projection = camera.GetProjectionMatrix();
        return model * view * projection;
    }

    public static int IndexFrom3D(int x, int y, int z, int ySize, int zSize)
    {
        // Index = ((x * YSIZE + y) * ZSIZE) + z;
        return (x * ySize + y) * zSize + z;
    }
}