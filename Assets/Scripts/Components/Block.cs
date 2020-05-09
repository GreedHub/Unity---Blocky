using UnityEngine;
using Unity.Entities;
using Unity.Rendering;

public struct Block : IBufferElementData {
    public BlockMaterials material;
    public int xCoord;
    public int yCoord;
    public int zCoord;
}

public enum BlockMaterials
{
    air,
    water,
    dirt,
    stone,
    sand,
    sandstone
}