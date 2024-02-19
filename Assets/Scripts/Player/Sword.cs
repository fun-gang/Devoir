using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Sword : MonoBehaviour
{
    [SerializeField] private PlayerData stats;
    private float swordDamage = 0.15f;
    [SerializeField] private float swordDamageBlock = 2;
    public Animator playerAnim;
    public GameObject[] swordSounds;
    public bool isReady;
    public bool isBlock;
    private int attackState;
    public List<SwordsAnimations> swordAnims = new List<SwordsAnimations>();
    private float blockAnimDuration = 1;
    public Gun gun;

    void Start() {
        attackState = -1;
        playerAnim.SetInteger("Attack", attackState);
        InitializeAnimDuration();
        isReady = true;
        isBlock = false;
        swordDamage += (stats.SwordDamageMod / 100) * swordDamage;
        swordDamageBlock += (stats.SwordDamageBlockMod / 100) * swordDamageBlock;
    }

    public void Attack(InputAction.CallbackContext value) {
        if (isReady && gun.isReadyState >= 1) {
            isReady = false;

            attackState += 1;
            if (attackState >= swordAnims.Count) attackState = 0;
            playerAnim.SetInteger("Attack", attackState);
                        
            RandomSwordSound();
            StartCoroutine(SwordAnimDisable(swordAnims[attackState].duration));
        }
    }

    public void Block(InputAction.CallbackContext value) {
        if (isReady && gun.isReadyState >= 1) {
            isReady = false;
            isBlock = true;

            playerAnim.SetBool("IsBlock", true);
            StartCoroutine(SwordAnimDisable(blockAnimDuration));
        }
    }

    IEnumerator SwordAnimDisable(float time) {
        yield return new WaitForSeconds(time);
        playerAnim.SetInteger("Attack", -1);
        playerAnim.SetBool("IsBlock", false);
        isBlock = false;
        isReady = true;
    }

    private void RandomSwordSound() {
        int rand = Random.Range(0, swordSounds.Length);
        Instantiate(swordSounds[rand]);
    }

    private void InitializeAnimDuration() {
        RuntimeAnimatorController ac = playerAnim.runtimeAnimatorController;
        for (int i = 0; i < ac.animationClips.Length; i++) {
            if (ac.animationClips[i].name == "PlayerBlock") {
                blockAnimDuration = ac.animationClips[i].length;
                continue;
            }

            for (int j = 0; j < swordAnims.Count; j++) {
                if (ac.animationClips[i] == swordAnims[j].anim) {
                    swordAnims[j].duration = ac.animationClips[i].length;
                }
            }
        }
    }
}

[System.Serializable]
public class SwordsAnimations {
    public AnimationClip anim;
    [HideInInspector] public float duration;
}