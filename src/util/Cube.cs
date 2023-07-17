
namespace SharpWoxel.util
{
    static class Cube
    {
        public static readonly float[] verticies = {
            // Back face
            1f,1f,0f,
            1f,0f,0f,
            0f,0f,0f,
            0f,1f,0f,

            // Front face
            0f,1f,1f,
            0f,0f,1f,
            1f,0f,1f,
            1f,1f,1f,

            // Right face
            1f,1f,1f,
            1f,0f,1f,
            1f,0f,0f,
            1f,1f,0f,

            // Left Face
            0f,1f,0f,
            0f,0f,0f,
            0f,0f,1f,
            0f,1f,1f,

            // Top face
            0f,1f,1f,
            1f,1f,1f,
            1f,1f,0f,
            0f,1f,0f,

            // Bottom face
            0f,0f,1f,
            0f,0f,0f,
            1f,0f,0f,
            1f,0f,1f
        };

        public static readonly float[] textureCoordinates = {
            1f,1f,
            1f,0f,
            0f,0f,
            0f,1f,

            1f,1f,
            1f,0f,
            0f,0f,
            0f,1f,

            1f,1f,
            1f,0f,
            0f,0f,
            0f,1f,

            1f,1f,
            1f,0f,
            0f,0f,
            0f,1f,

            1f,1f,
            1f,0f,
            0f,0f,
            0f,1f,

            1f,1f,
            1f,0f,
            0f,0f,
            0f,1f
        };

        public static readonly uint[] indicies = {
            0,1,3,
            3,1,2,
            4,5,7,
            7,5,6,
            8,9,11,
            11,9,10,
            12,13,15,
            15,13,14,
            16,17,19,
            19,17,18,
            20,21,23,
            23,21,22
        };
    }
}
