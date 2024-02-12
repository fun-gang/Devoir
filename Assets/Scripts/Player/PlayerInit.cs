using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class PlayerInit : MonoBehaviour
{
    // Movement
    private Gameplay controls = null;
    [HideInInspector] public Vector2 movement = Vector2.zero;
    [HideInInspector] public float jumpPressTime = float.NegativeInfinity;
    [HideInInspector] public float jumpStartTime = float.NegativeInfinity;


    // UI and Camera Controll
    private MenuDrop menuDrop;
    private CinemachineVirtualCamera cvm;


    // Device
    [HideInInspector] public static string currentDevice = "Keyboard";
    private PlayerInput plInpt = null;
    
    [Header ("Weapons")]
    public Gun gun;
    public Sword sword;
    public Interactive inter;
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
        
        if (controls.Player.Block.ReadValue<float>() >= InputSystem.settings.defaultButtonPressPoint) {
            if (sword.isReady) sword.isBlock = true;
        }
        else sword.isBlock = false;
    }

    void OnMovePerformed(InputAction.CallbackContext value) => movement = value.ReadValue<Vector2>();
    void OnMoveCanceled(InputAction.CallbackContext value) => movement = Vector2.zero;

    void PressJump (InputAction.CallbackContext value) => jumpPressTime = Time.time;
    void ReleaseJump (InputAction.CallbackContext value) => jumpPressTime = jumpStartTime = float.NegativeInfinity;

    private void OnEnable() {
        controls.Enable();
        controls.Player.Move.performed += OnMovePerformed;
        controls.Player.Move.canceled += OnMoveCanceled;
        controls.Player.Jump.performed += PressJump;
        controls.Player.Jump.canceled += ReleaseJump;
        controls.Player.Exit.performed += menuDrop.OpenPanel;
        controls.Player.Fire.performed += gun.Fire;
        controls.Player.Sword.performed += sword.Attack;
        
        controls.Player.Act.performed += inter.Act;
        controls.Player.Put.performed += inter.Put;
        controls.Player.Heal.performed += health.RecoverHP;
    }

    private void OnDisable() {
        controls.Disable();
        controls.Player.Move.performed -= OnMovePerformed;
        controls.Player.Move.canceled -= OnMoveCanceled;
        controls.Player.Jump.performed -= PressJump;
        controls.Player.Jump.canceled -= ReleaseJump;
        controls.Player.Exit.performed -= menuDrop.OpenPanel;
        controls.Player.Fire.performed -= gun.Fire;
        controls.Player.Sword.performed -= sword.Attack;
        
        controls.Player.Act.performed -= inter.Act;
        controls.Player.Put.performed -= inter.Put;
    }
}
