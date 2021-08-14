using UnityEngine;
using Unity.Entities;
using Unity.Rendering;

public struct ChunkBuffer : IBufferElementData {
    public Entity chunk;

}
