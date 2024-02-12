using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Drone : MonoBehaviour
{
    private Vector2 movement = Vector2.zero;
    private Rigidbody2D rb = null;
    private CinemachineVirtualCamera cmv;
    private GameObject player;
    private bool isActive = false;
    public int speed;

    private BoxCollider2D boxColl;
    private Transform botPos;
    public LayerMask groundLayer;
    public Transform origin;
    void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        boxColl = gameObject.GetComponent<BoxCollider2D>();
        cmv = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
        player = GameObject.Find("Player");
        botPos = GameObject.Find("BoxPos").transform;
        ChangePhysics(true);
    }
    public void Act() {
        transform.position = botPos.position;
        isActive = true;
        cmv.Follow = transform;
        Movement.control = false;
        ChangePhysics(false);
    }

    public void Stop() {
        isActive = false;
        cmv.Follow = player.transform;
        Movement.control = true;
        ChangePhysics(true);
        rb.velocity = movement = Vector2.zero;

        RaycastHit2D ray;
        ray = Physics2D.Raycast(origin.position, Vector2.down, 100, groundLayer); 
        if (ray.collider != null) {
            transform.position = new Vector3(transform.position.x, transform.position.y - ray.distance, 0);
        }
    }
    private void FixedUpdate()
    {
        if (isActive) MovePlayer();    
    }

    private void MovePlayer () {
        movement = player.GetComponent<PlayerInit>().movement;
        if (movement.x > 0) transform.rotation = Quaternion.Euler(0, 0, 0);
        if (movement.x < 0) transform.rotation = Quaternion.Euler(0, 180, 0);
        rb.velocity = new Vector2(movement.x * speed * Time.fixedDeltaTime * 10, movement.y * speed * Time.fixedDeltaTime * 5);
    }
    
    public void Repair() {
        Debug.Log("repair");
    }

    private void ChangePhysics(bool toStatic) {
        if (toStatic == true) rb.bodyType = RigidbodyType2D.Kinematic;
        else rb.bodyType = RigidbodyType2D.Dynamic;
        boxColl.isTrigger = toStatic;
    }
}