using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckBehavior : MonoBehaviour
{
    private void OnMouseDown()
    {
        FindObjectOfType<GameManager>().DeckClicked();
    }
}
