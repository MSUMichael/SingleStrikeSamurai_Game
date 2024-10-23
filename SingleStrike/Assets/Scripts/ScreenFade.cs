using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ScreenFade : MonoBehaviour
{

    private TextMeshProUGUI fade; //"fade" should be name of visual
    private float alphaRemaining = 1.0f;
    private float fadeRate = 0.012f;
    //private bool cycleFlag = false;

    private GameObject player;
    private Vector3 PlayerCoords;

    void Awake()
    {
        fade = GetComponent<TextMeshProUGUI>(); //Implies this script should be on the same object
        player = GameObject.Find("Main Camera");
        
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        PlayerCoords = player.transform.position;

        if (alphaRemaining <= 1.0f && /*cycleFlag == false*/ PlayerCoords.x >= 835)
        {
            alphaRemaining += fadeRate;
            //cycleFlag = false;
        }
        
        if (alphaRemaining >= 0.98f && /*cycleFlag == false*/ PlayerCoords.x <= 835)
        {
            alphaRemaining -= fadeRate/15;
            //cycleFlag = false;
        }

        else if (alphaRemaining >= 0.0f && /*cycleFlag == false*/ PlayerCoords.x <= 835)
        {
            alphaRemaining -= fadeRate;
            //cycleFlag = false;
        }
        /*else if (alphaRemaining <= 0.0f && cycleFlag == false)
        {
            alphaRemaining += fadeRate;
            cycleFlag = true;

        }
        else if (alphaRemaining <= 1.0f && cycleFlag == true)
        {
            alphaRemaining += fadeRate;
            cycleFlag = true;
        }
        else if (alphaRemaining >= 1.0f && cycleFlag == true)
        {
            alphaRemaining -= fadeRate;
            cycleFlag = false;
        }*/

        //Grab fade color, change alpha value, then put color back on fade

        Color aColor = fade.color;
        aColor.a = alphaRemaining;
        fade.color = aColor;
        //Debug.Log(fade.color.ToString());

        
        //Debug.Log(PlayerCoords.x.ToString());

    }
}
