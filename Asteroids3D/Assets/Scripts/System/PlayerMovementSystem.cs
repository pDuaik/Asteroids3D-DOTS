using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.Jobs;

public class PlayerMovementSystem : JobComponentSystem
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
        float horizontal = UnityEngine.Input.GetAxis("Horizontal");
        float vertical = UnityEngine.Input.GetAxis("Vertical");
        bool thrust = UnityEngine.Input.GetKey("left shift");

        // GameDataManager variables
        float canvasSize = GameDataManager.singleton.canvasSize;
        bool shield = GameDataManager.singleton.shield;

        // Check collision and move asteroid from collision place.
        bool collision = false;
        foreach (var item in GameDataManager.singleton.asteroids)
        {
            float comparisonValue = math.pow(EntityManager.GetComponentData<ShatterData>(item).smallAsteroid ? GameDataManager.singleton.asteroidSize / 4 : GameDataManager.singleton.asteroidSize, 2);
            if (math.distancesq(EntityManager.GetComponentData<Translation>(item).Value, EntityManager.GetComponentData<Translation>(player).Value) < comparisonValue)
            {
                EntityManager.SetComponentData(item, new CollisionData { collision = true });
                collision = true;

                break;
            }
        }

        JobHandle jobHandle = Entities
            .WithName("PlayerMovementSystem")
            .ForEach((ref Translation position, ref Rotation rotation, ref PlayerData playerData) =>
            {
                if (collision && !shield)
                {
                    rotation.Value = quaternion.identity;
                    position.Value = float3.zero;
                    playerData.currentThrust = float3.zero;
                }
                else
                {
                    // Set rotation
                    rotation.Value = math.mul(rotation.Value, quaternion.AxisAngle(new float3(0, 0, -1), playerData.rotationSpeed * horizontal * deltaTime));
                    rotation.Value = math.mul(rotation.Value, quaternion.AxisAngle(new float3(1, 0, 0), playerData.rotationSpeed * vertical * deltaTime));

                    // Set Position and Velocity
                    playerData.currentThrust += thrust ? math.mul(rotation.Value, new float3(0, 0, 1) * playerData.acceleration * deltaTime) : float3.zero;
                    playerData.currentVelocity = position.Value;
                    position.Value += playerData.currentThrust;
                    playerData.currentVelocity = position.Value - playerData.currentVelocity;
                }

            })
            .Schedule(inputDeps);

        return jobHandle;
    }
}