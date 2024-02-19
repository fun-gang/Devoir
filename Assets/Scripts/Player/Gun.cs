using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [SerializeField] private PlayerData stats;
    private float gunDamage = 0.1f;
    private float timeBtwShots = 0.1f;
    private float reloadTime = 1f;
    private int countOfBullets = 1;

    public GameObject bullet;
    public Transform firePos;
    public GameObject[] effects;
    public int isReadyState;
    public Animator playerAnim;
    public Sword sword;

    void Start() { 
        UseModificators();
        isReadyState = 2;
    }
    
    public void Fire(InputAction.CallbackContext value) {
        if (isReadyState == 2 && sword.isReady) {
            isReadyState = 0;
            playerAnim.SetBool("IsFire", true);
            StartCoroutine(FireCor());
        }
    }

    IEnumerator FireCor() {
        for (int k = 0; k < countOfBullets; k ++) {
            foreach (GameObject i in effects) {
                yield return new WaitForSeconds(timeBtwShots);
                Instantiate(i, firePos.position, transform.rotation);
            }
            GameObject newBullet = Instantiate(bullet, firePos.position, Quaternion.FromToRotation (Vector3.right, transform.lossyScale.x * transform.right));
            newBullet.GetComponent<PlayerBullet>().damage = gunDamage;
        }
        playerAnim.SetBool("IsFire", false);
        playerAnim.Play("Idle");
        isReadyState = 1;
        yield return new WaitForSeconds(reloadTime);
        isReadyState = 2;
    }

    private void UseModificators() {
        gunDamage += (stats.GunDamageMod / 100) * gunDamage;
        timeBtwShots -= (stats.TimeBtwShotsMod / 100) * timeBtwShots;
        reloadTime -= (stats.ReloadTimeMod / 100) * reloadTime;
        countOfBullets += stats.CountOfBulletsMod;
    }
}
