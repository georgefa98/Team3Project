using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    /* Input */
    float h, v;
    float mouseX, mouseY;

    /* Stats */
    public float speed;
    public Vector2 lookSpeed;


    /* Components */
    Camera cam;

    void Start()
    {
        cam = GetComponentInChildren<Camera>();
    }

    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        //player and camera rotation
        transform.rotation *=  Quaternion.AngleAxis(lookSpeed.x * mouseX, Vector3.up);
        cam.transform.rotation *= Quaternion.AngleAxis(lookSpeed.y * mouseY, Vector3.left);

        //Movement
        transform.position += transform.rotation * new Vector3(h, 0f, v).normalized * speed;
    }
}
