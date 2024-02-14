using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player Data", order = 51)]
public class PlayerData : ScriptableObject
{
    [SerializeField] private float swordDamage = 0.2f;
    [SerializeField] private float gunDamage = 0.1f;
    [SerializeField] private float timeBtwShots = 0.1f;
    [SerializeField] private float reloadTime = 0.5f;
    [SerializeField] private int countOfBullets = 2;
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private float maxEnergy = 1f;
    [SerializeField] private float energyRecoveringSpeed = 0.25f;
    [SerializeField] private float gunFireCost = 0.01f;

    public float SwordDamage { get => swordDamage; set => swordDamage = value; }
    public float GunDamage { get => gunDamage; set => gunDamage = value; }
    public float TimeBtwShots { get => timeBtwShots; set => timeBtwShots = value; }
    public float ReloadTime { get => reloadTime; set => reloadTime = value; }
    public int CountOfBullets { get => countOfBullets; set => countOfBullets = value; }
    public int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public float MaxEnergy { get => maxEnergy; set => maxEnergy = value; }
    public float EnergyRecoveringSpeed { get => energyRecoveringSpeed; set => energyRecoveringSpeed = value; }
    public float GunFireCost { get => gunFireCost; set => gunFireCost = value; }
}
