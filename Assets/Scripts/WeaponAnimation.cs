using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimation : MonoBehaviour
{
    public GameObject weapon;

    void enableCollision()
    {
        if (weapon != null)
        {
            if (weapon.TryGetComponent<MeshCollider>(out MeshCollider mesh))
            {
                mesh.enabled = true;
            }
            if (weapon.TryGetComponent<BoxCollider>(out BoxCollider box))
            {
                box.enabled = true;
            }
        }
    }

    void disableCollision()
    {
        if (weapon != null)
        {
            if (weapon.TryGetComponent<MeshCollider>(out MeshCollider mesh))
            {
                mesh.enabled = false;
            }
            if (weapon.TryGetComponent<BoxCollider>(out BoxCollider box))
            {
                box.enabled = false;
            }
        }
    }

}
