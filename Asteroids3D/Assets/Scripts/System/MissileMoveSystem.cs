using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.Jobs;

public class MissileMoveSystem : JobComponentSystem
{
    Entity player;
    protected override void OnStartRunning()
    {
        player = GetSingletonEntity<PlayerData>();
    }
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        // Retrieve from Engine DeltaTime and Inputs.
        float deltaTime = UnityEngine.Time.deltaTime;
        bool shoot = UnityEngine.Input.GetKeyDown("space");
        float3 playerVelocity = EntityManager.GetComponentData<PlayerData>(player).currentVelocity;
        float3 playerPosition = EntityManager.GetComponentData<Translation>(player).Value;
        quaternion playerRotation = EntityManager.GetComponentData<Rotation>(player).Value;
        float missileSpeed = GameDataManager.singleton.missileSpeed;
        bool doubleShot = GameDataManager.singleton.doubleShot;

        // Turn on missiles if player is shooting.
        if (shoot && GameDataManager.singleton.currentCooldown >= GameDataManager.singleton.cooldown)
        {
            GameDataManager.singleton.currentCooldown = 0;
            if (doubleShot)
            {
                int count = 0;
                foreach (var item in GameDataManager.singleton.missiles)
                {
                    if (!EntityManager.GetComponentData<MissileData>(item).isActive)
                    {
                        EntityManager.SetComponentData(item, new MissileData { awake = true, lifeSpan = GameDataManager.singleton.missileLifeSpan, currentLifeSpan = 0, doubleShot = count == 1 ? true : false });
                        count++;

                        if (count >= 2)
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                foreach (var item in GameDataManager.singleton.missiles)
                {
                    if (!EntityManager.GetComponentData<MissileData>(item).isActive)
                    {
                        EntityManager.SetComponentData(item, new MissileData { awake = true, lifeSpan = GameDataManager.singleton.missileLifeSpan, currentLifeSpan = 0 });
                        break;
                    }
                }
            }
        }

        // Check collision.
        foreach (var missile in GameDataManager.singleton.missiles)
        {
            if (EntityManager.GetComponentData<MissileData>(missile).isActive && !EntityManager.GetComponentData<CollisionData>(missile).collision)
            {
                foreach (var asteroid in GameDataManager.singleton.asteroids)
                {
                    float comparisonValue = math.pow(EntityManager.GetComponentData<ShatterData>(asteroid).smallAsteroid ? GameDataManager.singleton.asteroidSize / 4 : GameDataManager.singleton.asteroidSize, 2);
                    if (math.distancesq(EntityManager.GetComponentData<Translation>(asteroid).Value, EntityManager.GetComponentData<Translation>(missile).Value) < comparisonValue)
                    {
                        EntityManager.SetComponentData(asteroid, new CollisionData { collision = true });
                        EntityManager.SetComponentData(missile, new CollisionData { collision = true });
                    }
                }
            }
        }

        JobHandle jobHandle = Entities
            .WithName("MissileMoveSystem")
            .ForEach((ref Translation position, ref Rotation rotation, ref MissileData missileData, ref CollisionData collisionData) =>
            {
                // Awake missile if player is shooting
                if (missileData.awake)
                {
                    missileData.awake = false;
                    missileData.isActive = true;
                    missileData.initialVector = playerVelocity;
                    if (missileData.doubleShot)
                    {
                        position.Value = playerPosition + math.mul(playerRotation, new float3(5, 0, 0));
                    }
                    else
                    {
                        position.Value = playerPosition;
                    }
                }

                // Move all missiles
                if (missileData.isActive)
                {
                    if (missileData.currentLifeSpan >= missileData.lifeSpan || collisionData.collision)
                    {
                        position.Value = new float3(0, 800000, 0);
                        rotation.Value = quaternion.identity;
                        missileData.awake = false;
                        missileData.isActive = false;
                        missileData.initialVector = float3.zero;
                        missileData.currentLifeSpan = 0;
                        collisionData.collision = false;
                    }
                    else
                    {
                        float3 missileForward = math.mul(rotation.Value, new float3(0, 0, 1) * missileSpeed * deltaTime);
                        position.Value += missileData.initialVector + missileForward;
                        rotation.Value = playerRotation;
                        missileData.currentLifeSpan += deltaTime;
                    }
                }
            })
            .Schedule(inputDeps);

        return jobHandle;
    }
}
