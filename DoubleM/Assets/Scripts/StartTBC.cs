using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartTBC : MonoBehaviour
{
    public bool isSending = true;
    bool loaded = false;
    public string TBCScene;

    public Fighter[] fighters;
    static public Fighter[] fighterPass;

    public int victoryLoot;
    static int victoryLootPass;

    // Start is called before the first frame update
    void Start()
    {

    }
    public Fighter[] getFighters()
    {
        return fighterPass;
    }

    public int getVictoryLoot()
    {
        return victoryLootPass;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!loaded)
        {
            if (isSending)
            {
                fighterPass = fighters;
                victoryLootPass = victoryLoot;
            }
                
            loaded = true;
            SceneManager.LoadScene(TBCScene, LoadSceneMode.Additive);
        }
    }

}
