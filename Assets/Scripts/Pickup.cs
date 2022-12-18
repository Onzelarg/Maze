using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public WeaponScriptable weapon;
    GameObject pick;
    Transform parent;

    void Start()
    {
        parent = GameObject.FindGameObjectWithTag("RightHand").transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name=="PlayerModel")
        {
            Destroy(GameObject.Find("Weapon"));
            pick = Instantiate(Resources.Load(weapon.prefabName) as GameObject);
            pick.name = "Weapon";
            pick.transform.parent = parent;
            pick.transform.position = parent.position;
            pick.GetComponent<Animator>().Play(weapon.weaponName);
            pick.AddComponent<Weapon>();
            pick.GetComponent<Weapon>().damage = weapon.damage;
            GameObject.FindGameObjectWithTag("Playermodel").GetComponent<WeaponAnimation>().weapon = pick;
            GameObject.FindGameObjectWithTag("Playermodel").GetComponent<Animator>().SetFloat("attackMultiplier", 1 * weapon.attackAnimMultiplier);
        }
    }
}
