using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Pepperwood
{
    public readonly partial struct GraveyardAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRO<LocalTransform> transformRef;
        private LocalTransform transform => transformRef.ValueRO;
        private readonly RefRO<GraveyardProperties> graveyardProperties;
        private readonly RefRW<GraveyardRandom> graveyardRandom;

        public int NumberTombstoneToSpawn => graveyardProperties.ValueRO.NumberTombstonesToSpawn;
        public Entity TombstonePrefab => graveyardProperties.ValueRO.TombstonePrefab;

        public LocalTransform GetRandomTombstoneTransform()
        {
            return new LocalTransform
            {
                Position = GetRandomPosition(),
                Rotation = quaternion.identity,
                Scale = 1f,
            };
        }

        private float3 GetRandomPosition()
        {
            float3 randomPosition;

            do
            {
                randomPosition = graveyardRandom.ValueRW.Value.NextFloat3(MinCorner, MaxCorner);
            } while (math.distancesq(transform.Position, randomPosition) <= BRAIN_SAFETY_RADIUS_SQ);
            
            return randomPosition;
        }

        private float3 MinCorner => transform.Position - HalfDimensions;
        private float3 MaxCorner => transform.Position + HalfDimensions;
        private float3 HalfDimensions => new()
        {
            x = graveyardProperties.ValueRO.FieldDimensions.x * 0.5f,
            y = 0f,
            z = graveyardProperties.ValueRO.FieldDimensions.y * 0.5f
        };
        private const float BRAIN_SAFETY_RADIUS_SQ = 100;
    }
}
