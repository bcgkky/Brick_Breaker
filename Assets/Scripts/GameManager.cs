using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int level = 1;
    public int score = 0;
    public int lives = 3;
    public Brick[] bricks { get; private set; }

    public Ball ball { get; private set; }
    public Paddle paddle { get; private set; }


    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        this.ball = FindObjectOfType<Ball>();
        this.paddle = FindObjectOfType<Paddle>();
        this.bricks = FindObjectsOfType<Brick>();
    }

    void Start()
    {
        NewGame();
    }
    void NewGame()
    {
        this.score = 0;
        this.lives = 3;
        LoadLevel(1);
    }
    void LoadLevel(int level)
    {
        this.level = level;
        SceneManager.LoadScene("Level" + level);
    }

    public void Hit(Brick brick)
    {
        this.score += brick.points;
        if (Cleared())
        {
            LoadLevel(this.level + 1);
        }
    }

    bool Cleared()
    {
        for (int i = 0; i < this.bricks.Length; i++)
        {
            if (this.bricks[i].gameObject.activeInHierarchy && !this.bricks[i].unbreakable)
            {
                return false;
            }
        }
        return true;
    }

    public void Miss()
    {
        this.lives--;

        if (this.lives > 0)
        {
            ResetLevel();
        }
        else { GameOver(); }
    }

    void ResetLevel()
    {
        this.ball.ResetBall();
        this.paddle.ResetPaddle();

        for (int i = 0; i < this.bricks.Length; i++)
        {
            this.bricks[i].ResetBrick();
        }
    }

    void GameOver()
    {
        NewGame();
    }
}
