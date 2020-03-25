using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortLayer : MonoBehaviour
{

    [SerializeField]
    private bool continuous = false;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        sortBasedOnY();
    }

    // Update is called once per frame
    void Update()
    {
        if (continuous)
            sortBasedOnY();
    }

    private void sortBasedOnY()
    {
        var bottom = spriteRenderer.bounds.min.y;
        spriteRenderer.sortingOrder = (int)(bottom * -100);
    }
}
