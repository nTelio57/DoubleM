using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecks : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Capture point")
        {
            collision.gameObject.GetComponent<CapturePoint>().StartTheFight(collision);
        }

        if (collision.tag == "Spawn zone")
        {
            collision.gameObject.GetComponent<Spawner>().isEntered = true;
        }

        if (collision.tag == "Enemy")
        {
          //  Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Capture point")
        {
            collision.gameObject.GetComponent<CapturePoint>().ExitedCollision(collision);
        }

        if (collision.tag == "Spawn zone")
        {
            collision.gameObject.GetComponent<Spawner>().isEntered = false;
        }
    }
}
