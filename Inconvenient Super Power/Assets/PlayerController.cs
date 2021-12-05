using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{

    public SpriteRenderer sprite;
    private Dictionary<string, Action> MovementControls;
    private Dictionary<string, Action> FireControls;

    public LaserController LaserRef;
    public ShieldController ShieldRef;
    public ShieldController LastShield;

    public bool canFire = true;
    public bool canBomb = false;

    public Vector2 Min;
    public Vector2 Max;
    public Vector2 NextMove;

    private Vector2 StartingPosition;

    public float InvulnerabilityDuration = 3;
    public float Invulnerable = -1;
    public float Speed = 7;

    public float RespawnAt = -1;
    public float RespawnDelay = 3;

    public int startingLives = 3;
    public int lives = 0;
    // Start is called before the first frame update
    void Start()
    {
        MovementControls = new Dictionary<string, Action>();
        MovementControls["Left"] = () => CalculateMove(-1, 0);
        MovementControls["Right"] = () => CalculateMove(1, 0);
        MovementControls["Up"] = () => CalculateMove(0, 1);
        MovementControls["Down"] = () => CalculateMove(0, -1);

        FireControls = new Dictionary<string, Action>();
        FireControls["Fire"] = () => Fire();

        StartingPosition = new Vector2(0, Min.y);
        this.transform.position = new Vector2(-500, -500);
        RespawnAt = 0;
        //Restart();
    }

    // Update is called once per frame
    void Update()
    {
        NextMove = new Vector2();

        if (RespawnAt < 0)
        {
            foreach (string key in MovementControls.Keys)
            {
                if (Input.GetButton(key))
                {
                    MovementControls[key]();
                }
            }

            foreach (string key in FireControls.Keys)
            {
                if (Input.GetButtonDown(key))
                {
                    FireControls[key]();
                }
            }
        }

        if (Invulnerable > 0)
        {
            float alpha = Mathf.Abs(Mathf.Sin(Time.time*20));
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
        }
        else 
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1);
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
        if (Invulnerable < Time.time)
        {
            Invulnerable = -1;
        }

    }

    public void BlowUp()
    {
        ExplosionController explosion = UnityEngine.Object.Instantiate<ExplosionController>(GameController.Instance.ExplosionTemplate);
        explosion.transform.position = this.transform.position;
        explosion.gameObject.SetActive(true);
        GameController.Instance.PlayerExplosion.Play();
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
        Invulnerable = Time.time + InvulnerabilityDuration;
        this.transform.position = StartingPosition;
        RespawnAt = -1;
    }

    public void Restart()
    {
        this.lives = startingLives;
        Respawn();
    }

    public void Fire()
    {
        if (this.canFire)
        {
            LaserController newLaser = UnityEngine.Object.Instantiate<LaserController>(LaserRef);
            newLaser.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 0.5f);
            newLaser.gameObject.SetActive(true);
        }
        if (this.canBomb)
        {
            EnemyController.DestroyAllEnemies();
            this.canBomb = false;
            GameController.Instance.BombExplosion.Play();
            GameController.Instance.CurrentPower.text = "None";
        }
    }

    public void DisableShield()
    {
        if (LastShield != null)
        {
            UnityEngine.Object.Destroy(LastShield.gameObject);
        }
    }

    public void SpawnShield()
    {
        DisableShield();
        ShieldController newShield = UnityEngine.Object.Instantiate<ShieldController>(ShieldRef);
        newShield.transform.parent = this.transform;
        newShield.transform.localPosition = new Vector3();
        newShield.gameObject.SetActive(true);
        LastShield = newShield;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        EnemyController enemy = other.GetComponent<EnemyController>();
        if (enemy != null && Invulnerable < 0)
        {
            BlowUp();
        }
    }
}
