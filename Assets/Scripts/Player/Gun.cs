using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    private int countOfBullets;
    private float timeBtwShots;
    private float reloadTime;
    private float damage;
    
    public GameObject bullet;
    public Transform firePos;
    public GameObject[] effects;
    private bool isReady = true;
    public Sword sword;
    public Health health;
    private float gunFireCost;

    void Start() {
        InitParams();
    }

    public void Fire(InputAction.CallbackContext value) {
        if (isReady && Movement.control && !sword.isBlock && CheckEnergyLimit()) {
            isReady = false;
            StartCoroutine(FireCor());
        }
    }

    private bool CheckEnergyLimit() {
        if (health.energy >= countOfBullets * gunFireCost) return true;
        return false;
    }

    IEnumerator FireCor() {
        health.energy -= countOfBullets * gunFireCost;
        health.SetUIHealthAndEnergy();
        for (int k = 0; k < countOfBullets; k ++) {
            foreach (GameObject i in effects) {
                yield return new WaitForSeconds(timeBtwShots);
                Instantiate(i, firePos.position, transform.rotation);
            }
            GameObject newBullet = Instantiate(bullet, firePos.position, Quaternion.FromToRotation (Vector3.right, transform.lossyScale.x * transform.right));
            newBullet.GetComponent<PlayerBullet>().damage = damage;
        }
        yield return new WaitForSeconds(reloadTime);
        isReady = true;
    }

    private void InitParams() {
        countOfBullets = PlayerStats.CountOfBullets;
        timeBtwShots = PlayerStats.TimeBTWshots;
        reloadTime = PlayerStats.GunReloadTime;
        damage = PlayerStats.GunDamage;
        gunFireCost = PlayerStats.GunFireCost;
    }
}
