
namespace SharpWoxel.world
{
    enum BlockType
    {
        AIR,
        GRASS,
        DIRT,
        STONE,
    }

    class Block
    {
        public BlockType Type { get; set; }
        
        public Block(BlockType type = BlockType.AIR)
        {
            Type = type;
        }

        public bool IsAir()
        {
            return (Type == BlockType.AIR);
        }
    }
}
