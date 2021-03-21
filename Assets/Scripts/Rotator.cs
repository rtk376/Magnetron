using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class Rotator : MonoBehaviour
{
    public float speed;
}

class RotatorSystem : ComponentSystem
{
    struct Components
    {
        public Rotator rotator;
        public Transform transform;
    }

    //will run of the component system every frame
    protected override void OnUpdate()
    {
        

    }
}
