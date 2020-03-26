using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using UnityEngine.Jobs;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Transforms;

public class Manager : MonoBehaviour
{
    public GameObject thing;
    Entity ething;

    //MovementJob job;
    //TransformAccessArray transforms;
    JobHandle handle;

    EntityManager eManager;

    private void OnDisable()
    {
        handle.Complete();
        //transforms.Dispose();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Create entity prefab from the game object hierarchy once
        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
        ething = GameObjectConversionUtility.ConvertGameObjectHierarchy(thing, settings);
        eManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            
        //transforms = new TransformAccessArray(0, -1);
        AddCubes(10);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            AddCubes(10);
    }

    void AddCubes(int amount)
    {

        NativeArray<Entity> entities = new NativeArray<Entity>(amount, Allocator.Temp);
        eManager.Instantiate(ething, entities);

        for (int i = 0; i < amount; i++)
        {
            // Place and set speed of entity
            eManager.SetComponentData(entities[i], new Translation { Value = new float3(0, 2f * i, 0) });
            eManager.SetComponentData(entities[i], new MoveSpeed { Value = 1f * i });
        }

        //Dispose of NativeArray
        entities.Dispose();


        for (int i = 0; i < amount; i++)
        {
            //Efficiently instantiate a bunch of entities from the already converted entity prefab
            Entity instance = eManager.Instantiate(ething);

            // Place and set speed of entity
            eManager.SetComponentData(instance, new Translation { Value = new float3(0, 2f * i, 0) });
            eManager.SetComponentData(instance, new MoveSpeed { Value = 1f * i });
        }

    }
}
