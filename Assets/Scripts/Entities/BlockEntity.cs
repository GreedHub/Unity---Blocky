using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;

public class BlockEntity : MonoBehaviour {

    private void Start(){

        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        EntityArchetype entityArchetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(RenderMesh),
            typeof(Block),
            typeof(LocalToWorld),
            typeof(RenderBounds)
        );

        Entity entity = entityManager.CreateEntity(entityArchetype);

        DynamicBuffer<Block> dynamicBuffer =  entityManager.AddBuffer<Block>(entity);   
        DynamicBuffer<VertexBuffer> dynamicVertexBuffer =  entityManager.AddBuffer<VertexBuffer>(entity);     


    }

}