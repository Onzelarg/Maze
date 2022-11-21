using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fallzone : MonoBehaviour
{
    void Update()
    {
        GameObject ball = this.gameObject;
        if (ball.transform.position.y<-50)
        {
            UnityEngine.Object.Destroy(ball);
        }
    }
}
