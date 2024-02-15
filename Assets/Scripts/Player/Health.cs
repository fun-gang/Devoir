using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Health : MonoBehaviour
{
    [SerializeField] private PlayerData stats;
    private int maxHealth = 5;
    private float maxEnergy = 1f;
    private float energyRecoveringSpeed = 0.15f;
    private Rate rt;
    private int health;
    private SetHPCells ui;
    [HideInInspector] public float energy = 0;

    void Start() {
        UseModificators();
        energy = 0;
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
        if (health < maxHealth && energy >= maxEnergy) {
            energy = 0;
            health += 1;
        }
        SetUIHealthAndEnergy();
    }

    public void AddEnergy() {
        energy += energyRecoveringSpeed;
        if (energy > maxEnergy) energy = 1;
        SetUIHealthAndEnergy();
    }

    public void SetUIHealthAndEnergy() {
        ui.SetHPCellsUI(health);
        ui.SetEnergyBarUI(energy);
    }

    private void UseModificators() {
        maxHealth += stats.MaxHealthMod;
        maxEnergy += (stats.MaxEnergyMod / 100) * maxEnergy;
        energyRecoveringSpeed += (stats.EnergyRecoveringSpeedMod / 100) * energyRecoveringSpeed;
    }
}
