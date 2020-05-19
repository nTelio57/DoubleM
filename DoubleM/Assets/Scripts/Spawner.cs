using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Spawner : MonoBehaviour
{
    public bool isActive = true;
    public bool isEntered;
    public int maxEntitiesCount;
    [Range(0.1f, 15)]
    public float spawningInterval;
    public RectTransform[] spawnZones;
    public GameObject[] fighters;

    //[HideInInspector]
    public int entitiesCount = 0;
    float timer;
    Vector3 newPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameStatus.isMainLevelPaused)
            timer += Time.deltaTime;

        if (timer >= spawningInterval && isEntered && isActive && entitiesCount < maxEntitiesCount)
        {
            int rand = Random.Range(0, spawnZones.Length);
            //RectTransform rt = GetComponent<RectTransform>();
            RectTransform rt = spawnZones[rand];
            int fighterFromArray = Random.Range(0, fighters.Length - 1);
            newPosition.x = Random.Range(rt.position.x - rt.sizeDelta.x / 2, rt.position.x + rt.sizeDelta.x / 2);
            newPosition.y = Random.Range(rt.position.y - rt.sizeDelta.y / 2, rt.position.y + rt.sizeDelta.y / 2);
            newPosition.z = fighters[fighterFromArray].transform.position.z;

            entitiesCount++;
            Instantiate(fighters[fighterFromArray], newPosition, Quaternion.identity, transform);

            timer = 0;
        }
    }
}
