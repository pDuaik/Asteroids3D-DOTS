using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameDataManager))]
public class GameDataManager_Editor : Editor
{

    private GameDataManager mScript;

    private void Awake()
    {
        mScript = (GameDataManager)target;
    }

    public override void OnInspectorGUI()
    {
        GUILayout.Label("CAMERA");
        GUILayout.Space(2);

        // Transform
        mScript.mainCamera = (Transform)EditorGUILayout.ObjectField("- Transform", mScript.mainCamera, typeof(Transform), false);

        // Canvas Size
        mScript.canvasSize = EditorGUILayout.FloatField("- Canvas Size", mScript.canvasSize);
        GUILayout.Space(10);

        GUILayout.Label("ASTEROID");
        GUILayout.Space(2);

        // Prefab
        mScript.asteroidPrefab = (GameObject)EditorGUILayout.ObjectField("- Prefab", mScript.asteroidPrefab, typeof(GameObject), true);
        // Quantity
        mScript.numberOfAsteroids = EditorGUILayout.IntField("- Quantity", mScript.numberOfAsteroids);
        // Size
        mScript.asteroidSize = EditorGUILayout.FloatField("- Size", mScript.asteroidSize);

        // Random Rotation
        GUILayout.BeginHorizontal();
        GUILayout.Label("- Random Rotation");
        GUILayout.FlexibleSpace();
        GUILayout.Label("Min:", GUILayout.MaxWidth(30));
        var rotMin = EditorGUILayout.FloatField(mScript.asteroidRandomRotationSpeedMinMax.x, GUILayout.MaxWidth(50));
        GUILayout.Label("Max:", GUILayout.MaxWidth(30));
        var rotMax = EditorGUILayout.FloatField(mScript.asteroidRandomRotationSpeedMinMax.y, GUILayout.MaxWidth(50));
        mScript.asteroidRandomRotationSpeedMinMax = new Vector2(rotMin, rotMax);
        GUILayout.EndHorizontal();

        // Random Speed
        GUILayout.BeginHorizontal();
        GUILayout.Label("- Random Speed");
        GUILayout.FlexibleSpace();
        GUILayout.Label("Min:", GUILayout.MaxWidth(30));
        var speedMin = EditorGUILayout.FloatField(mScript.asteroidRandomSpeedMinMax.x, GUILayout.MaxWidth(50));
        GUILayout.Label("Max:", GUILayout.MaxWidth(30));
        var speedMax = EditorGUILayout.FloatField(mScript.asteroidRandomSpeedMinMax.y, GUILayout.MaxWidth(50));
        mScript.asteroidRandomSpeedMinMax = new Vector2(speedMin, speedMax);
        GUILayout.EndHorizontal();

        GUILayout.Space(10);

        GUILayout.Label("MISSILE");
        GUILayout.Space(2);

        // Prefab
        mScript.missilePrefab = (GameObject)EditorGUILayout.ObjectField("- Prefab", mScript.missilePrefab, typeof(GameObject), true);
        // Quantity
        mScript.numberOfMissiles = EditorGUILayout.IntField("- Quantity", mScript.numberOfMissiles);
        // Size
        mScript.missileSize = EditorGUILayout.FloatField("- Size", mScript.missileSize);
        // Life Span
        mScript.missileLifeSpan = EditorGUILayout.FloatField("- Life Span", mScript.missileLifeSpan);
        GUILayout.Space(10);

        GUILayout.Label("SHIELD");
        GUILayout.Space(2);
        GUILayout.BeginHorizontal();
        mScript.shieldTimer = EditorGUILayout.FloatField("- Shield Timer", mScript.shieldTimer);
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.Label("DOUBLE SHOT");
        GUILayout.Space(2);
        GUILayout.BeginHorizontal();
        mScript.shotTimer = EditorGUILayout.FloatField("- Double Shot Timer", mScript.shotTimer);
        GUILayout.EndHorizontal();
    }
}