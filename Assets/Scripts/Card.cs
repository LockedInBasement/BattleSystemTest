using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class Card : MonoBehaviour
{
    public CardScriptableObject cardScriptableObject;

    public bool isPlayer;

    public int currentHealth;
    public int attackPower;
    public int manaCost;

    public TMP_Text health_Text, attack_Text, mana_Text;
    public TMP_Text nameText, actionDescriptionText, loreText;

    public Image characterArt, backgroundArt;

    public float moveSpeed = 5f;
    public float rotateSpeed = 540f;

    private Vector3 targetPoint;
    private Quaternion targetRotation;

    public bool inHand;
    public int handPositon;

    private HandController handController;
    private bool isSelected;
    private Collider cardCollider;

    public LayerMask whatIsDesktop;
    public LayerMask whatIsPlacement;
    public bool justPressed;

    public CardPlacePoint assignedPlace;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        if(targetPoint == Vector3.zero)
        {
            targetPoint = transform.position;
            targetRotation = transform.rotation;
        }

        SetupCard();

        handController = FindObjectOfType<HandController>();
        cardCollider = GetComponent<Collider>();
    }

    public void SetupCard()
    {
        currentHealth = cardScriptableObject.currentHealth;
        attackPower = cardScriptableObject.attackPower;
        manaCost = cardScriptableObject.manaCost;

        UpdateCardDisplay();

        nameText.text = cardScriptableObject.cardName;
        actionDescriptionText.text = cardScriptableObject.actionDescripton;
        loreText.text = cardScriptableObject.cardLore;

        characterArt.sprite = cardScriptableObject.characterSprite;
        backgroundArt.sprite = cardScriptableObject.bgSprite;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPoint, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);

        if (isSelected)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f, whatIsDesktop))
            {
                MoveToPoint(hit.point + new Vector3(0f, 2f, 0f), Quaternion.identity);
            }

            if(Input.GetMouseButtonDown(1))
            {
                ReturnToHand();
            }

            if (Input.GetMouseButtonDown(0) && justPressed == false)
            {
                if (Physics.Raycast(ray, out hit, 100f, whatIsPlacement) && BattleController.instance.currentPhase == BattleController.TurnOrder.playerActive)
                {
                    CardPlacePoint selectedPoint = hit.collider.GetComponent<CardPlacePoint>();

                    if(selectedPoint.activeCard == null && selectedPoint.isPlayerPoint) 
                    {
                        if(BattleController.instance.playerMana >= manaCost)
                        {
                            selectedPoint.activeCard = this;
                            assignedPlace = selectedPoint;

                            MoveToPoint(selectedPoint.transform.position, Quaternion.identity);

                            inHand = false;
                            isSelected = false;

                            handController.RemoveCardFromHand(this);

                            BattleController.instance.SpendPlayerMana(manaCost);    
                        }
                        else
                        {
                            ReturnToHand();

                            UIController.instance.ShowManaWarning();
                        }
       
                    }
                    else
                    {
                        ReturnToHand();
                    }
                }
                else
                {
                    ReturnToHand();
                }
            }
        }

        justPressed = false;
    }


    public void MoveToPoint(Vector3 pointMoveTo, Quaternion rotationToMatch)
    {
        targetPoint = pointMoveTo;
        targetRotation = rotationToMatch;
    }

    private void OnMouseOver()
    {
        if(inHand && isPlayer)
        {
            MoveToPoint(handController.cardPosition[handPositon] + new Vector3(0f,1f,.5f), Quaternion.identity);
        }
    }

    private void OnMouseExit()
    {
        if (inHand)
        {
            MoveToPoint(handController.cardPosition[handPositon], handController.minPos.rotation);
        }
    }

    private void OnMouseDown()
    {
        if (inHand && BattleController.instance.currentPhase == BattleController.TurnOrder.playerActive && isPlayer)
        {
            isSelected = true;
            cardCollider.enabled = false;

            justPressed = true;
        }
    }

    public void ReturnToHand()
    {
        isSelected = false;
        cardCollider.enabled = true;

        MoveToPoint(handController.cardPosition[handPositon], handController.minPos.rotation);
    }

    public void DamageCard(int damageAmount)
    {
        currentHealth -= damageAmount;

        if(currentHealth < 0)
        {
            currentHealth = 0;

            assignedPlace.activeCard = null;

            MoveToPoint(BattleController.instance.discardPoint.position, BattleController.instance.discardPoint.rotation);

            animator.SetTrigger("Jump");

            Destroy(gameObject, 5f);
        }

        animator.SetTrigger("Hurt");

        UpdateCardDisplay();
    }

    public void UpdateCardDisplay()
    {
        health_Text.text = currentHealth.ToString();
        attack_Text.text = attackPower.ToString();
        mana_Text.text = manaCost.ToString();
    }
}
