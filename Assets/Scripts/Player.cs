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

    /* Game Stats */
    public float health;

    /* States */
    public bool alive;
    public bool vulnerable;
    public bool running;
    public bool crouching;

    private int currentItem;
    private float switchingTimer;

    public bool justStartedAiming;
    public bool justStoppedAiming;

    /* Physics */
    float verticalSpeed;

    /* Misc */
    Transform hand;

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
        
        /* Aiming */
        if(switchingTimer > 0f)
            switchingTimer -= Time.deltaTime;

        try {
            GameObject itemObj = hand.GetChild(0).gameObject;

            if(itemObj) {
                Weapon weapon = itemObj.GetComponent<Weapon>();

                if(aim && justStartedAiming) {
                    weapon.StartAiming();
                } else if(!aim && justStoppedAiming) {
                    weapon.StopAiming();
                }
            }
        } catch {

        }

        if(aim) {
            if(justStartedAiming) {
                justStartedAiming = false;
                
            }
            justStoppedAiming = true;
        } else {
            if(justStoppedAiming) {
                justStoppedAiming = false;
            }
            justStartedAiming = true;
        }

            
        /* Update rotation */
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

    public float GetWeaponCharge() {
        if(hand.transform.childCount > 0) {
            GameObject obj = hand.transform.GetChild(0).gameObject;
            Weapon weapon = obj.GetComponent<Weapon>();
            if(weapon != null) {
                return weapon.GetCharge();
            }
        }

        return 0f;
    }

}

