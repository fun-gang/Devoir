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
    
    [Header ("Components")]
    public Gun gun;
    public Sword sword;
    private Health health;

    void Awake() {
        controls = new Gameplay();
        
        plInpt = GetComponent<PlayerInput>();
        menuDrop = GameObject.FindGameObjectWithTag("MenuDrop").GetComponent<MenuDrop>();
        cvm = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
        cvm.Follow = transform;
        health = GetComponent<Health>();
    }

    void Update() {
        currentDevice = plInpt.currentControlScheme;
    }

    private void OnEnable() {
        controls.Enable();

        controls.Player.Exit.performed += menuDrop.OpenPanel;
        
        controls.Player.Sword.performed += sword.Attack;
        controls.Player.Fire.performed += gun.Fire;
        controls.Player.Block.performed += sword.Block;
        controls.Player.Heal.performed += health.RecoverHP;
    }

    private void OnDisable() {
        controls.Disable();
    }
}
