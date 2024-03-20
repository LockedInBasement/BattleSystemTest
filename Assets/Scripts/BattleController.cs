using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public static BattleController instance;

    public int startingMana = 4, maxMana = 12;
    public int playerMana;

    public int startingCardsAmount= 5;

    public enum TurnOrder { playerActive, playerCardAttacks, enemyActivve, enemyCardsAttacks }
    public TurnOrder currentPhase;
    public int currentPlayerMaxMana;

    public int cardsToDrawPerTurn = 2;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentPlayerMaxMana = startingMana;

        FillPlayerMana();

        DeckController.instance.DrawMultipleCard(startingCardsAmount);
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

        UIController.instance.SetPlayerManaText(playerMana);
    }

    public void FillPlayerMana()
    {
        playerMana = currentPlayerMaxMana;
        UIController.instance.SetPlayerManaText(playerMana);
    }

    public void AdvanceTurn()
    {
        currentPhase++;

        if((int)currentPhase >= System.Enum.GetValues(typeof(TurnOrder)).Length) { currentPhase = 0; }

        switch (currentPhase)
        {
            case TurnOrder.playerActive:
                UIController.instance.endTurnButton.SetActive(true);
                UIController.instance.drawCardButton.SetActive(true);

                if(currentPlayerMaxMana<maxMana)
                {
                    currentPlayerMaxMana++;
                }

                FillPlayerMana();

                DeckController.instance.DrawMultipleCard(cardsToDrawPerTurn);

                break;
            case TurnOrder.playerCardAttacks:

                CardsPointsController.instance.PlayerAttack();
                
                break;
            case TurnOrder.enemyActivve:
                AdvanceTurn();
                break;
            case TurnOrder.enemyCardsAttacks:
                AdvanceTurn();
                break;
        }
    }

    public void EndPlyerTurn()
    {
        UIController.instance.endTurnButton.SetActive(false);
        UIController.instance.drawCardButton.SetActive(false);

        AdvanceTurn();
    }
}
