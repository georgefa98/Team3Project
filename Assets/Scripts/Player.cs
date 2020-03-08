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

    /* States */
    public bool onGround;

    /* Components */
    Camera cam;
    Rigidbody rigid;
    CapsuleCollider capColl;

    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        rigid = GetComponent<Rigidbody>();
        capColl = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        jump = Input.GetAxis("Jump");

        //player and camera rotation
        transform.rotation *=  Quaternion.AngleAxis(lookSpeed.x * mouseX, Vector3.up);
        cam.transform.rotation *= Quaternion.AngleAxis(lookSpeed.y * mouseY, Vector3.left);

        //Movement
        transform.position += transform.rotation * new Vector3(h, 0f, v).normalized * speed;
        
        if(onGround) {
            rigid.AddForce(Vector3.up * jump * jumpIntensity, ForceMode.Impulse);
        }
    }

    void FixedUpdate() {
        Vector3 point0 = transform.position + Vector3.down * capColl.height/2f;
        Vector3 point1 = transform.position + Vector3.up * capColl.height/2f;
        Collider[] colls = Physics.OverlapCapsule(point0, point1, capColl.radius);

        if(colls.Length <= 1) {
            onGround = false;
        }
    }

    void OnCollisionEnter(Collision coll) {
        ContactPoint contact = coll.GetContact(0);
        if(Vector3.Angle(contact.normal, Vector3.up) < 45f) {
            onGround = true;
        }
    }
}

