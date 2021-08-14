using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using System.Collections;
using System.Collections.Generic;

public struct ChunkComponent : IComponentData {
    
    public int xSize;
    public int ySize;
    public int zSize;
    public Biomes biome;
    public bool hasToRender;

}

public enum Biomes{
    plains
}