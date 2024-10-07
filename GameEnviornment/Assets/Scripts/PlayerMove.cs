using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    public float MoveSpeed = 10.0f;
    private float HorzMoveInput;
    private float VertMoveInput;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HorzMoveInput = Input.GetAxis("Horizontal") * MoveSpeed;
        VertMoveInput = Input.GetAxis("Vertical") * MoveSpeed;

        this.transform.Translate(Vector3.forward * VertMoveInput * Time.deltaTime);
        this.transform.Translate(Vector3.right * HorzMoveInput * Time.deltaTime);
    }
}
