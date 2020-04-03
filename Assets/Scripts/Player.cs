using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mob
{

    /* Input */
    bool fire;
    bool reload;
    bool switchItem;
    bool aim;
    bool openInventory;
    bool interact;
    bool exit;

    /* Game Stats */
    float energy;

    /* States */
    private int currentItem;
    private float switchingTimer;

    public bool justStartedAiming;
    public bool justStoppedAiming;

    public bool uiMode;
    public string uiType;

    /* Misc */
    Transform hand;
    FPSController fps;
    Transform upperBody;
    public List<GameObject> tools;
    public InventoryUI lootingInventory;
    public InventoryUI inventory;

    void Start()
    {
        upperBody = transform.GetChild(1);
        hand = upperBody.GetChild(0);
        fps = GetComponent<FPSController>();

        if(tools.Count > 0) {
            GameObject itemObj = Instantiate(tools[0], Vector3.zero, Quaternion.identity);
            itemObj.transform.SetParent(hand);
            itemObj.transform.localPosition = Vector3.zero;
            itemObj.transform.localRotation = Quaternion.identity;
        }

        currentItem = 0;

        health = 100f;
        energy = 100f;
        alive = true;
    }

    void Update()
    {

        /*Update player input*/
        fire = Input.GetButtonDown("Fire1");
        reload = Input.GetButtonDown("Reload");
        switchItem = Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0f;
        aim = Input.GetButton("Aim");
        openInventory = Input.GetButtonDown("Inventory");
        interact = Input.GetButtonDown("Interact");
        exit = Input.GetButtonDown("Cancel");
        
        if(!uiMode) {
            /*Fire*/
            if(fire || reload) {
                try {
                    GameObject itemObj = hand.GetChild(0).gameObject;

                    if(itemObj) {
                        Tool item = itemObj.GetComponent<Tool>();

                        if(fire)
                            item.Use();
                        else if(reload)
                            item.Refill();
                    }
                } catch {

                }
            }

            /* Interact (E) */
            if(interact) {
                RaycastHit raycastHit;
                if(Physics.Raycast(upperBody.position, upperBody.forward, out raycastHit, 3f)) {
                    if(raycastHit.collider.gameObject.tag == "Interactive") {
                        lootingInventory.inventoryController = raycastHit.collider.gameObject.GetComponent<InventoryController>();
                        inventory.Toggle();
                        lootingInventory.Toggle();
                        EnterUIMode("LootingInventory");
                    }
                }
            }


            /*Switch item (scroll wheel)*/
            if(switchItem && switchingTimer <= 0f) {
                switchingTimer = 0.1f;

                Destroy(hand.GetChild(0).gameObject);

                currentItem = (currentItem + 1) % tools.Count;

                GameObject itemObj = Instantiate(tools[currentItem], Vector3.zero, Quaternion.identity);
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

            /* Inventory */
            if(openInventory) {
                inventory.Toggle();
                EnterUIMode("Inventory");
            }
            
        } else {
            if(exit) {
                LeaveUIMode();
            }
        }

        this.TakeEnergy(-Time.deltaTime);

    }

    public void EnterUIMode(string uiType) {
        this.uiType = uiType;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        uiMode = true;
        fps.uiMode = true;
    }
    public void LeaveUIMode() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        uiMode = false;
        fps.uiMode = false;

        if(uiType == "Inventory") {
            inventory.Toggle();
        } else if(uiType == "LootingInventory") {
            inventory.Toggle();
            lootingInventory.Toggle();
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

    public float Energy {
        get { return energy; }
    }

    public void TakeEnergy(float cost) {
        energy = Mathf.Clamp(energy - cost, 0f, 100f);
    }

    public override IEnumerator Die() {
        yield return new WaitForSeconds(1f);
        Debug.Log("You Died");
    }

}

