
namespace SharpWoxel.util.noise
{
    // Check for explenation on different values
    // https://www.redblobgames.com/maps/terrain-from-noise/#elevation
    // The values in the tutorial above for rougness is 0.5 and smoothness is 2
    // because he keeps divding the amplitude by 2 and increasing the frequency times 2
    // ----------------------------------------------------------------
    // Octaves        - how many times you combine the noise
    // Frequency      - starting noise frequency (kod hopsonovog open buildera je to smoothness
    //                  i koristi se kao 1.0f/smoothness da se dobije isto starting freq)
    //                  higher values add more peaks, it feels like a zoom out
    // Roughness      - determines the roughness of the noise usually left around 0.5f
    // Redistribution - binomna funkcija koja odreduje padove i rastove, default je 1.0f
    class NoiseOptions
    {
        public float Octaves { get; set; }
        public float Frequency { get; set; }
        public float Roughness { get; set; }
        public float Redistribution { get; set; }

        public NoiseOptions()
        {
            Octaves = 4;
            Frequency = 0.25f;
            Roughness = 0.5f;
            Redistribution = 1.0f;
        }
    }
}
