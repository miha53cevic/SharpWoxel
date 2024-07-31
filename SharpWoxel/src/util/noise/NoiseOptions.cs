namespace SharpWoxel.util.noise;

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
internal class NoiseOptions
{
    public int Octaves { get; set; } = 4;
    public float Frequency { get; set; } = 0.25f;
    public float Amplitude { get; set; } = 0.5f;
}