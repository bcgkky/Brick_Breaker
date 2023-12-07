using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
   public int healt { get; private set; }
    public SpriteRenderer spriterenderer { get; private set; }
    public Sprite[] states;
    public bool unbreakable;
    public int points = 100;

    void Awake()
    {
        this.spriterenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        ResetBrick();
    }

    void Hit()
    {
        if (this.unbreakable)
        {
            return;
        }
        this.healt--;
        if (this.healt <= 0)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.spriterenderer.sprite = this.states[this.healt - 1];
        }
        FindObjectOfType<GameManager>().Hit(this);
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Ball") { Hit(); }
    }

    public void ResetBrick()
    {
        this.gameObject.SetActive(true);

        if (!this.unbreakable)
        {
            this.healt = this.states.Length;
            this.spriterenderer.sprite = this.states[this.healt - 1];
        }
    }
}
