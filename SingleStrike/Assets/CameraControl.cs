using UnityEngine;
//Written by ryan reisdorf

public class CameraControl : MonoBehaviour
{
    public float moveSpeed = 10f; // Speed of camera movement
    public float verticalSpeed = 5f; // Speed for moving up and down
    public float rotationSpeed = 50f; // Speed for camera rotation

    void Update()
    {
        // Horizontal and vertical movement (Numpad keys)
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.Keypad8)) // Move forward
        {
            moveDirection += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.Keypad2)) // Move backward
        {
            moveDirection += Vector3.back;
        }
        if (Input.GetKey(KeyCode.Keypad4)) // Move left
        {
            moveDirection += Vector3.left;
        }
        if (Input.GetKey(KeyCode.Keypad6)) // Move right
        {
            moveDirection += Vector3.right;
        }

        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

        // Up and down movement (Numpad + and -)
        if (Input.GetKey(KeyCode.KeypadPlus)) // Move up
        {
            transform.Translate(Vector3.up * verticalSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.KeypadMinus)) // Move down
        {
            transform.Translate(Vector3.down * verticalSpeed * Time.deltaTime, Space.World);
        }

        // Camera rotation (Numpad 7 and 9)
        if (Input.GetKey(KeyCode.Keypad7)) // Rotate left
        {
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey(KeyCode.Keypad9)) // Rotate right
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        }
    }
}
