using UnityEngine;
using System;
using Unity.Entities;


//Entity component defining position. Doesn't affect anythinh right now.
[GenerateAuthoringComponent]
public struct Position : IComponentData
{
    public Vector3 Value;
}
