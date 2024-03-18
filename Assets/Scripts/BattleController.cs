using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public static BattleController instance;

    public int startingMana = 4, maxMana = 12;
    public int playerMana;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerMana = startingMana;
        UIController.Instance.SetPlayerManaText(playerMana);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpendPlayerMana(int amountToSpend)
    {
        playerMana -= amountToSpend;

        if(playerMana < 0)
        {
            playerMana = 0;
        }

        UIController.Instance.SetPlayerManaText(playerMana);
    }
}