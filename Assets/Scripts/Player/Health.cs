using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Health : MonoBehaviour
{
    [SerializeField] private PlayerData stats;
    private Rate rt;
    private int health;
    private SetHPCells ui;
    [HideInInspector] public float energy = 0;

    void Start() {
        energy = 0;
        health = stats.MaxHealth;
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
        if (health < stats.MaxHealth && energy >= stats.MaxEnergy) {
            energy = 0;
            health += 1;
        }
        SetUIHealthAndEnergy();
    }

    public void AddEnergy() {
        energy += stats.EnergyRecoveringSpeed;
        if (energy > stats.MaxEnergy) energy = 1;
        SetUIHealthAndEnergy();
    }

    public void SetUIHealthAndEnergy() {
        ui.SetHPCellsUI(health);
        ui.SetEnergyBarUI(energy);
    }
}
