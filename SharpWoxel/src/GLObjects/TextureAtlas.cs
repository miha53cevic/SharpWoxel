using OpenTK.Graphics.OpenGL4;

namespace SharpWoxel.GLObjects;

internal class TextureAtlas
{
    private readonly float _individualTextureSize;
    private readonly float _pixelSize;
    private readonly Texture _texture;

    public TextureAtlas(string path, int imageSize, int individualTextureSize)
    {
        _texture = Texture.LoadFromFile(path);

        var texturesPerRow = imageSize / (float)individualTextureSize;
        _individualTextureSize = 1.0f / texturesPerRow;
        _pixelSize = 1.0f / imageSize;
    }

    // Return individualTexture coordinates at a given position in the 2D grid atlas
    public float[] GetTextureCoords(int x, int y)
    {
        var xMin = x * _individualTextureSize + 0.5f * _pixelSize;
        var yMin = y * _individualTextureSize + 0.5f * _pixelSize;

        var xMax = xMin + _individualTextureSize - _pixelSize;
        var yMax = yMin + _individualTextureSize - _pixelSize;

        // In opengl texture coordinates are like in math, bottom left is (0,0) and top right is (1,1)
        yMin = 1 - yMin;
        yMax = 1 - yMax;

        // A texture has 4 vec2 coordinates (4 * 2 = 8 values)
        float[] coords = [xMin, yMin, xMin, yMax, xMax, yMax, xMax, yMin];

        return coords;
    }

    public void Use(TextureUnit unit)
    {
        _texture.Use(unit);
    }
}