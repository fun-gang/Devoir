using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [SerializeField] private PlayerData stats;
    
    public GameObject bullet;
    public Transform firePos;
    public GameObject[] effects;
    private bool isReady = true;
    public Sword sword;
    public Health health;
    
    public void Fire(InputAction.CallbackContext value) {
        if (isReady && Movement.control && !sword.isBlock && CheckEnergyLimit()) {
            isReady = false;
            StartCoroutine(FireCor());
        }
    }

    private bool CheckEnergyLimit() {
        if (health.energy >= stats.CountOfBullets * stats.GunFireCost) return true;
        return false;
    }

    IEnumerator FireCor() {
        health.energy -= stats.CountOfBullets * stats.GunFireCost;
        health.SetUIHealthAndEnergy();
        for (int k = 0; k < stats.CountOfBullets; k ++) {
            foreach (GameObject i in effects) {
                yield return new WaitForSeconds(stats.TimeBtwShots);
                Instantiate(i, firePos.position, transform.rotation);
            }
            GameObject newBullet = Instantiate(bullet, firePos.position, Quaternion.FromToRotation (Vector3.right, transform.lossyScale.x * transform.right));
            newBullet.GetComponent<PlayerBullet>().damage = stats.GunDamage;
        }
        yield return new WaitForSeconds(stats.ReloadTime);
        isReady = true;
    }
}
