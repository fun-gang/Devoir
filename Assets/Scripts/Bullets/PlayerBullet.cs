using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour {
    public GameObject particle;
    public float bulletSpeed;
    [HideInInspector] public float damage;

    void Start() {
        StartCoroutine(DestroyTime(0.8f));
    }

    void Update() {
        transform.Translate(Vector2.right * bulletSpeed * Time.fixedDeltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(particle,transform.position, transform.rotation);
        Destroy(gameObject);
    }

    IEnumerator DestroyTime(float t) {
        yield return new WaitForSeconds(t);
        Instantiate(particle, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}