using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
//Written entirely by Michael Anglemier

public class ScreenFade : MonoBehaviour
{

    private TextMeshProUGUI fade; //"fade" should be name of visual
    private float alphaRemaining = 1.0f;
    private float fadeRate = 0.012f;

    private GameObject player;
    private Vector3 PlayerCoords;

    void Awake()
    {
        fade = GetComponent<TextMeshProUGUI>(); //Implies this script should be on the same object
        player = GameObject.Find("Main Camera");

    }

    void FixedUpdate() //Fades screen dependant on player location
    {

        PlayerCoords = player.transform.position;

        if (alphaRemaining <= 1.0f && PlayerCoords.x >= 835)
        {
            alphaRemaining += fadeRate;
        }

        if (alphaRemaining >= 0.98f && PlayerCoords.x <= 835)
        {
            alphaRemaining -= fadeRate / 15;
        }

        else if (alphaRemaining >= 0.0f && PlayerCoords.x <= 835)
        {
            alphaRemaining -= fadeRate;
        }


        //Grab fade color, change alpha value, then put color back on fade

        Color aColor = fade.color;
        aColor.a = alphaRemaining;
        fade.color = aColor;
        //Debug.Log(fade.color.ToString());


        //Debug.Log(PlayerCoords.x.ToString());

    }
}