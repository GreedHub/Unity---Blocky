using UnityEngine;
using Unity.Entities;
using Unity.Rendering;

public struct Block : IBufferElementData {
    public BlockMaterials material;
    public Entity leftBlock;
    public Entity rightBlock;
    public Entity topBlock;
    public Entity bottomBlock;
    public Entity frontBlock;
    public Entity backBlock;
    public bool IsLoaded;
}

public enum BlockMaterials
{
    air,
    water,
    dirt,
    stone,
    sand,
    sandstone,
    grass
}