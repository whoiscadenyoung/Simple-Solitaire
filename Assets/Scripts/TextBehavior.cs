using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBehavior : MonoBehaviour
{
    GameManager gameManager;
    Text card;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        card = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.cardCurrent != null)
        {
            card.text = gameManager.cardCurrent.GetComponent<CardBehavior>().CardName();
        }
    }
}
