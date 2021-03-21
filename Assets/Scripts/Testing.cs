using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Rendering;
using Unity.Mathematics;

[RequireComponent(typeof(JSONLoader))]
public class Testing : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Mesh mesh;
    [SerializeField] private Material material;
    [SerializeField] private float Scale = 1;

    //minimum magnetisation for the arrow to be rendered.
    [SerializeField] private float displayMagnetisation = 0f;

    public float[,,,] tmag;

    private JSONLoader jsonLoader;

    private void Awake()
    {
        jsonLoader = GetComponent<JSONLoader>();
    }

    private void Start()
    {
        tmag = jsonLoader.Magnet;
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
            //typeof(Scale) -- replaced by NonUniformScale to scale on each local axis independently
            typeof(NonUniformScale),
            typeof(RenderBounds),
            typeof(LineRenderer)
        );

        NativeArray<Entity> entityArray = new NativeArray<Entity>(N_x * N_y * N_z, Allocator.Temp);
        entityManager.CreateEntity(entityArchetype, entityArray);


        int n = 0; //counter for the absolute number of entities looped through

        //looping through all magnetisation array positions
        for (int i = 0; i < N_x; i++)
        {
            for (int j = 0; j < N_y; j++)
            {
                for (int k = 0; k < N_z; k++)
                {
                    Entity entity = entityArray[n];
                    n++;
                    
                    AABB box = new AABB();
                    box.Center = new float3 (i*3, j*3, k*3);
                    box.Extents = new float3(1f, 1f, 1f);

                    
                    float3 direction = new float3(tmag[i, j, k, 0], tmag[i, j, k, 1], tmag[i, j, k, 2]);
                    float magnitude = Mathf.Sqrt(Mathf.Pow(direction.x, 2) + Mathf.Pow(direction.y, 2) + Mathf.Pow(direction.y, 2)) * Scale;

                    //Debug.Log(tmag.GetLength(3));
                    //Debug.Log(magnitude);

                    Quaternion rotation = Quaternion.LookRotation(direction);
                    
                 
                    entityManager.SetComponentData(entity, new DataStruct { position = box.Center, direction = direction});

                    //entityManager.SetComponentData(entity, new Scale { Value = magnitude });
                    float3 nonUniformScale = new float3(1, magnitude, 1);
                    entityManager.SetComponentData(entity, new NonUniformScale { Value = nonUniformScale});


                    entityManager.SetComponentData(entity, new Translation { Value = box.Center});
                    entityManager.SetComponentData(entity, new Rotation { Value = rotation });

                    //only display the mesh if the magnitude is above the displayMagnetisation threshold
                    if (magnitude >= displayMagnetisation)
                    {
                        entityManager.SetSharedComponentData(entity, new RenderMesh { mesh = mesh, material = material });
                    }
                    
                    
                }
            }
        }

        entityArray.Dispose();
    }
}
