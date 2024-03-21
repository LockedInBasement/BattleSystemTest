using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public static BattleController instance;

    public int startingMana = 4, maxMana = 12;
    public int playerMana , enemyMana;

    public int startingCardsAmount= 5;

    public enum TurnOrder { playerActive, playerCardAttacks, enemyActivve, enemyCardsAttacks }
    public TurnOrder currentPhase;
    public int currentPlayerMaxMana, currentEnemyMaxMana;

    public int cardsToDrawPerTurn = 2;

    public Transform discardPoint;

    public int playerHealth;
    public int enemyHealth;

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

        UIController.instance.SetPlayerHealthText(playerHealth);
        UIController.instance.SetEnemyHealthText(enemyHealth);
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
                EnemyController.instance.StartAction();
                break;

            case TurnOrder.enemyCardsAttacks:
                CardsPointsController.instance.EnemyAtttack();
                break;

            default:
                Debug.LogError("This should not happen");
                break;
        }
    }

    public void EndPlyerTurn()
    {
        UIController.instance.endTurnButton.SetActive(false);
        UIController.instance.drawCardButton.SetActive(false);

        AdvanceTurn();
    }

    public void DamagePlayer(int damageAmount)
    {
        if (playerHealth > 0)
        {
            playerHealth -= damageAmount;

            if (playerHealth <= 0)
            {
                playerHealth = 0;
            }

            UIController.instance.SetPlayerHealthText(playerHealth);

            UIDamageIndicator damageClone =  Instantiate(UIController.instance.playerDamage, UIController.instance.playerDamage.transform.parent);
            damageClone.damageText.text = damageAmount.ToString();
            damageClone.gameObject.SetActive(true);
        }
    }

    public void DamageEnemy(int damageAmount)
    {
        if (enemyHealth > 0)
        {
            enemyHealth -= damageAmount;

            if (enemyHealth <= 0)
            {
                enemyHealth = 0;
            }

            UIController.instance.SetEnemyHealthText(enemyHealth);

            UIDamageIndicator damageClone = Instantiate(UIController.instance.enemyDamage, UIController.instance.enemyDamage.transform.parent);
            damageClone.damageText.text = damageAmount.ToString();
            damageClone.gameObject.SetActive(true);
        }
    }
}
