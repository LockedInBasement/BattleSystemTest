using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public CardScriptableObject cardScriptableObject;

    public int currentHealth;
    public int attackPower;
    public int manaCost;

    public TMP_Text health_Text, attack_Text, mana_Text;
    public TMP_Text nameText, actionDescriptionText, loreText;

    public Image characterArt, backgroundArt;

    // Start is called before the first frame update
    void Start()
    {
        SetupCard();
    }

    public void SetupCard()
    {
        currentHealth = cardScriptableObject.currentHealth;
        attackPower = cardScriptableObject.attackPower;
        manaCost = cardScriptableObject.manaCost;

        health_Text.text = currentHealth.ToString();
        attack_Text.text = attackPower.ToString();
        mana_Text.text = manaCost.ToString();

        nameText.text = cardScriptableObject.cardName;
        actionDescriptionText.text = cardScriptableObject.actionDescripton;
        loreText.text = cardScriptableObject.cardLore;

        characterArt.sprite = cardScriptableObject.characterSprite;
        backgroundArt.sprite = cardScriptableObject.bgSprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
