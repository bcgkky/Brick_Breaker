using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Rigidbody2D rb { get; private set; }
    public float speed = 500f;

    void Awake()
    {
        this.rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        ResetBall();

    }

    void SetRandomTrajectory()
    {
        Vector2 force = Vector2.zero;
        force.x = Random.Range(-1f , 1f);
        force.y = -1f;

        this.rb.AddForce(force.normalized*speed);

    }

    public void ResetBall()
    {
        this.transform.position = Vector2.zero;
        this.rb.velocity = Vector2.zero;
        Invoke(nameof(SetRandomTrajectory), 1f);
    }

}
