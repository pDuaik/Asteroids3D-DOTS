using Unity.Mathematics;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    // Allow DOTS to access the Data Manager.
    public static GameDataManager singleton;

    // Canvas variables
    public int canvasHalfSize = 512;

    // Asteroid variables.
    public int numberOfAsteroids = 1000;
    public int asteroidRadius = 200;
    public float2 asteroidRandomRotationSpeedMinMax = float2.zero;
    public float2 asteroidRandomSpeedMinMax = float2.zero;

    // Missile Variables
    public float missileSpeed = 30;
    public float missileSize = 2;
    public float missileLifeSpan = 5;
    public float currentCooldown { get; set; }

    // Make sure there is only one instance of the class.
    private void Awake()
    {
        if (singleton != null && singleton != this)
            Destroy(gameObject);
        else
            singleton = this;
    }
}