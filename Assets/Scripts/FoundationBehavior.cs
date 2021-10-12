using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoundationBehavior : MonoBehaviour
{
    public string suit;
    public GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    private void OnMouseDown()
    {
        if (gameManager.cardCurrent != null)
        {
            if (gameManager.cardCurrent.GetComponent<CardBehavior>().suit == this.suit)
            {
                gameManager.AddToFoundation(this.gameObject);
            }
        }
    }
}
