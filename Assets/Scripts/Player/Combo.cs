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
    
    void Start() {
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

    public void OnReloadCombo() {
        StartCoroutine(ReloadCombo());
    }

    IEnumerator ReloadCombo() {
        yield return new WaitForSeconds(comboTime);
        isReadyState = 0;
        effect.color = new Color(1f, 1f, 0f);
    }
}