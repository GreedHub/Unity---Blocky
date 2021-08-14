using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;

public class TerrainEntity : MonoBehaviour {

    FastNoise noise = new FastNoise();
    [SerializeField] private int xSize = 16;
    [SerializeField] private int ySize = 225;
    [SerializeField] private int zSize = 16;

    private void Start(){

        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        EntityArchetype entityArchetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(Heightmap)
        );

        Entity entity = entityManager.CreateEntity(entityArchetype);

        DynamicBuffer<ChunkBuffer> dynamicBuffer =  entityManager.AddBuffer<ChunkBuffer>(entity);        

/*          for (int x = 0; x < xSize; x=+16)
                for (int z = 0; z < zSize; z=+16)
                {

                    dynamicBuffer.Add(new ChunkBuffer
                    {
                        chunk = new Chunk{
                            xCoord = 16,
                            zCoord = 16
                        }
                    });

                } */
    }

}