using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;
using System;

public class Chunk : MonoBehaviour {
    
    [SerializeField] private Mesh mesh;
    [SerializeField] private Material material;
    [SerializeField] private int xSize = 16;
    [SerializeField] private int ySize = 225;
    [SerializeField] private int zSize = 16;

    [SerializeField] public int xCoord = 0;
    [SerializeField] public int zCoord = 0;

    FastNoise noise = new FastNoise();
    float[,] heightMap ;
    float[,] heightMap2;

    private void Start(){
        heightMap = new float[xSize,zSize];
        heightMap2 = new float[xSize,zSize];

        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        EntityArchetype entityArchetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(ChunkComponent),
            typeof(Heightmap),
            typeof(RenderMesh),
            typeof(LocalToWorld),
            typeof(RenderBounds)
        );

        Entity entity = entityManager.CreateEntity(entityArchetype);

        DynamicBuffer<Block> dynamicBuffer =  entityManager.AddBuffer<Block>(entity);

        for (int x = 0; x < xSize; x++)
            for (int y = 0; y < ySize; y++)
                for (int z = 0; z < zSize; z++)
                {

                    dynamicBuffer.Add(new Block
                    {
                        material = GetBlockType( x,  y,  z)
                    });

                }

        DynamicBuffer<Block> blockBuffer = dynamicBuffer.Reinterpret<Block>();

        DynamicBuffer<VertexBuffer> dynamicVertexBuffer = entityManager.AddBuffer<VertexBuffer>(entity);

        for (int x = 0; x <= xSize; x++)
            for (int y = 0; y <= ySize; y++)
                for (int z = 0; z <= zSize; z++)
                {
                    dynamicVertexBuffer.Add(new VertexBuffer{ vertex = new Vector3(x, y, z)});
                }

        entityManager.SetComponentData(entity, new ChunkComponent
        {
            xSize = xSize,
            ySize = ySize,
            zSize = zSize,
            biome = Biomes.plains,
            hasToRender = true

        });

        entityManager.SetSharedComponentData(entity, new RenderMesh{
                    mesh = mesh,
                    material = material
                });


    }

    BlockMaterials GetBlockType(int x, int y, int z)
    {
        /*if(y < 33)
            return BlockType.Dirt;
        else
            return BlockType.Air;*/

        


        //print(noise.GetSimplex(x, z));
        float simplex1 = noise.GetSimplex(x*.8f, z*.8f)*10;
        float simplex2 = noise.GetSimplex(x * 3f, z * 3f) * 10*(noise.GetSimplex(x*.3f, z*.3f)+.5f);

        float heightMap = simplex1 + simplex2;

        //add the 2d noise to the middle of the terrain chunk
        float baseLandHeight = ySize * .5f + heightMap;

        //3d noise for caves and overhangs and such
        float caveNoise1 = noise.GetPerlinFractal(x*5f, y*10f, z*5f);
        float caveMask = noise.GetSimplex(x * .3f, z * .3f)+.3f;

        //stone layer heightmap
        float simplexStone1 = noise.GetSimplex(x * 1f, z * 1f) * 10;
        float simplexStone2 = (noise.GetSimplex(x * 5f, z * 5f)+.5f) * 20 * (noise.GetSimplex(x * .3f, z * .3f) + .5f);

        float stoneHeightMap = simplexStone1 + simplexStone2;
        float baseStoneHeight = ySize * .25f + stoneHeightMap;


        //float cliffThing = noise.GetSimplex(x * 1f, z * 1f, y) * 10;
        //float cliffThingMask = noise.GetSimplex(x * .4f, z * .4f) + .3f;



        BlockMaterials blockType = BlockMaterials.air;

        //under the surface, dirt block
        if(y <= baseLandHeight)
        {
            blockType = BlockMaterials.dirt;

            //just on the surface, use a grass type
            if(y > baseLandHeight - 1 && y > 20)
                blockType = BlockMaterials.grass;

            if(y <= baseStoneHeight)
                blockType = BlockMaterials.stone;
        }


        if(caveNoise1 > Mathf.Max(caveMask, .2f))
            blockType = BlockMaterials.air;

        /*if(blockType != BlockType.Air)
            blockType = BlockType.Stone;*/

        //if(blockType == BlockType.Air && noise.GetSimplex(x * 4f, y * 4f, z*4f) < 0)
          //  blockType = BlockType.Dirt;

        //if(Mathf.PerlinNoise(x * .1f, z * .1f) * 10 + y < TerrainChunk.chunkHeight * .5f)
        //    return BlockType.Grass;

        return blockType;
    }

   

}
