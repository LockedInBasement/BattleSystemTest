using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckController : MonoBehaviour
{
    public static DeckController instance;

    public List<CardScriptableObject> deckToUse = new List<CardScriptableObject>();

    private List<CardScriptableObject> activeCards = new List<CardScriptableObject>();

    public Card cardToSpawn;

    public int drawCardCost = 2;

    public float waitBetweenCardDraw = 0.25f;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.L)) 
        //{
        //    DrawCardToHand();   
        //}
    }

    public void SetUpDecck()
    {
        activeCards.Clear();

        List<CardScriptableObject> tempDeck = new List<CardScriptableObject>();
        tempDeck.AddRange(deckToUse);

        int iterations = 0;
        while (tempDeck.Count > 0 && iterations <500)
        {
            int selected = Random.Range(0, tempDeck.Count);
            activeCards.Add(tempDeck[selected]);
            tempDeck.RemoveAt(selected);

            iterations++;
        }
    }

    public void DrawCardToHand()
    {
        if(activeCards.Count == 0) 
        { 
            SetUpDecck();
        }

        Card newCard = Instantiate(cardToSpawn, transform.position, transform.rotation);
        newCard.gameObject.SetActive(true);
        newCard.cardScriptableObject = activeCards[0];
        newCard.SetupCard();

        activeCards.RemoveAt(0);

        HandController.instance.AddCardToHand(newCard);
    }

    public void DrawCardForMana()
    {
        if(BattleController.instance.playerMana >= drawCardCost)
        {
            DrawCardToHand();
            BattleController.instance.SpendPlayerMana(drawCardCost);    
        }
        else
        {
            UIController.instance.ShowManaWarning();
            UIController.instance.drawCardButton.SetActive(false);  
        }
    }

    public void DrawMultipleCard(int amountToDraw)
    {
        StartCoroutine(DrawMultipleCardCo(amountToDraw));
    }

    IEnumerator DrawMultipleCardCo(int amountToDraw)
    {
        for (int i = 0; i < amountToDraw; i++)
        {
            DrawCardToHand();

            yield return new WaitForSeconds(waitBetweenCardDraw);
        }
    }
}