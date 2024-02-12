using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintManager : MonoBehaviour
{
    public Sprite keyboard;
    public Sprite gamepadPS;
    public Sprite gamepadXB;
    public Image img;

    void Start() {
        img.color = new Color(255f,1f,1f,0f);
    }

    public void ShowHint() {
        img.color = new Color(255f,1f,1f,0.8f);
        if (PlayerInit.currentDevice == "Gamepad") {
            if (OptionsMenu.gamepadDefault == 0) img.sprite = gamepadPS;
            else if (OptionsMenu.gamepadDefault == 1) img.sprite = gamepadXB;
        }
        else if (PlayerInit.currentDevice == "Keyboard") {
            img.sprite = keyboard;
        }
    }

    public void HideHint() {
        img.color = new Color(255f,1f,1f,0f);
    }
}