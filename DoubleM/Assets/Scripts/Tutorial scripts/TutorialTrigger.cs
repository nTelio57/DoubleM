using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialTrigger : MonoBehaviour
{
    public UnityEvent toDo;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        toDo.Invoke();
    }
}
