using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private PlayerData stats;
    private float gunDamage = 0.1f;
    private float timeBtwShots = 0.1f;
    private float reloadTime = 1f;
    private int countOfBullets = 1;
    private float gunFireCost = 0.15f;

    public GameObject bullet;
    public Transform firePos;
    public GameObject[] effects;
    public bool isReady = true;
    public Health health;
    public float flyingSpeed = 10;
    public float rotateSpeed = 25;
    public float flyingTime = 4.5f;

    void Start() => UseModificators();
    
    public void Fire() {
        if (isReady && CheckEnergyLimit()) {
            isReady = false;
            StartCoroutine(FireCor());
        }
    }

    public void Rotatata() {
        Debug.Log("Ratatataat");
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
            newBullet.GetComponent<PlayerBullet>().damage = gunDamage;
        }
        yield return new WaitForSeconds(reloadTime);
        isReady = true;
    }

    private void UseModificators() {
        gunDamage += (stats.GunDamageMod / 100) * gunDamage;
        timeBtwShots -= (stats.TimeBtwShotsMod / 100) * timeBtwShots;
        reloadTime -= (stats.ReloadTimeMod / 100) * reloadTime;
        gunFireCost -= (stats.GunFireCostMod / 100) * gunFireCost;
        countOfBullets += stats.CountOfBulletsMod;
    }
}
