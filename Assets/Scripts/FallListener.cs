using UnityEngine;
using System.Collections;

public class FallListener : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        // colidiu um powerup
        if (other.gameObject.tag.StartsWith("powerup"))
        {
            Debug.Log("powerup destroyed");
            Destroy(other.gameObject);
        }
        // colidiu uma bola
        else if (other.gameObject.GetComponent<Ball>() != null)
        {
            Debug.Log("ball fall");
            other.gameObject.GetComponent<Ball>().HandleBallFall();
        }
    }


}
