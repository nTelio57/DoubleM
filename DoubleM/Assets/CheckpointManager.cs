using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public Transform player;
    public Transform spawn;

    public static Transform currentCheckpoint;

    // Start is called before the first frame update
    void Start()
    {
        currentCheckpoint = spawn;
        Respawn();
    }
    

    public void Respawn()
    {
        Vector3 newPos;
        newPos.x = currentCheckpoint.position.x;
        newPos.y = currentCheckpoint.position.y;
        newPos.z = player.position.z;
        player.position = newPos;
    }
}
