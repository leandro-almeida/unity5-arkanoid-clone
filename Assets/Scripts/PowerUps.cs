using UnityEngine;
using System.Collections;

public class PowerUps : MonoBehaviour
{

    public float velocidade;
    private Rigidbody2D rb;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = -Vector2.up * velocidade;
    }

}
