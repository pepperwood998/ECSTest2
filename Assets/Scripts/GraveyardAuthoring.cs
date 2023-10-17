using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class GraveyardAuthoring : MonoBehaviour
{
    public float2 FieldDimensions;
    public int NumberTombstonesToSpawn;
    public GameObject TombstonePrefab;
    public uint RandomSeed;
}

public class GraveyardBaker : Baker<GraveyardAuthoring>
{
    public override void Bake(GraveyardAuthoring authoring)
    {
        var graveyardEntity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(graveyardEntity, new GraveyardProperties
        {
            FieldDimensions = authoring.FieldDimensions,
            NumberTombstonesToSpawn = authoring.NumberTombstonesToSpawn,
            TombstonePrefab = GetEntity(authoring.TombstonePrefab, TransformUsageFlags.Dynamic),
        });
        AddComponent(graveyardEntity, new GraveyardRandom
        {
            Value = Unity.Mathematics.Random.CreateFromIndex(authoring.RandomSeed),
        });
        AddComponent<ZombieSpawnPoints>(graveyardEntity);
    }
}
