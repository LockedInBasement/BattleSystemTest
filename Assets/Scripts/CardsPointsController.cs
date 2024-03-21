using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsPointsController : MonoBehaviour
{
    public static CardsPointsController instance;

    private void Awake()
    {
        instance = this;
    }

    public CardPlacePoint[] playerCardPoints, enemyCardPoints;
    public float timeBetweenAttacks = 0.1f;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayerAttack()
    {
        StartCoroutine(PlayerAttackCo());
    }

    IEnumerator PlayerAttackCo()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);

        for (int i = 0; i < playerCardPoints.Length; i++)
        {
            if (playerCardPoints[i].activeCard != null)
            {
                if (enemyCardPoints[i].activeCard != null)
                {
                    enemyCardPoints[i].activeCard.DamageCard(playerCardPoints[i].activeCard.attackPower);
                }
                else
                {
                    BattleController.instance.DamageEnemy(playerCardPoints[i].activeCard.attackPower);
                }

                playerCardPoints[i].activeCard.animator.SetTrigger("Attack");

                yield return new WaitForSeconds(timeBetweenAttacks);
            }
        }

        CheckAssignedCard();

        BattleController.instance.AdvanceTurn();
    }

    public void EnemyAtttack()
    {
        StartCoroutine(EnemyAttackCo());    
    }

    IEnumerator EnemyAttackCo()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);

        for (int i = 0; i < enemyCardPoints.Length; i++)
        {
            if (enemyCardPoints[i].activeCard != null)
            {
                if (playerCardPoints[i].activeCard != null)
                {
                    playerCardPoints[i].activeCard.DamageCard(enemyCardPoints[i].activeCard.attackPower);
                }
                else
                {
                    BattleController.instance.DamagePlayer(enemyCardPoints[i].activeCard.attackPower);
                }

                enemyCardPoints[i].activeCard.animator.SetTrigger("Attack");

                yield return new WaitForSeconds(timeBetweenAttacks);
            }
        }

        CheckAssignedCard();

        BattleController.instance.AdvanceTurn();
    }


    public void CheckAssignedCard()
    {
        foreach (CardPlacePoint point in enemyCardPoints)
        {
            if(point.activeCard != null)
            {
                if(point.activeCard.currentHealth <= 0 )
                {
                    point.activeCard.currentHealth = 0;
                }
            }
        }

        foreach (CardPlacePoint point in playerCardPoints)
        {
            if (point.activeCard != null)
            {
                if (point.activeCard.currentHealth <= 0)
                {
                    point.activeCard.currentHealth = 0;
                }
            }
        }
    }
}
