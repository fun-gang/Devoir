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
    private bool isResist = false;
    [SerializeField] private float resistanceFrames = 1.5f;

    void Start() {
        maxHealth += stats.MaxHealthMod;
        resistanceFrames += (stats.ResistanceFramesMod / 100) * resistanceFrames;
        health = maxHealth;
        ui = GameObject.FindGameObjectWithTag("MenuDrop").GetComponent<SetHPCells>();
        SetUIHealthAndEnergy();
    }

    void OnTriggerEnter2D(Collider2D hit) {
        if (hit.tag == "Hit1" && !isResist) {
            health -= 1;
            StartCoroutine(Resistance());
        }
        if (hit.tag == "Hit2" && !isResist) {
            health -= 2;
            StartCoroutine(Resistance());
        }
        SetUIHealthAndEnergy();
    }

    public void RecoverHP(InputAction.CallbackContext value) {
        health = maxHealth;
        SetUIHealthAndEnergy();
    }

    public void SetUIHealthAndEnergy() => ui.SetHPCellsUI(health);

    IEnumerator Resistance() {
        isResist = true;
        obereg.SetActive(true);
        yield return new WaitForSeconds(resistanceFrames);
        obereg.SetActive(false);
        isResist = false;
    }
}
