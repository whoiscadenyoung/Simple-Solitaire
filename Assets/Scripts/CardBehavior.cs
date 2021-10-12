using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBehavior : MonoBehaviour
{
    public int rank;
    public string suit;
    // 1 = deck, 2 = tableau, 3 = foundation
    public int position;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Debug.Log("You have clicked: " + this.name);
        if (gameManager.cardCurrent != null) Debug.Log("Current is: " + gameManager.cardCurrent.name);

        // If it's in the deck, draw it
        if (this.tag == "Deck") gameManager.DrawCard(this.gameObject);

        // If it's on the tableau
        else if (this.tag == "Tableau" && this.gameObject.GetComponent<SpriteBehavior>().faceUp == false)
        {
            // If it's the top one in the upside down stack, flip it and select it
            if (this.gameObject.transform.parent.GetChild(this.gameObject.transform.parent.childCount - 1).gameObject.name == this.gameObject.name)
            {
                this.gameObject.GetComponent<SpriteBehavior>().FlipCard();
            }
        }
        // TODO: If it's on the tableau, face up, and is lower in the stack

        // If there is already a card selected then go ahead and see what it is
        else if (gameManager.cardCurrent != null)
        {
            // If the current one is different than this one, check if you can play it on this one
            if (gameManager.cardCurrent.name != this.name)
            {
                switch (this.tag)
                {
                    // Can't play the current card on the deck or discard piles
                    case "Deck":
                        gameManager.DeselectAll();
                        break;
                    case "Discard":
                        gameManager.DeselectAll();
                        break;
                    // Check if you can play the current card on this tableau card
                    case "Tableau":
                        if (gameManager.cardCurrent.GetComponent<CardBehavior>().CompareTableau(this.gameObject))
                        {
                            gameManager.AddToTableau(this.gameObject);
                        }
                        break;
                    // Check if you can play the current card on this foundation card
                    case "Foundation":
                        if (gameManager.cardCurrent.GetComponent<CardBehavior>().CompareFoundation(this.gameObject))
                        {
                            gameManager.AddToFoundation(this.gameObject);
                        }
                        break;
                }
            }
            // If the one selected is the same one, go ahead and deselect it
            else
            {
                gameManager.DeselectAll();
                if (gameManager.cardCurrent == null) Debug.Log("You have deselected:" + this.name);
            }
        }
        // If the current card is null, go ahead and find and select it to store it
        else
        {
            gameManager.SelectCard(this.gameObject);
        }
    }

    public bool CompareTableau(GameObject bottom)
    {
        return (bottom.GetComponent<CardBehavior>().ReturnColor() != this.ReturnColor()) &&
            (this.rank + 1 == bottom.GetComponent<CardBehavior>().rank);
    }
    public string ReturnColor()
    {
        string color;
        if (this.suit == "hearts" || this.suit == "diamonds") color = "red";
        else color = "black";
        return color;
    }
    public bool CompareFoundation(GameObject bottom)
    {
        return this.rank - 1 == bottom.GetComponent<CardBehavior>().rank && this.suit == bottom.GetComponent<CardBehavior>().suit;
    }
    public string CardName()
    {
        return this.rank.ToString() + " of " + this.suit;
    }
}
