using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Health : MonoBehaviour
{
    private Rate rt;
    private int health;
    private int maxHealth;
    private SetHPCells ui;
    public float energy = 0;
    private float recoveringSpeed;

    void Start() {
        energy = 0;
        maxHealth = PlayerStats.MaxHealth;
        recoveringSpeed = PlayerStats.RecoveringSpeed;
        health = maxHealth;
        rt = GetComponent<Rate>();
        ui = GameObject.FindGameObjectWithTag("MenuDrop").GetComponent<SetHPCells>();
        SetUIHealthAndEnergy();
    }

    void OnTriggerEnter2D(Collider2D hit) {
        if (hit.tag == "Hit1" || hit.tag == "Hit2") {
            rt.ChangeRate(0, true);
            if (hit.tag == "Hit1") {
                health -= 1;
            }
            if (hit.tag == "Hit2") {
                health -= 2;
            }
            SetUIHealthAndEnergy();
        }
    }

    public void RecoverHP(InputAction.CallbackContext value) {
        if (health < maxHealth && energy >= 1) {
            energy = 0;
            health += 1;
        }
        SetUIHealthAndEnergy();
    }

    public void AddEnergy() {
        energy += recoveringSpeed;
        if (energy > 1) energy = 1;
        SetUIHealthAndEnergy();
    }

    public void SetUIHealthAndEnergy() {
        ui.SetHPCellsUI(health);
        ui.SetEnergyBarUI(energy);
    }
}
