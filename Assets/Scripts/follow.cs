using UnityEngine;

public class follow : MonoBehaviour
{
    public GameObject play_camera;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        play_camera = GameObject.FindGameObjectWithTag("PlayerFollowCamera");

    }

    private void LateUpdate()
    {
        play_camera.transform.position = player.transform.position;
    }
 
}

