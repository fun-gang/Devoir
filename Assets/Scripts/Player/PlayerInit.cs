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
    private Movement move;

    [Header ("Combo inputs")]
    [Tooltip ("Time we have to do combo attack with other inputs using sword")]
    public float swordComboBufferTime = 0.5f;
    [Tooltip ("Time we have to do combo attack with other inputs using gun")]
    public float gunComboBufferTime = 0.5f;
    [Tooltip ("Time we have to do combo attack with other inputs using jump")]
    public float jumpComboBufferingTime = 0.5f;
    [HideInInspector] public float swordPressTime = float.NegativeInfinity;
    [HideInInspector] public float gunPressTime = float.NegativeInfinity;

    void Awake() {
        controls = new Gameplay();
        
        plInpt = GetComponent<PlayerInput>();
        menuDrop = GameObject.FindGameObjectWithTag("MenuDrop").GetComponent<MenuDrop>();
        cvm = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
        cvm.Follow = transform;
        move = GetComponent<Movement>();
        health = GetComponent<Health>();
    }

    void Update() {
        currentDevice = plInpt.currentControlScheme;
        if (Movement.control) ComboLogic();
    }

    private void ComboLogic() {
        bool isPressedJump = Time.time - move.jumpPressTime < jumpComboBufferingTime;
        bool isPressedGun = Time.time - gunPressTime < gunComboBufferTime;
        bool isPressedSword = Time.time - swordPressTime < swordComboBufferTime;

        if (isPressedJump) {
            if (isPressedSword) sword.CircleAttack();
            else if (isPressedGun) gun.Rotatata();
        }
        else {
            if (isPressedSword) sword.Attack();
            else if (isPressedGun) gun.Fire();
        }
    }

    void PressSword (InputAction.CallbackContext value) => swordPressTime = Time.time;
    void ReleaseSword (InputAction.CallbackContext value) => swordPressTime = float.NegativeInfinity;
    void PressGun (InputAction.CallbackContext value) => gunPressTime = Time.time;
    void ReleaseGun (InputAction.CallbackContext value) => gunPressTime = float.NegativeInfinity;

    private void OnEnable() {
        controls.Enable();

        controls.Player.Exit.performed += menuDrop.OpenPanel;
        
        controls.Player.Sword.performed += PressSword;
        controls.Player.Sword.canceled += ReleaseSword;
        controls.Player.Fire.performed += PressGun;
        controls.Player.Fire.canceled += ReleaseGun;
        
        controls.Player.Heal.performed += health.RecoverHP;
    }

    private void OnDisable() {
        controls.Disable();

        controls.Player.Exit.performed -= menuDrop.OpenPanel;
        
        controls.Player.Sword.performed -= PressSword;
        controls.Player.Sword.canceled -= ReleaseSword;
        controls.Player.Fire.performed -= PressGun;
        controls.Player.Fire.canceled -= ReleaseGun;
    }
}
