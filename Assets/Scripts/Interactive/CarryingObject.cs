using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryingObject : MonoBehaviour
{
    private Transform boxPos;
    private BoxCollider2D boxColl;
    public LayerMask groundLayer;
    public Transform origin;
    private CircleCollider2D ccol;

    void Start() {
        boxPos = GameObject.Find("BoxPos").transform;

        boxColl = gameObject.GetComponent<BoxCollider2D>();

        boxColl.isTrigger = true;
        ccol = gameObject.GetComponent<CircleCollider2D>();
    }

    public void Act() {
        transform.parent = boxPos;
        transform.localPosition = Vector2.zero;
        boxColl.isTrigger = false;
        ccol.enabled = true;
    }

    public void Stop() {
        transform.parent = null;
        boxColl.isTrigger = true;

        RaycastHit2D ray;
        ray = Physics2D.Raycast(origin.position, Vector2.down, 100, groundLayer); 
        if (ray.collider != null) {
            transform.position = new Vector3(transform.position.x, transform.position.y - ray.distance, 0);
            ccol.enabled = false;
        }
    }
}
