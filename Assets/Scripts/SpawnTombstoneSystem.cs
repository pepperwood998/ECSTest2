using Pepperwood;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

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
        var blobBuilder = new BlobBuilder(Allocator.Temp);
        ref var spawnPoints = ref blobBuilder.ConstructRoot<ZombieSpawnPointsBlob>();
        var arrayBuilder = blobBuilder.Allocate(ref spawnPoints.Value, graveyard.NumberTombstoneToSpawn);
        var tombstoneOffset = new float3(0f, -2f, 1f);

        for (int i = 0; i < graveyard.NumberTombstoneToSpawn; i++)
        {
            var tombstone = ecb.Instantiate(graveyard.TombstonePrefab);
            var tombstoneTransform = graveyard.GetRandomTombstoneTransform();
            ecb.SetComponent(tombstone, tombstoneTransform);

            var zombieSpawnPoint = tombstoneTransform.Position + tombstoneOffset;
        }

        var blobAsset = blobBuilder.CreateBlobAssetReference<ZombieSpawnPointsBlob>(Allocator.Persistent);
        ecb.SetComponent(graveyardEntity, new ZombieSpawnPoints
        {
            Value = blobAsset,
        });
        blobBuilder.Dispose();

        ecb.Playback(state.EntityManager);
    }
}
