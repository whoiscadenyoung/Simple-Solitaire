using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBehavior : MonoBehaviour
{
    private Sprite cardFace;
    private Sprite cardBack;
    public bool faceUp = false;
    public int timer;
    public bool turning = false;

    private SpriteRenderer spriteRenderer;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        int i = 0;
        foreach ((int rank, string suit) in gameManager.cards)
        {
            string cardName = rank.ToString() + "-" + suit;
            if (this.name == cardName)
            {
                cardFace = gameManager.cardFaces[i];
                break;
            }
            i++;
        }

        cardBack = gameManager.deckTypes[gameManager.deckType];
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (this.faceUp == true) spriteRenderer.sprite = cardFace;
        else spriteRenderer.sprite = cardBack;

        if (gameManager.cardCurrent != null && gameManager.cardCurrent.name == this.name)
        {
            spriteRenderer.color = Color.yellow;
        }
        else spriteRenderer.color = Color.white;
    }

    void Flip()
    {
        this.faceUp = true;
    }

    public void FlipCard()
    {
        StartCoroutine(FlipTimer());
    }

    IEnumerator FlipTimer()
    {
        int dir = 10;
        for (int i = 0; i < 18; i++)
        {
            yield return new WaitForSeconds(0.01f);
            transform.Rotate(new Vector3(0,dir,0));
            timer++;
            if (timer == 9 || timer == -9)
            {
                Flip();
                dir *= -1;
            }
        }
        timer = 0;
    }
}
