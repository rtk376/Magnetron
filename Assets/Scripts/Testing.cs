using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Rendering;
using Unity.Mathematics;
using System;

[RequireComponent(typeof(JSONLoader))]
public class Testing : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Mesh mesh;
    [SerializeField] private Material material;
    [SerializeField] private float Scale = 1;
    public float[,,,] tmag;

    private JSONLoader jsonLoader;

    private void Awake()
    {
        jsonLoader = GetComponent<JSONLoader>();

    }

    private void Start()
    {
        //Debug.Log(JSONLoader.instance.gameObject.name);
        //JSONLoader.instance.GetMagnet(out tmag);

        tmag = jsonLoader.Magnet;
        Debug.Log(tmag[0,0,0,0]);
        int N_x = tmag.GetLength(0);
        int N_y = tmag.GetLength(1);
        int N_z = tmag.GetLength(2);



        
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        EntityArchetype entityArchetype = entityManager.CreateArchetype(
            typeof(DataStruct),
            typeof(Translation),
            typeof(RenderMesh),
            typeof(LocalToWorld),
            typeof(Rotation),
            typeof(Scale),
            typeof(RenderBounds),
            typeof(LineRenderer)
        );
        NativeArray<Entity> entityArray = new NativeArray<Entity>(N_x * N_y * N_z, Allocator.Temp);
        entityManager.CreateEntity(entityArchetype, entityArray);
        //entityManager.AddComponentData(entity, new Magnetisation()); this is for one entity
        int _i = 0;
        int _j = 0;
        int _k = 0;
        //for (int i = 0; i <entityArray.Length; i++)
        //{
        //    Entity entity = entityArray[i];
        //    float3 position = new float3(UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100));

        //    AABB box = new AABB();
        //    box.Center = position;
        //    box.Extents = new float3 (3f, 3f, 3f);


        //    entityManager.SetComponentData(entity, new Magnetisation { Mag_4_map = tmag });
        //    entityManager.SetComponentData(entity, new Scale { Value = 1f });
        //    entityManager.SetComponentData(entity, new Translation { Value =  position});
        //    entityManager.SetComponentData(entity, new RenderBounds { Value = box });
        //    entityManager.SetSharedComponentData(entity, new RenderMesh
        //    {
        //        mesh = mesh,
        //        material = material,
        //    });
        //}
        int n = 0;

        for (int i = 0; i < N_x; i+=1)
        {
            for (int j = 0; j < N_y; j+=1)
            {
                for (int k = 0; k < N_z; k+=1)
                {
                    Entity entity = entityArray[n];
                    n++;
                    
                    AABB box = new AABB();
                    box.Center = new float3 (i*3, j*3, k*3);
                    box.Extents = new float3(1f, 1f, 1f);

                    
                    float3 direction = new float3(tmag[i, j, k, 0], tmag[i, j, k, 1], tmag[i, j, k, 2]);
                    float3 pos1 = box.Center - direction / 2;
                    float3 pos2 = box.Center + direction / 2;

                    float magnitude = Mathf.Sqrt(Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.y, 2) + Mathf.Pow(direction.y, 2)) * Scale;
                    Debug.Log(tmag.GetLength(3));

                    Quaternion rotation = Quaternion.LookRotation(direction);
                    //LineRenderer line = new LineRenderer();
                    //line.positionCount = 2;
                    //line.SetPosition(0, pos1);
                    //line.SetPosition(1, pos2);

                    entityManager.SetComponentData(entity, new DataStruct { position = box.Center, direction = direction});
                    entityManager.SetComponentData(entity, new Scale { Value = magnitude });
                    entityManager.SetComponentData(entity, new Translation { Value = box.Center});
                    entityManager.SetComponentData(entity, new Rotation { Value = rotation });
                    //entityManager.SetComponentData(entity, new RenderBounds { Value = box });
                    //entityManager.SetComponentData(entity, line);
                    entityManager.SetSharedComponentData(entity, new RenderMesh
                    {
                        mesh = mesh,
                        material = material,
                    });
                    
                }
            }
        }

        entityArray.Dispose();
    }
}
