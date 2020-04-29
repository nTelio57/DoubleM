using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanelManager : MonoBehaviour
{
    public Text name1, text1;
    public Text name2, text2;
    public Text name3, text3;
    public Text name4, text4;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setTextfields(Item i)
    {
        name1.text = i.name1;
        text1.text = i.text1;

        name2.text = i.name2;
        text2.text = i.text2;

        name3.text = i.name3;
        text3.text = i.text3;

        name4.text = i.name4;
        text4.text = i.text4;
    }
}
