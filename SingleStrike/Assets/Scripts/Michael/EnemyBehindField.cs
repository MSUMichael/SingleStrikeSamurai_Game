using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
//Written entirely by Michael Anglemier

public class EnemyBehindField : MonoBehaviour
{

    private GameObject player;
    private Vector3 PlayerCoords;

    private bool teleported = false;


    void Awake()
    {
        player = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerCoords = player.transform.position;

        if (PlayerCoords.x >= 395 && !teleported)
        {
            transform.position = new Vector3(385.0f, 1.0f, 0.0f);
            teleported = true;
        }
    }
}
