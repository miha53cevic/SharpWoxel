using OpenTK.Mathematics;

namespace SharpWoxel.world
{
    class Chunk
    {
        private Vector3i _chunkSize;
        private int[,,] _blocks; // int default value is 0

        public Chunk(Vector3i chunkSize) 
        {
            _chunkSize = chunkSize;
            _blocks = new int[chunkSize.X, chunkSize.Y, chunkSize.Z];
        }

        public int GetBlockLocal(int x, int y, int z)
        {
            if (IsBlockOutOfBounds(x, y, z))
                throw new Exception(string.Format("[Chunk::GetBlockLocal]: Block ({0},{1},{2}) is out of bounds", x, y, z));

            return _blocks[x, y, z];
        }

        public void SetBlockLocal(int x, int y, int z, int block)
        {
            if (IsBlockOutOfBounds(x, y, z))
                throw new Exception(string.Format("[Chunk::SetBlockLocal]: Block ({0},{1},{2}) is out of bounds", x, y, z));

            _blocks[x, y, z] = block;
        }

        private bool IsBlockOutOfBounds(int x, int y, int z)
        {
            if (x < 0 || x >= _chunkSize.X || y < 0 || y >= _chunkSize.Y || z < 0 || z >= _chunkSize.Z)
                return true;
            else return false;
        }
    }
}
