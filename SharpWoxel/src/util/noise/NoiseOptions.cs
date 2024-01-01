
namespace SharpWoxel.Util.Noise;

// Check for explenation on different values
// https://www.youtube.com/watch?v=Z6m7tFztEvw
// https://github.com/klaytonkowalski/example-fractal-noise
// ----------------------------------------------------------------
// Octaves        - how many times you combine the noise
// Frequency      - starting noise frequency 
//                  higher zooms out more, lower values zoom in
// Amplitude      - determines how vertical the map will be
//                  the returning noise will be [0, 1] so we need to set the amp to be between
//                  those two values (default should be half of the max)
class NoiseOptions
{
    public int Octaves { get; set; }
    public float Frequency { get; set; }
    public float Amplitude { get; set; }

    public NoiseOptions()
    {
        Octaves = 4;
        Frequency = 0.25f;
        Amplitude = 0.5f;
    }
}
