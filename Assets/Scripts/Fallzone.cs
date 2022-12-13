using UnityEngine;

public class Fallzone : MonoBehaviour
{
    Material mat;
    void Start()
    {
        mat = this.gameObject.GetComponent<Renderer>().material= Instantiate(Resources.Load("Ball_cube") as Material);
        mat.SetFloat("_Alpha", 1);
    }
    void Update()
    {
        GameObject ball = this.gameObject;
        if (ball.transform.position.y<-50)
        {
            Destroy(ball);
        }
        

    }
}
