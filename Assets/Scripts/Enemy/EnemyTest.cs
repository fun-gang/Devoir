using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    private float health = 1f;
    private Rate rt;
    private Health heal;

    void Start() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        rt = player.GetComponent<Rate>();
        heal = player.GetComponent<Health>();
    }
    
    void OnTriggerEnter2D(Collider2D hit) {
        if (hit.GetComponent<Sword>() != null || hit.GetComponent<PlayerBullet>() != null) {
            health -= 0.1f;
            rt.ChangeRate(1, false);
            if (hit.GetComponent<Sword>() != null) {
                heal.AddEnergy();
            }
        }
        if (health <= 0) Destroy(gameObject);
    }
}
