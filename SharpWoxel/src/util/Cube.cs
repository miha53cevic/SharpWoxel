namespace SharpWoxel.util;

internal static class Cube
{
    public enum Face
    {
        Top,
        Bottom,
        Left,
        Right,
        Back,
        Front
    }

    public static readonly float[] Verticies =
    [
        // Back face
        1f, 1f, 0f,
        1f, 0f, 0f,
        0f, 0f, 0f,
        0f, 1f, 0f,

        // Front face
        0f, 1f, 1f,
        0f, 0f, 1f,
        1f, 0f, 1f,
        1f, 1f, 1f,

        // Right face
        1f, 1f, 1f,
        1f, 0f, 1f,
        1f, 0f, 0f,
        1f, 1f, 0f,

        // Left Face
        0f, 1f, 0f,
        0f, 0f, 0f,
        0f, 0f, 1f,
        0f, 1f, 1f,

        // Top face
        0f, 1f, 1f,
        1f, 1f, 1f,
        1f, 1f, 0f,
        0f, 1f, 0f,

        // Bottom face
        0f, 0f, 1f,
        0f, 0f, 0f,
        1f, 0f, 0f,
        1f, 0f, 1f
    ];

    public static readonly float[] TextureCoordinates =
    [
        1f, 1f,
        1f, 0f,
        0f, 0f,
        0f, 1f,

        1f, 1f,
        1f, 0f,
        0f, 0f,
        0f, 1f,

        1f, 1f,
        1f, 0f,
        0f, 0f,
        0f, 1f,

        1f, 1f,
        1f, 0f,
        0f, 0f,
        0f, 1f,

        1f, 1f,
        1f, 0f,
        0f, 0f,
        0f, 1f,

        1f, 1f,
        1f, 0f,
        0f, 0f,
        0f, 1f
    ];

    public static readonly uint[] Indicies =
    [
        0, 1, 3,
        3, 1, 2,
        4, 5, 7,
        7, 5, 6,
        8, 9, 11,
        11, 9, 10,
        12, 13, 15,
        15, 13, 14,
        16, 17, 19,
        19, 17, 18,
        20, 21, 23,
        23, 21, 22
    ];

    public static class CubeFace
    {
        public static uint[] Indicies =
        [
            0, 1, 3,
            3, 1, 2
        ];

        public static float[] TextureCoordinates =
        [
            0.0f, 0.0f,
            0.0f, 1.0f,
            1.0f, 1.0f,
            1.0f, 0.0f
        ];

        public static float[] GetCubeFace(Face face)
        {
            return face switch
            {
                Face.Top => [0, 1, 0, 0, 1, 1, 1, 1, 1, 1, 1, 0],
                Face.Bottom => [0, 0, 1, 0, 0, 0, 1, 0, 0, 1, 0, 1],
                Face.Left => [0, 1, 0, 0, 0, 0, 0, 0, 1, 0, 1, 1],
                Face.Right => [1, 1, 1, 1, 0, 1, 1, 0, 0, 1, 1, 0],
                Face.Front => [0, 1, 1, 0, 0, 1, 1, 0, 1, 1, 1, 1],
                Face.Back => [1, 1, 0, 1, 0, 0, 0, 0, 0, 0, 1, 0],
                _ => throw new Exception("Invalid Face for CubeFace given")
            };
        }
    }
}