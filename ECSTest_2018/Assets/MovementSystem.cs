using UnityEngine;
using System.Collections;
using System;
using Unity.Entities;
using Unity.Jobs;
using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Burst;

public class MovementSystem : JobComponentSystem
{
    ComponentGroup group;

    //Calculate for multiple entities in one go.
    [BurstCompile]
    struct MovementJob : IJobParallelFor
    {
        //The different 
        public ArchetypeChunkComponentType<Position> PositionType;
        public ArchetypeChunkComponentType<MoveSpeed> SpeedType;

        //NativeArray of Chunks. Each chunk contain many entities in archetype.
        [DeallocateOnJobCompletion] public NativeArray<ArchetypeChunk> chunks;

        public float deltaTime;

        public void Execute(int chunkIndex)
        {
            ArchetypeChunk chunk = chunks[chunkIndex];

            //Get native arrays of component datas in chunk
            NativeArray<Position> positions = chunk.GetNativeArray(PositionType);
            NativeArray<MoveSpeed> speeds = chunk.GetNativeArray(SpeedType);

            //Debug.Log(chunk.Count);
            for (int i = 0; i < chunk.Count; i++)
                positions[i] = new Position { Value = positions[i].Value + new float3(Vector3.right) * speeds[i].Value * deltaTime };
        }
    }

    //Calculate for each entity separately
    //[BurstCompile]
    //struct MovementJob : IJobProcessComponentData<Position, MoveSpeed>
    //{

    //    public float deltaTime;

    //    public void Execute(ref Position position, ref MoveSpeed speed)
    //    {
    //        position.Value += new float3(Vector3.right) * speed.Value * deltaTime;
    //    }
    //}

    protected override JobHandle OnUpdate(JobHandle inputDpes)
    {
        NativeArray<ArchetypeChunk> tempChunks = group.CreateArchetypeChunkArray(Allocator.TempJob);
        //Kind of  All Job
        JobHandle handle = new MovementJob
        {
            chunks = tempChunks,
            PositionType = GetArchetypeChunkComponentType<Position>(),
            SpeedType = GetArchetypeChunkComponentType<MoveSpeed>(),
            deltaTime = Time.deltaTime
        }.Schedule(tempChunks.Length, 32, inputDpes);
        
        return handle;

        //Each job
        //return new MovementJob
        //{
        //    deltaTime = Time.deltaTime
        //}.Schedule(this, inputDpes);

    }

    protected override void OnCreateManager()
    {
        base.OnCreateManager();

        //group defines what archetypes to search for
        group = GetComponentGroup(new EntityArchetypeQuery()
        {
            Any = Array.Empty<ComponentType>(),
            None = Array.Empty<ComponentType>(),
            All = new[] { ComponentType.Create<Position>(), ComponentType.Create<MoveSpeed>(), }
        });
    }
}
