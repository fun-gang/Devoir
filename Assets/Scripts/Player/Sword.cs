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
    private int attackState;
    public List<SwordsAnimations> swordAnims = new List<SwordsAnimations>();

    void Start() {
        attackState = -1;
        playerAnim.SetInteger("Attack", attackState);
        InitializeAnimDuration();
        isReady = true;
    }

    void FixedUpdate() => playerAnim.SetBool("isBlock", isBlock);

    public void Attack(InputAction.CallbackContext value) {
        if (isReady && Movement.control) {
            isReady = false;

            attackState += 1;
            if (attackState >= swordAnims.Count) attackState = 0;
            playerAnim.SetInteger("Attack", attackState);
                        
            RandomSwordSound();
            StartCoroutine(SwordAnimDisable());
        }
    }

    IEnumerator SwordAnimDisable() {
        yield return new WaitForSeconds(swordAnims[attackState].duration);
        playerAnim.SetInteger("Attack", -1);
        isReady = true;
    }

    private void RandomSwordSound() {
        int rand = Random.Range(0, swordSounds.Length);
        Instantiate(swordSounds[rand]);
    }

    private void InitializeAnimDuration() {
        RuntimeAnimatorController ac = playerAnim.runtimeAnimatorController;
        for (int i = 0; i < ac.animationClips.Length; i++) {
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