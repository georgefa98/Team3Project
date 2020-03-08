using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    /* Input */
    float h, v;
    float mouseX, mouseY;
    float jump;

    /* Movement Stats */
    public float crouchSpeed;
    public float walkSpeed;
    public float runSpeed;
    public Vector2 lookSpeed;
    public float jumpIntensity;
    public float gravity;

    /* Game Stats */
    public float health;

    /* States */
    public bool alive;
    public bool vulnerable;
    public bool running;
    public bool crouching;

    /* Physics */
    float verticalSpeed;

    /* Misc */
    GameObject hand;
    Transform upperBody;

    /* Components */
    Camera cam;
    Rigidbody rigid;
    CharacterController charContr;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        cam = GetComponentInChildren<Camera>();
        rigid = GetComponent<Rigidbody>();
        charContr = GetComponent<CharacterController>();
        upperBody = transform.GetChild(0);
    }

    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        jump = Input.GetAxis("Jump");
        running = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        crouching = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

        //player and camera rotation
        transform.rotation *=  Quaternion.AngleAxis(lookSpeed.x * Time.deltaTime * mouseX, Vector3.up);
        cam.transform.rotation *= Quaternion.AngleAxis(lookSpeed.y * Time.deltaTime * mouseY, Vector3.left);
        
    }

    void FixedUpdate() {
        float speed = walkSpeed;
        if(running && !crouching) {
            speed = runSpeed;
        } else if(crouching) {
            speed = crouchSpeed;
            //upperBody.localPosition = new Vector3(0f, 0f, 0f);
            charContr.height = 1f;
        }

        if(!crouching) {
            //upperBody.localPosition = new Vector3(0f, 0.5f, 0f);
            charContr.height = 2f;
        }

        charContr.Move(transform.rotation * new Vector3(h, 0f, v).normalized * speed * Time.deltaTime + Vector3.up * verticalSpeed * Time.deltaTime);

        if(charContr.isGrounded) {
            verticalSpeed = jump * jumpIntensity;
        } else {
            verticalSpeed -= gravity * Time.deltaTime;
        }
    }

}

