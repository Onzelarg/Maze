using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage;
    string weaponName;

    public void disableCollision()
    {
        //this.GetComponent<MeshCollider>().enabled = false;
    }
    public void enableCollision()
    {
        //this.GetComponent<MeshCollider>().enabled = true;
    }
}
