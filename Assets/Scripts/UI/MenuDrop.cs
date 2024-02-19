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
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            panel.SetActive(true);
            selectionButton.Select();
        }
    }

    public void ClosePanel() {
        Movement.control = true;
        Time.timeScale = 1;
        panel.SetActive(false);
    }
    
    public void Menu() {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
