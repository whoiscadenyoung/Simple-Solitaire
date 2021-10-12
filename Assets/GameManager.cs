using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Sprite[] cardFaces;
    public Sprite[] deckTypes;

    public GameObject cardPrefab;

    public GameObject[] tableauPositions;
    public GameObject[] foundationPositions;
    public GameObject[] deckPositions;

    // Lists representing cards
    public List<(int, string)> cards;
    public List<(int, string)> deck;

    // Card selectors
    public GameObject cardCurrent = null;
    public GameObject cardTarget = null;
    public GameObject cardSelected = null;

    // Suits
    public static string[] suits = new string[] {"clubs", "diamonds", "hearts", "spades"};

    // Ranks: ace, 2-10, jack, queen, king
    public static int[] ranks = new int[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13};

    // Deck types: 0 is red, 1 is blue
    public int deckType = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Create the list of tuples representing all card options in order
        cards = CreateDeck();

        // Assign suits to the foundation cards
        for (int i = 0; i < foundationPositions.Length; i++)
        {
            foundationPositions[i].GetComponent<FoundationBehavior>().suit = suits[i];
            foundationPositions[i].name = "0-" + suits[i];
        }

        // Set the deck type to the player's choice
        deckType = PlayerPrefs.GetInt("back");

        // Initiate the game: shuffling, dealing, etc.
        StartPlaying();
    }

    // Update is called once per frame
    void Update()
    {
    }

    /// <summary>
	/// Called from the Start function to run the game after everything is set up
	/// </summary>
    public void StartPlaying()
    {
        deck = CreateDeck();
        Shuffle(deck);
        DealCards();
    }

    /// <summary>
	/// Create a list of tuples representing all the cards in a deck
	/// </summary>
	/// <returns>List of (int, string)</returns>
    public static List<(int, string)> CreateDeck()
    {
        List<(int, string)> allCards = new List<(int, string)>();
        foreach (int r in ranks)
        {
            foreach (string s in suits)
            {
                allCards.Add((r, s));
                
            }
        }
        return allCards;
    }

    /// <summary>
	/// Shuffle objects in place in a list
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="list">List of objects</param>
    void Shuffle<T>(List<T> list)
    {
        System.Random random = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            int k = random.Next(n);
            n--;
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    /// <summary>
    /// If all the foundation piles are full, you have won the game.
    /// </summary>
    void WonGame()
    {
        Debug.Log("You have won the game");
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
    }

    /// <summary>
	/// Creates game objects from the deckand deals them to the piles on screen
	/// </summary>
    void DealCards()
    {
        int d = 0;

        // Place the cards into the Tableau columns
        for (int i = 0; i < 7; i++)
        {
            // For each column, the number of cards is equal to the column number
            List<(int, string)> column = deck.GetRange(d, i + 1);
            d += i + 1;

            // Create the objects in the correct places in the columns
            // Offsets for y and z ensure cards sit on top of each other and in the stack correctly
            for (int j = 0; j < column.Count; j++)
            {
                // Create the card
                GameObject newCard = Instantiate(
                    cardPrefab,
                    new Vector3(
                        tableauPositions[i].transform.position.x,
                        tableauPositions[i].transform.position.y - (0.6f * j),
                        tableauPositions[i].transform.position.z - (0.03f * (j + 1))
                    ),
                    Quaternion.identity,
                    tableauPositions[i].transform
                );
                // Set the name of the card
                newCard.name = column[j].Item1.ToString() + "-" + column[j].Item2;
                newCard.tag = "Tableau";

                // Set the card rank and suit
                newCard.GetComponent<CardBehavior>().rank = column[j].Item1;
                newCard.GetComponent<CardBehavior>().suit = column[j].Item2;

                // Set the face if necessary
                if (j == column.Count - 1) newCard.GetComponent<SpriteBehavior>().faceUp = true;
            }
        }

        // Put remaining cards in deck
        List<(int, string)> drawPile = deck.GetRange(d, deck.Count - d);
        for (int i = 0; i < drawPile.Count; i++)
        {
            // Create the card
            GameObject newCard = Instantiate(cardPrefab,
                new Vector3(
                    deckPositions[0].transform.position.x,
                    deckPositions[0].transform.position.y,
                    deckPositions[0].transform.position.z - (0.03f * (i + 1))
                ),
                Quaternion.identity,
                deckPositions[0].transform
            );
            // Set the name of the card
            newCard.name = drawPile[i].Item1.ToString() + "-" + drawPile[i].Item2;
            newCard.tag = "Deck";

            // Set the card rank and suit
            newCard.GetComponent<CardBehavior>().rank = drawPile[i].Item1;
            newCard.GetComponent<CardBehavior>().suit = drawPile[i].Item2;
        }
    }

    /// <summary>
	/// Handles behavior upon clicking a card
	/// </summary>
	/// <param name="cardName">Name of the card clicked</param>
    public void CardClicked(string cardName)
    {
        /*
        // Find the card the user selected, assign it to current or target
        cardSelected = GameObject.Find(cardName);

        if (cardSelected != null)
        {
            // If it's in the deck, draw it and skip the other mechanics
            if (cardSelected.tag == "Deck") DrawCard(cardSelected);

            // Otherwise, switch the pointers around and move the card accordingly
            else if (cardCurrent != null)
            {
                if (cardSelected.name != cardCurrent.name)
                {
                    cardTarget = cardSelected;
                    MoveCard();
                }
                else cardCurrent = null;
            }
            else cardCurrent = cardSelected;
        }
        Debug.Log(cardSelected.name + cardCurrent.name + cardTarget.name);
        */
    }

    public void SelectCard(GameObject cardSelected)
    {
        cardCurrent = cardSelected;
    }

    public void DeselectAll()
    {
        Debug.Log("All cards are being deselected");
        cardCurrent = null;
        cardSelected = null;
        cardTarget = null;
        if (cardCurrent == null) Debug.Log("Current has been removed");
    }

    // TODO: We need mechanics for what to do when clicking directly on the deck, tableau, and foundation piles...

    /// <summary>
	/// After assigning the current and target cards, move it based on the game mechanics
	/// </summary>
    public void MoveCard()
    {
        /*
        switch (cardTarget.tag)
        {
            // Card you targeted is in the deck; based on physics, you can only select top one
            case "Deck":
                break;
            // Card you targeted is in the tableau
            case "Tableau":
                if (cardCurrent.GetComponent<CardBehavior>().CompareTableau(cardTarget))
                    AddToTableau(cardTarget);
                break;
            // Card you targeted is in the foundation
            case "Foundation":
                if (cardCurrent.GetComponent<CardBehavior>().CompareFoundation(cardTarget))
                    AddToFoundation(cardTarget);
                break;
            // Card you targeted is in the discard pile
            case "Discard":
                break;
        }
        */
    }

    /// <summary>
	/// Draw the top card from the deck by moving it to the location of discard pile
	/// </summary>
    public void DrawCard(GameObject cardSelected)
    {
        cardSelected.GetComponent<SpriteBehavior>().FlipCard();
        cardSelected.transform.position =
            new Vector3(
                deckPositions[1].transform.position.x,
                deckPositions[1].transform.position.y,
                deckPositions[1].transform.position.z - ((GameObject.FindGameObjectsWithTag("Discard").Length + 1) * 0.03f)
            );
        
        cardSelected.tag = "Discard";
    }

    /// <summary>
	/// When the bottom of the deck is clicked, the deck flips back so you can shuffle through the cards again
	/// </summary>
    public void DeckClicked()
    {
        // Create a list of discard pile in order to put it back in the deck
        List<GameObject> discardPile = GameObject.FindGameObjectsWithTag("Discard").OrderBy(gameObject => gameObject.transform.position.z).ToList();

        // Move the cards from the discard back to the normal deck;
        for (int i = discardPile.Count - 1; i >= 0; i--)
        {
            GameObject cardObject = discardPile[i];
            cardObject.transform.position =
                new Vector3(
                    deckPositions[0].transform.position.x,
                    deckPositions[0].transform.position.y,
                    deckPositions[0].transform.position.z - (0.03f * (i + 1))
                );
            cardObject.GetComponent<SpriteBehavior>().faceUp = false;
            cardObject.tag = "Deck";
        }
    }

    /// <summary>
	/// Adds the card to the foundation
	/// </summary>
	/// <param name="foundation">Takes the target object from the foundation or card clicked</param>
    public void AddToFoundation(GameObject foundationCard)
    {
        GameObject parent = foundationCard.tag == "FoundationBase" ? foundationCard : foundationCard.transform.parent.gameObject;

        // Add the card to the foundation since it already passsed all the checks
        cardCurrent.transform.position =
            new Vector3(
                foundationCard.transform.position.x,
                foundationCard.transform.position.y,
                foundationCard.transform.position.z - 0.03f
            );
        cardCurrent.tag = "Foundation";
        cardCurrent.transform.SetParent(parent.transform);
        DeselectAll();

        int numFull = 0;
        for (int i = 0; i < foundationPositions.Length; i++)
        {
            if (foundationPositions[i].transform.childCount == 13) numFull++;
        }
        if (numFull == 4) WonGame();
    }

    public void AddToTableau(GameObject tableauCard)
    {
        // You will be receiving the tableau card object directly from the card
        // You just have to add the current card on top of this card, since all the checks were already done
        float yOffset = tableauCard.tag == "TableauBase" ? 0f : 0.6f;
        GameObject parent = tableauCard.transform.childCount > 0 ? tableauCard : tableauCard.transform.parent.gameObject;

        /* I tried to make moving stacks work but it didn't
        if (cardCurrent.tag == "Tableau" && cardCurrent.transform.GetSiblingIndex() != cardCurrent.transform.parent.childCount - 1) {
            for (int i = cardCurrent.transform.GetSiblingIndex(); i < cardCurrent.transform.parent.childCount; i++)
            {
                GameObject childObject = cardCurrent.transform.parent.GetChild(i).gameObject;
                childObject.transform.position =
                    new Vector3(
                        tableauCard.transform.position.x,
                        tableauCard.transform.position.y - (0.6f * i),
                        tableauCard.transform.position.z - (0.03f * i)
                    );
                childObject.transform.SetParent(parent.transform);
            }
        }*/
        cardCurrent.transform.position =
        new Vector3(
            tableauCard.transform.position.x,
            tableauCard.transform.position.y - yOffset,
            tableauCard.transform.position.z - 0.03f
        );
        cardCurrent.transform.SetParent(parent.transform);
        cardCurrent.tag = "Tableau";
        DeselectAll();

        /*
        // Find card from its location in either foundation, discard, or another tableau when it's clicked
        // Remove the card from there and add it to the proper tableau pile
        // Simultaneously update the sprite
        Card thisCard;
        GameObject tableauObject = tableauPositions[t];
        switch (cardCurrent.GetComponent<CardBehavior>().position)
        {
            case 1:
                thisCard = decks[1].Find(c => c.image == cardCurrent.name);
                tableau[t].Add(thisCard);
                decks[1].Remove(thisCard);
                break;
            case 2:
                int col;
                for (col = 0; col < tableau.Length; col++)
                {
                    thisCard = tableau[col].Find(c => c.image == cardCurrent.name);
                    if (thisCard != null)
                    {
                        tableau[t].Add(thisCard);
                        tableau[col].Remove(thisCard);
                        break;
                    }
                }
                break;
        }
        float zOff = 0.03f * tableau[t].Count;
        float yOff = 0.6f * tableau[t].Count - 1;
        cardCurrent.transform.position = new Vector3(tableauObject.transform.position.x, tableauObject.transform.position.y - yOff, tableauObject.transform.position.z - zOff);
        cardCurrent.GetComponent<CardBehavior>().position = 3;
        cardCurrent = null;
        cardTarget = null;
        */
    }
    public void FlipNextCard()
    {
        // If the card is on top of Tableau, and it's been clicked, and it's currently face down, flip card over in place
        // If the card is on top of deck, and it's currently face down, flip card over in place
    }
}