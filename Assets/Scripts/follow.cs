using UnityEngine;

public class follow : MonoBehaviour
{
    public GameObject _camera;
    public GameObject player;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        _camera = GameObject.FindGameObjectWithTag("MainCamera");
        //3 -10
        //2 -4
        offset = new Vector3(0f, 3f, -10f);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        _camera.transform.position = player.transform.position + offset;
    }
}

