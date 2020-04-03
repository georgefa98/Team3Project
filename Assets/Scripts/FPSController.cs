using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
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
    
    /* States */
    bool running;
    bool crouching;
    public bool uiMode;

    /* Physics */
    public float gravityMult;
    float verticalSpeed;

    /* Components */
    Camera cam;
    Rigidbody rigid;
    CharacterController charContr;

    // Start is called before the first frame update
    void Start()
    {

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        cam = GetComponentInChildren<Camera>();
        rigid = GetComponent<Rigidbody>();
        charContr = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {
        running = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        crouching = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        jump = Input.GetAxis("Jump");

        if(!uiMode) {
            transform.rotation *=  Quaternion.AngleAxis(lookSpeed.x * mouseX, Vector3.up);
            cam.transform.rotation *= Quaternion.AngleAxis(lookSpeed.y * mouseY, Vector3.left);
        }
        
    }

    void FixedUpdate() {

        float speed = walkSpeed;
        if(running && !crouching) {
            speed = runSpeed;

        } else if(crouching) {
            speed = crouchSpeed;
            //upperBody.localPosition = new Vector3(0f, 0f, 0f);
            charContr.height = 0.75f;
        }

        if(!crouching) {
            //upperBody.localPosition = new Vector3(0f, 0.5f, 0f);
            charContr.height = 1.5f;
        }

        charContr.Move(transform.rotation * new Vector3(h, 0f, v).normalized * speed * Time.deltaTime + Vector3.up * verticalSpeed * Time.deltaTime);

        if(charContr.isGrounded) {
            verticalSpeed = jump * jumpIntensity;
        } else {
            verticalSpeed -= Physics.gravity.magnitude * gravityMult * Time.deltaTime;
        }
    }
}
