using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DropdownBehavior : MonoBehaviour
{
    public Dropdown dropdown;
    // Start is called before the first frame update
    void Start()
    {
        dropdown.value = PlayerPrefs.GetInt("back");
    }
    public void UpdateDropdown()
    {
        switch(dropdown.value)
        {
            case 0: PlayerPrefs.SetInt("back", 0);
                break;
            case 1: PlayerPrefs.SetInt("back", 1);
                break;
        }
    }
}
