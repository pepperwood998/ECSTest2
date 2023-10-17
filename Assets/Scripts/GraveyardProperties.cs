using Unity.Entities;
using Unity.Mathematics;

public struct GraveyardProperties : IComponentData
{
    public float2 FieldDimensions;
    public int NumberTombstonesToSpawn;
    public Entity TombstonePrefab;
}
