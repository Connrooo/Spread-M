using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{

    [SerializeField] TMP_Text humanCountText;
    MenuManager menuManager;


    // Start is called before the first frame update
    void Start()
    {
        menuManager = FindObjectOfType<MenuManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CountHumans();
    }

    private void CountHumans()
    {
        int amountOfHumans = GameObject.FindGameObjectsWithTag("Human").Count();
        if (amountOfHumans <= 0)
        {
            humanCountText.text = ("Humans remaining: " + amountOfHumans);
            menuManager.GoToMenu();
        }
        else
        {
            humanCountText.text = ("Humans remaining: " + amountOfHumans);
        }    
    }



}
