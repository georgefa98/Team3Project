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
    public float maxEnergy;
    float energy;
    
    /* Status Effects */
    public List<StatusEffect> statusEffects;
    public List<float> durationLeft;

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

    

    public InventoryUI handInventory;
    public InventoryUI bagInventory;
    public InventoryUI lootingInventory;

    void Start()
    {
        upperBody = transform.GetChild(1);
        hand = upperBody.GetChild(0);
        fps = GetComponent<FPSController>();
        statusEffects = new List<StatusEffect>();

        if(tools.Count > 0) {
            GameObject itemObj = Instantiate(tools[0], Vector3.zero, Quaternion.identity);
            itemObj.transform.SetParent(hand);
            itemObj.transform.localPosition = Vector3.zero;
            itemObj.transform.localRotation = Quaternion.identity;
        }

        currentItem = 0;

        health = maxHealth;
        energy = maxEnergy;
        alive = true;

        InventoryController handInventoryContr, bagInventoryContr;
        InventoryController[] ics = GetComponents<InventoryController>();
        if(ics[0].inventoryName == "Hand") {
            handInventoryContr = ics[0];
            bagInventoryContr = ics[1];
        } else {
            handInventoryContr = ics[1];
            bagInventoryContr = ics[0];
        }

        handInventory.inventoryController = handInventoryContr;
        bagInventory.inventoryController = bagInventoryContr;
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
                        lootingInventory.inventoryName = raycastHit.collider.gameObject.name;
                        lootingInventory.inventoryController = raycastHit.collider.gameObject.GetComponent<InventoryController>();
                        handInventory.Toggle();
                        bagInventory.Toggle();
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
                handInventory.Toggle();
                bagInventory.Toggle();
                EnterUIMode("Inventory");
            }
            
        } else {
            if(exit) {
                LeaveUIMode();
            }
        }

        this.TakeEnergy(Time.deltaTime);

        for(int i = 0; i < statusEffects.Count; i++) {
            StatusEffect s = statusEffects[i];
            switch(s.statEffected) {
                case StatusEffect.StatEffected.Health:
                    TakeDamage(-(s.amount/s.duration) * Time.deltaTime);
                    break;
                case StatusEffect.StatEffected.Energy:
                    TakeEnergy(-(s.amount/s.duration) * Time.deltaTime);
                    break;
            }

            durationLeft[i] -= Time.deltaTime;
            if(durationLeft[i] < 0) {
                statusEffects.RemoveAt(i);
                durationLeft.RemoveAt(i);
            }
        }

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
            handInventory.Toggle();
            bagInventory.Toggle();
        } else if(uiType == "LootingInventory") {
            handInventory.Toggle();
            bagInventory.Toggle();
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

    public void AddStatusEffect(StatusEffect s) {
        statusEffects.Add(s);
        durationLeft.Add(s.duration);
    }

    public override IEnumerator Die() {
        yield return new WaitForSeconds(1f);
        Debug.Log("You Died");
    }

    

}

