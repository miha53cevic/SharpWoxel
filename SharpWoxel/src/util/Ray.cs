using OpenTK.Mathematics;

namespace SharpWoxel.util;

internal class Ray(Vector3 start, Vector3 direction)
{
    private readonly Vector3 _start = start;
    private Vector3 _end = start;

    // start - the start position of the ray
    // direction - the direction in which the ray is cast (must be unit vector)

    // samller values in scale, means more precision but takes longer for the for loop to finish
    public void Step(float scale)
    {
        _end += direction * scale;
    }

    public Vector3 GetEnd()
    {
        return _end;
    }

    public float GetLength()
    {
        return Vector3.Distance(_start, _end);
    }
}