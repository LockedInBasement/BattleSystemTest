using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class HandController : MonoBehaviour
{
    public List<Card> heldCards = new List<Card>();
    public Transform minPos, maxPos;
    public List<Vector3> cardPosition = new List<Vector3>();

    public static HandController instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SetCardPositionInHand();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetCardPositionInHand()
    {
        cardPosition.Clear();

        Vector3 distanceBetweenPoints = Vector3.zero;
        if (heldCards.Count > 1)
        {
            distanceBetweenPoints = (maxPos.position - minPos.position) / (heldCards.Count - 1);
        }

        for (int i = 0; i < heldCards.Count; i++)
        {
            cardPosition.Add(minPos.position + (distanceBetweenPoints * i));

            //heldCards[i].transform.position = cardPosition[i];
            //heldCards[i].transform.rotation = minPos.rotation;

            heldCards[i].MoveToPoint(cardPosition[i], minPos.rotation);

            heldCards[i].inHand = true;
            heldCards[i].handPositon = i;
        }
    }

    public void RemoveCardFromHand(Card cardToRemove)
    {
        if (heldCards[cardToRemove.handPositon] == cardToRemove)
        {
            heldCards.RemoveAt(cardToRemove.handPositon);
        }
        else
        {
            Debug.LogError("Card at position " + cardToRemove.handPositon + " is not the card being removed from hand");
        }

        SetCardPositionInHand();
    }

    public void AddCardToHand(Card cardToAdd)
    {
        heldCards.Add(cardToAdd);
        SetCardPositionInHand();
    }
}
