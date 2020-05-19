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

    public void setTextfields(Fighter f)
    {
        name1.text = f.AbilityOne.name;
        text1.text = f.AbilityOne.describtion;

        name2.text = f.AbilityTwo.name;
        text2.text = f.AbilityTwo.describtion;

        name3.text = f.AbilityThree.name;
        text3.text = f.AbilityThree.describtion;

        name4.text = f.AbilityFour.name;
        text4.text = f.AbilityFour.describtion;
    }
}
