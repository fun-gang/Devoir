using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sword : MonoBehaviour
{
    public Animator playerAnim;
    public GameObject[] swordSounds;
    public bool isReady;
    public bool isBlock;
    private float swordAnimTime;

    void Start() {
        isReady = true;
        RuntimeAnimatorController ac = playerAnim.runtimeAnimatorController;
        for (int i = 0; i < ac.animationClips.Length; i++) {
            if (ac.animationClips[i].name == "PlayerSwordAttack") {
                swordAnimTime = ac.animationClips[i].length;
            }
        }
        
    }

    void FixedUpdate() => playerAnim.SetBool("isBlock", isBlock);

    public void Attack(InputAction.CallbackContext value) {
        if (isReady && Movement.control && !isBlock) {
            isReady = false;
            playerAnim.SetBool("IsAttack", true);
            RandomSwordSound();
            StartCoroutine(SwordAnimDisable());
        }
    }

    IEnumerator SwordAnimDisable() {
        yield return new WaitForSeconds(swordAnimTime);
        playerAnim.SetBool("IsAttack", false);
        isReady = true;
    }

    private void RandomSwordSound() {
        int rand = Random.Range(0, swordSounds.Length);
        Instantiate(swordSounds[rand]);
    }
}
