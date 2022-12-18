using UnityEngine;

public class DestroyPopup : MonoBehaviour
{
    Camera main;

    void Awake()
    {
        main = Camera.main;
    }

    void Update()
    {
        transform.rotation = main.transform.rotation;
    }

    void destroyPopup()
    {
        Destroy(gameObject);
    }
}
