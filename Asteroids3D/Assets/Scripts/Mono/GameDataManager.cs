using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;

public class GameDataManager : MonoBehaviour
{
    // Allow DOTS to access the Data Manager.
    public static GameDataManager singleton;

    //MainCamera
    public Transform mainCamera;

    // Main Camera is feeding this information.
    public float3 playerPosition { get; set; }
    public quaternion playerRotation { get; set; }
    public float3 playerVelocity { get; set; }

    // Prefabs to convert into Entities.
    public GameObject missilePrefab;
    public GameObject asteroidPrefab;

    // Canvas variables
    public float canvasSize = 512;

    // Asteroid variables.
    public Entity[] asteroids;
    public int numberOfAsteroids = 1000;
    public float2 asteroidRandomRotationSpeedMinMax = float2.zero;
    public float2 asteroidRandomSpeedMinMax = float2.zero;
    public float asteroidSize = 200;

    // Missile Variables
    public Entity[] missiles;
    public int numberOfMissiles = 100;
    public float missileSpeed = 30;
    public float missileSize = 2;
    public float missileLifeSpan = 5;
    public float cooldown = 1;
    public float currentCooldown { get; set; }

    // Power-ups
    public bool shield = false;
    public bool doubleShot = false;

    // Shield logic
    public float shieldTimer = 5;
    private float currentShieldTimer = 0;

    // Shot Logic
    public float shotTimer = 10;
    public float currentShotTimer = 0;

    private void Awake()
    {
        // Make sure there is only one instance of the class.
        if (singleton != null && singleton != this)
            Destroy(gameObject);
        else
            singleton = this;
    }

    private void Update()
    {
        // This update must move to the player data.
        // This is not a good pattern, but a choice due to time constrains.
        // Power-Ups rely on this update.
        currentCooldown += Time.deltaTime;
        currentShieldTimer += Time.deltaTime;
        if (currentShieldTimer >= shieldTimer)
        {
            currentShieldTimer -= shieldTimer;
            shield = !shield;
        }

        if (currentShotTimer > 0)
        {
            currentShotTimer -= Time.deltaTime;
        }
        else
        {
            doubleShot = false;
        }
    }
}
