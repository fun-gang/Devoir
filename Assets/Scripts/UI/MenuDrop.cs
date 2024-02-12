using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuDrop : MonoBehaviour
{   
    [SerializeField] private GameObject panel;
    [SerializeField] private Button selectionButton;
    [SerializeField] private Animator deathAnim;

    void Start () {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OpenPanel(InputAction.CallbackContext value) {
        if (Movement.control) {
            Movement.control = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            panel.SetActive(true);
            selectionButton.Select();
        }
    }

    public void PlayDeathAnim() {
        deathAnim.Play("Close");
    }

    public void ClosePanel() {
        Movement.control = true;
        panel.SetActive(false);
    }
    
    public void Menu() => SceneManager.LoadScene(0);
}
