using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Health : MonoBehaviour
{
    [SerializeField] private PlayerData stats;
    private int maxHealth = 5;
    private int health;
    private SetHPCells ui;
    public GameObject obereg;
    [SerializeField] private float framesAfterHurt = 3.5f;

    void Start() {
        maxHealth += stats.MaxHealthMod;
        health = maxHealth;
        ui = GameObject.FindGameObjectWithTag("MenuDrop").GetComponent<SetHPCells>();
        SetUIHealthAndEnergy();
    }

    void OnTriggerEnter2D(Collider2D hit) {
        if (hit.tag == "Hit1") {
            health -= 1;
        }
        if (hit.tag == "Hit2") {
            health -= 2;
        }
        SetUIHealthAndEnergy();
    }

    public void RecoverHP(InputAction.CallbackContext value) {
        health = maxHealth;
        SetUIHealthAndEnergy();
    }

    public void SetUIHealthAndEnergy() => ui.SetHPCellsUI(health);
}
