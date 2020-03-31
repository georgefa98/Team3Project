using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    /* Input */
    float h, v;
    float mouseX, mouseY;
    float jump;

    bool fire;
    bool reload;
    bool switchItem;
    bool aim;

    /* Movement Stats */
    public float crouchSpeed;
    public float walkSpeed;
    public float runSpeed;
    public Vector2 lookSpeed;
    public float jumpIntensity;
    public float gravity;
    public float aimTime;

    /* Game Stats */
    public float health;

    /* States */
    public bool alive;
    public bool vulnerable;
    public bool running;
    public bool crouching;

    private int currentItem;
    private float switchingTimer;

    private float aimTimer;

    /* Physics */
    float verticalSpeed;

    /* Misc */
    Transform hand;
    Vector3 handPosition;
    Vector3 aimedPosition = new Vector3(0f, -0.1f, 0.3f);

    Transform upperBody;
    public List<GameObject> inventory;

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
        upperBody = transform.GetChild(1);
        hand = upperBody.GetChild(0);
        handPosition = hand.localPosition;

        if(inventory.Count > 0) {
            GameObject itemObj = Instantiate(inventory[0], Vector3.zero, Quaternion.identity);
            itemObj.transform.SetParent(hand);
            itemObj.transform.localPosition = Vector3.zero;
            itemObj.transform.localRotation = Quaternion.identity;
        }

        currentItem = 0;
    }

    void Update()
    {

        /*Update player input*/
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        jump = Input.GetAxis("Jump");
        running = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        crouching = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

        fire = Input.GetButtonDown("Fire1");
        reload = Input.GetButtonDown("Reload");
        switchItem = Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0f;
        aim = Input.GetButton("Aim");
        
        /*Fire*/
        if(fire || reload) {
            try {
                GameObject itemObj = hand.GetChild(0).gameObject;

                if(itemObj) {
                    Item item = itemObj.GetComponent<Item>();

                    if(fire)
                        item.Use();
                    else if(reload)
                        item.Refill();
                }
            } catch {

            }
        }


        /*Switch item (scroll wheel)*/
        if(switchItem && switchingTimer <= 0f) {
            switchingTimer = 0.1f;

            Destroy(hand.GetChild(0).gameObject);

            currentItem = (currentItem + 1) % inventory.Count;

            GameObject itemObj = Instantiate(inventory[currentItem], Vector3.zero, Quaternion.identity);
            itemObj.transform.SetParent(hand);
            itemObj.transform.localPosition = Vector3.zero;
            itemObj.transform.localRotation = Quaternion.identity;

        }
        
        if(switchingTimer > 0f)
            switchingTimer -= Time.deltaTime;


        /*Reloading*/
        if(reload) {
            try {
                GameObject itemObj = hand.GetChild(0).gameObject;

                if(itemObj) {
                    Item item = itemObj.GetComponent<Item>();
                    item.Refill();
                }
            } catch {

            }
        }


        /*Aiming*/
        if(aim) {
            hand.localPosition = Vector3.Lerp(handPosition, aimedPosition, aimTimer/aimTime);
            
            if(aimTimer + Time.deltaTime <= aimTime) {
                aimTimer += Time.deltaTime;
            } else {
                aimTimer = aimTime;
            }
        }
        else {
            hand.localPosition = Vector3.Lerp(handPosition, aimedPosition, aimTimer/aimTime);

            if(aimTimer - Time.deltaTime >= 0f) {
                aimTimer -= Time.deltaTime;
            } else {
                aimTimer = 0f;
            }
        }

            
        /*Update rotation*/
        transform.rotation *=  Quaternion.AngleAxis(lookSpeed.x * mouseX, Vector3.up);
        cam.transform.rotation *= Quaternion.AngleAxis(lookSpeed.y * mouseY, Vector3.left);
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
            verticalSpeed -= gravity * Time.deltaTime;
        }
    }

}

