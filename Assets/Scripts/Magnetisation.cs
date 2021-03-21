using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public struct DataStruct : IComponentData
{
    //public float[,,,] Mag_4_map;
    public float3 position;
    public float3 direction;

}
