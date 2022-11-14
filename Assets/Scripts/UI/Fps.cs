using UnityEngine;
using TMPro;

public class Fps : MonoBehaviour
{
    public TextMeshProUGUI fps_text;
    public float update_time;
    public float time;
    public int frame_count;
    public MazeGenerator maze;

    void Start()
    {
        update_time = 1f;
    }
    void Update()
    {
        time += Time.deltaTime;
        frame_count++;

        if (time>=update_time)
        {
            fps_text.text = Mathf.RoundToInt(frame_count / time) + " FPS";
            time -= update_time;
            frame_count = 0;
        }
    }

    public void gen()
    {
        Debug.Log("click1");
        maze.generateRooms(1);
    }

    public void genNoRoom()
    {
        Debug.Log("click2");
        maze.generateMazeNoRooms();

    }
}
 