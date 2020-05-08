using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct Block : IComponentData {
    public BlockMaterials material;

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