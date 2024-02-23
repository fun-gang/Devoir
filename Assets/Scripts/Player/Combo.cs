using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Combo : MonoBehaviour
{
    [SerializeField] private PlayerData stats;
    [SerializeField] private ComboUI ui;
    private float comboTime = 10f;
    [HideInInspector] public int isReadyState;
    public Movement movem;
    public SpriteRenderer effect;
    private Animator anim;
    private float ComboDamage = 10;
    public List<AnimationClip> comboAnims = new List<AnimationClip>();
    
    void Start() {
        anim = GetComponent<Animator>();
        comboTime -= (stats.ComboTimeMod / 100) * comboTime;
        isReadyState = 0;
        effect.color = new Color(1f, 1f, 0f);
    }

    public void Actvate(InputAction.CallbackContext value) {
        if (isReadyState == 0) {
            ui.Show();
            isReadyState = 1;
            ui.gameObject.transform.localScale = new Vector3(movem.direction,1,1);
            effect.color = new Color(1f, 0f, 0f);
        }
        else if (isReadyState == 1) {
            ui.Click();
        }
    }

    public void OnReloadCombo(List<float> power) {
        StartCoroutine(ActivateAnims(power));
    }

    IEnumerator ActivateAnims(List<float> power) {
        foreach (float pow in power) {
            int randAnim = Random.Range(0, comboAnims.Count);

            float comboMod = ComboDamage - pow * ComboDamage;
            stats.GunDamageMod += comboMod;
            stats.SwordDamageMod += comboMod;

            anim.SetInteger("Combo", randAnim+1);
            yield return WaitNextAct(comboAnims[randAnim].length);
            
            stats.GunDamageMod -= comboMod;
            stats.SwordDamageMod -= comboMod;
        }
        anim.SetInteger("Combo", 0);
        Movement.control = true;
        StartCoroutine(ReloadCombo());
    }

    IEnumerator WaitNextAct(float length) {
        yield return new WaitForSeconds(length);
        anim.SetInteger("Combo", 0);
        yield return new WaitForSeconds(0.025f);
    }

    IEnumerator ReloadCombo() {
        yield return new WaitForSeconds(comboTime);
        isReadyState = 0;
        effect.color = new Color(1f, 1f, 0f);
    }
}