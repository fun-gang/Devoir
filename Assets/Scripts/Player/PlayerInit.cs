using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class PlayerInit : MonoBehaviour
{
    private Gameplay controls = null;

    // UI and Camera Controll
    private MenuDrop menuDrop;
    private CinemachineVirtualCamera cvm;


    // Device
    [HideInInspector] public static string currentDevice = "Keyboard";
    private PlayerInput plInpt = null;
    
    [Header ("Weapons")]
    public Gun gun;
    public Sword sword;
    private Health health;

    void Awake() {
        controls = new Gameplay();
        
        plInpt = gameObject.GetComponent<PlayerInput>();
        menuDrop = GameObject.FindGameObjectWithTag("MenuDrop").GetComponent<MenuDrop>();
        cvm = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
        cvm.Follow = gameObject.transform;

        health = GetComponent<Health>();
    }

    void Update() {
        currentDevice = plInpt.currentControlScheme;
    }

    private void OnEnable() {
        controls.Enable();

        controls.Player.Exit.performed += menuDrop.OpenPanel;
        controls.Player.Fire.performed += gun.Fire;
        controls.Player.Sword.performed += sword.Attack;
        
        controls.Player.Heal.performed += health.RecoverHP;
    }

    private void OnDisable() {
        controls.Disable();

        controls.Player.Exit.performed -= menuDrop.OpenPanel;
        controls.Player.Fire.performed -= gun.Fire;
        controls.Player.Sword.performed -= sword.Attack;
    }
}
