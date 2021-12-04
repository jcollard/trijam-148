using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{

    private Dictionary<string, Action> MovementControls;

    public Vector2 Min;
    public Vector2 Max;
    public Vector2 NextMove;

    private Vector2 StartingPosition;
    public float Speed = 7;

    public float RespawnAt = -1;
    public float RespawnDelay = 3;

    public int startingLives = 3;
    public int lives = 3;
    // Start is called before the first frame update
    void Start()
    {
        MovementControls = new Dictionary<string, Action>();
        MovementControls["Left"] = () => CalculateMove(-1, 0);
        MovementControls["Right"] = () => CalculateMove(1, 0);
        MovementControls["Up"] = () => CalculateMove(0, 1);
        MovementControls["Down"] = () => CalculateMove(0, -1);
        StartingPosition = new Vector2(0, Min.y);
        Restart();
    }

    // Update is called once per frame
    void Update()
    {
        NextMove = new Vector2();

        foreach (string key in MovementControls.Keys)
        {
            if (Input.GetButton(key) && RespawnAt < 0)
            {
                MovementControls[key]();
            }
        }

        DoMove();
        CleanUp();
    }

    public void CalculateMove(float x, float y)
    {
        NextMove.x += x;
        NextMove.y += y;
    }

    public void DoMove()
    {
        this.transform.Translate(NextMove * Speed * Time.deltaTime);
    }

    public void CleanUp()
    {
        if (RespawnAt > 0 && RespawnAt < Time.time)
        {
            Respawn();
        }
        else if (RespawnAt < 0)
        {
            Vector2 position = this.transform.position;
            float newX = Mathf.Clamp(position.x, Min.x, Max.x);
            float newY = Mathf.Clamp(position.y, Min.y, Max.y);
            this.transform.position = new Vector2(newX, newY);
        }

    }

    public void BlowUp()
    {
        RespawnAt = Time.time + RespawnDelay;
        this.transform.position = new Vector2(-500, 500);
        lives--;
        if (lives <= 0)
        {
            RespawnAt = float.PositiveInfinity;
            GameController.Instance.GameOverScreen.gameObject.SetActive(true);
        }
    }

    public void Respawn()
    {
        this.transform.position = StartingPosition;
        RespawnAt = -1;
    }

    public void Restart()
    {
        this.lives = startingLives;
        Respawn();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null)
        {
            BlowUp();
        }
    }
}
