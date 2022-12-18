using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class EnemyUI : MonoBehaviour
{
    public GameObject damageText;
    List<GameObject> popups;
    public GameObject parent;
    public GameObject popupParent;
    public float yOffset=20;
    public Vector3 offset;
    Camera mainCam;
    GameObject player;

    public static EnemyUI instance;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mainCam = Camera.main;
        popups = new List<GameObject>();
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
     


    public void damageIndicator(GameObject gameObject,int amount)
    {
        Vector3 boundY = gameObject.GetComponentInChildren<Collider>().bounds.size;
        Vector3 goPosition = gameObject.transform.position;
        Vector3 position = new Vector3(goPosition.x, (goPosition.y + boundY.y), goPosition.z);
        popups.Add(Instantiate(damageText, position, mainCam.transform.rotation, popupParent.transform));
        popups[popups.Count - 1].transform.name = "Damage 4 " + gameObject.transform.name;
        popups[popups.Count - 1].GetComponent<TextMeshPro>().text = amount.ToString();
    }
}
