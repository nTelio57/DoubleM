using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData 
{
    public int count;
    public float[] x;
    public float[] y;
    public float[] z;
    public int[] spawnerId;
    public int[] prefabId;

    public EnemyData(CapturePointManager manager)
    {
        ParentSpawnerInfo[] enemyInfos = manager.GetComponentsInChildren<ParentSpawnerInfo>();

        count = enemyInfos.Length;
        x = new float[enemyInfos.Length];
        y = new float[enemyInfos.Length];
        z = new float[enemyInfos.Length];
        spawnerId = new int[enemyInfos.Length];
        prefabId = new int[enemyInfos.Length];

        for (int i = 0; i < enemyInfos.Length; i++)
        {
            x[i] = enemyInfos[i].transform.position.x;
            y[i] = enemyInfos[i].transform.position.y;
            z[i] = enemyInfos[i].transform.position.z;
            spawnerId[i] = enemyInfos[i].spawnerId;
            prefabId[i] = enemyInfos[i].prefabId;
            Debug.Log("Save prefab id: " + prefabId[i]);
        }
    }

}
