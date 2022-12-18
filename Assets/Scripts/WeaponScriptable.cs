using UnityEngine;

[CreateAssetMenu(fileName = "WeaponScriptable", menuName = "Objects/Weapon", order = 1)]
public class WeaponScriptable : ScriptableObject
{
    public string prefabName;
    public int id;
    public string weaponName;
    public int damage;
    public float attackAnimMultiplier;
}