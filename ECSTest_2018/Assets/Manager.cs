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
        // Get the manager of the active world
        eManager = World.Active.GetOrCreateManager<EntityManager>();
            
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
        //Creates nativearry. Allocator.Temp means the array is stored in memory during only this frame.
        NativeArray<Entity> entities = new NativeArray<Entity>(amount, Allocator.Temp);

        //Instantiate new entities in the array.
        eManager.Instantiate(thing, entities);

        for (int i = 0; i < amount; i++)
        {
            // Set speed and positions of new entities.
            eManager.SetComponentData(entities[i], new Position { Value = new float3(0, 2f * i, 0) });
            eManager.SetComponentData(entities[i], new MoveSpeed { Value = 1f * i });
        }

        //Dispose of NativeArray
        entities.Dispose();

        //This approach instantiates one entity at a time.
        //for (int i = 0; i < amount; i++)
        //{
        //    //Efficiently instantiate a bunch of entities from the already converted entity prefab
        //    Entity instance = eManager.Instantiate(thing);

        //    // Place and set speed of entity
        //    eManager.SetComponentData(instance, new Position { Value = new float3(0, 2f * i, 0) });
        //    eManager.SetComponentData(instance, new MoveSpeed { Value = 1f * i });
        //}

    }
}
