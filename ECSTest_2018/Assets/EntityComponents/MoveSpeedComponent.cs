using UnityEngine;
using System;
using Unity.Entities;

//Entitycomponent containing speed of cube

[Serializable]
public struct MoveSpeed : IComponentData
{
    public float Value;
}

public class MoveSpeedComponent : ComponentDataProxy<MoveSpeed> { }