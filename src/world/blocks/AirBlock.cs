﻿using OpenTK.Mathematics;
using SharpWoxel.util;

namespace SharpWoxel.world.blocks
{
    class AirBlock : IBlock
    {
        public Vector2i GetFaceTextureAtlasCoordinates(Cube.Face face)
        {
            throw new NotImplementedException();
        }

        public string GetID()
        {
            return "air_block";
        }

        public bool IsAir()
        {
            return true;
        }

        public bool IsTransparent()
        {
            throw new NotImplementedException();
        }
    }
}
