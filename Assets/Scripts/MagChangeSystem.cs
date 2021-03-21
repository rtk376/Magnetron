using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class MagChangeSystem : ComponentSystem
{
    
    protected override void OnUpdate()
    {
        Entities.ForEach((ref DataStruct mag) =>
        {
            //mag.Mag_4_map gives the magnetisation direction array with dims N_x, N_y, N_z
            //mag.position gives the entity position

        });
    }

    protected override void OnCreate()
    {
        base.OnCreate();
        //position =  Vector3(mag )
        Entities.ForEach((ref DataStruct data) =>
        {
            Vector3 position;
            //Debug.Log(data.direction);
            //Vector3 direction = data.Mag_4_map[data.position[0], data.position[1], data.position[2]];

});

    }
}
