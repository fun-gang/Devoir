using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player Data", order = 51)]
public class PlayerData : ScriptableObject
{
    [Tooltip("+%")] [SerializeField] private float swordDamageMod = 0f;
    [Tooltip("+%")] [SerializeField] private float gunDamageMod = 0f;
    [Tooltip("-%")] [SerializeField] private float timeBtwShotsMod = 0f;
    [Tooltip("-%")] [SerializeField] private float reloadTimeMod = 0f;
    [Tooltip("+ bullets (int)")] [SerializeField] private int countOfBulletsMod = 0;
    [Tooltip("+ HP cells (int)")] [SerializeField] private int maxHealthMod = 0;
    [Tooltip("How much increased damage after COOL block of enemy attack (+%)")] [SerializeField] private float swordDamageBlockMod = 0f;
    [Tooltip("-%")] [SerializeField] private float comboTimeMod = 0f;
    [Tooltip("-%")] [SerializeField] private float comboCursorSpeedMod = 0f;

    public float SwordDamageMod { get => swordDamageMod; set => swordDamageMod = value; }
    public float GunDamageMod { get => gunDamageMod; set => gunDamageMod = value; }
    public float TimeBtwShotsMod { get => timeBtwShotsMod; set => timeBtwShotsMod = value; }
    public float ReloadTimeMod { get => reloadTimeMod; set => reloadTimeMod = value; }
    public int CountOfBulletsMod { get => countOfBulletsMod; set => countOfBulletsMod = value; }
    public int MaxHealthMod { get => maxHealthMod; set => maxHealthMod = value; }
    public float SwordDamageBlockMod {get => swordDamageBlockMod; set => swordDamageBlockMod = value; }
    public float ComboTimeMod {get => comboTimeMod; set => comboTimeMod = value; }
    public float ComboCursorSpeedMod {get => comboCursorSpeedMod; set => comboCursorSpeedMod = value; }
}
