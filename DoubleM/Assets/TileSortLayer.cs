using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSortLayer : MonoBehaviour
{
    [SerializeField]
    private bool continuous = false;
    private TilemapRenderer tilemapRenderer;
    Tilemap tilemap;
    BoundsInt bounds;
    TileBase[] allTiles;

    // Start is called before the first frame update
    void Start()
    {
        tilemapRenderer = GetComponent<TilemapRenderer>();
        tilemap = GetComponent<Tilemap>();
        bounds = tilemap.cellBounds;
        allTiles = tilemap.GetTilesBlock(bounds);

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
        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if(tile != null)
                    Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
            }
        }
       // var bottom = tilemapRenderer.bounds.min.y;
      //  tilemapRenderer.sortingOrder = (int)(bottom * -100);
    }
}
