using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public Text priceText;
    public int ID;
    public int price;

    // Start is called before the first frame update
    void Start()
    {
        priceText.text = price + "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
