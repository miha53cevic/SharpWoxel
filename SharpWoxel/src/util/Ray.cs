using OpenTK.Mathematics;

namespace SharpWoxel.Util;

class Ray
{
    private Vector3 _start;
    private Vector3 _end;
    private Vector3 _direction;

    // start - the start position of the ray
    // direction - the direction in which the ray is cast (must be unit vector)
    public Ray(Vector3 start, Vector3 direction)
    {
        _start = start;
        _end = start;
        _direction = direction;
    }

    // samller values in scale, means more precision but takes longer for the for loop to finish
    public void Step(float scale)
    {
        _end += _direction * scale;
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
