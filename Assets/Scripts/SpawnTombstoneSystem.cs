using Pepperwood;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct SpawnTombstoneSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GraveyardProperties>();
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        state.Enabled = false;

        var graveyardEntity = SystemAPI.GetSingletonEntity<GraveyardProperties>();
        var graveyard = SystemAPI.GetAspect<GraveyardAspect>(graveyardEntity);

        var ecb = new EntityCommandBuffer(Allocator.Temp);
        for (int i = 0; i < graveyard.NumberTombstoneToSpawn; i++)
        {
            var tombstone = ecb.Instantiate(graveyard.TombstonePrefab);
            var tombstoneTransform = graveyard.GetRandomTombstoneTransform();
            ecb.SetComponent(tombstone, tombstoneTransform);
        }

        ecb.Playback(state.EntityManager);
    }
}
