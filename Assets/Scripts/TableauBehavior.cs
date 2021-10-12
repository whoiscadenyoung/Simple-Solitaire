using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableauBehavior : MonoBehaviour
{
    public GameManager gameManager;

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
        if (gameManager.cardCurrent != null && gameManager.cardCurrent.GetComponent<CardBehavior>().rank == 13)
        {
            gameManager.AddToTableau(this.gameObject);
        }

    }
}
