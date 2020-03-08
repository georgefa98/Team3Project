using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    /* Input */
    float h, v;
    float mouseX, mouseY;
    float jump;

    /* Stats */
    public float speed;
    public Vector2 lookSpeed;
    public float jumpIntensity;
    public float gravity;

    /* States */

    /* Physics */
    float verticalSpeed;

    /* Components */
    Camera cam;
    Rigidbody rigid;
    CharacterController charContr;

    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        rigid = GetComponent<Rigidbody>();
        charContr = GetComponent<CharacterController>();
    }

    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        jump = Input.GetAxis("Jump");

        //player and camera rotation
        transform.rotation *=  Quaternion.AngleAxis(lookSpeed.x * Time.deltaTime * mouseX, Vector3.up);
        cam.transform.rotation *= Quaternion.AngleAxis(lookSpeed.y * Time.deltaTime * mouseY, Vector3.left);

        //Movement
        
    }

    void FixedUpdate() {
        charContr.Move(transform.rotation * new Vector3(h, 0f, v).normalized * speed * Time.deltaTime + Vector3.up * verticalSpeed * Time.deltaTime);

        if(charContr.isGrounded) {
            verticalSpeed = jump * jumpIntensity;
        } else {
            verticalSpeed -= gravity * Time.deltaTime;
        }
    }

}

