using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintManager : MonoBehaviour
{
    public Sprite kb;
    public Sprite xb;
    public Sprite ps;

    void Start() {    
        if (GetComponent<Image>() != null) ChangeIfUI();
        else if (GetComponent<SpriteRenderer>() != null) ChangeIfSprite();
    }

    public void ChangeIfSprite() {
        SpriteRenderer sp = GetComponent<SpriteRenderer>();

        if (PlayerInit.currentDevice == "Keyboard") {
            sp.sprite = kb;
        }
        else {
            if (OptionsMenu.gamepadDefault == 0) {
                sp.sprite = ps;
            }
            if (OptionsMenu.gamepadDefault == 1) {
                sp.sprite = xb;
            }
        }
    }

    public void ChangeIfUI() {
        Image sp = GetComponent<Image>();

        if (PlayerInit.currentDevice == "Keyboard") {
            sp.sprite = kb;
        }
        else {
            if (OptionsMenu.gamepadDefault == 0) {
                sp.sprite = ps;
            }
            if (OptionsMenu.gamepadDefault == 1) {
                sp.sprite = xb;
            }
        }
    }
}
