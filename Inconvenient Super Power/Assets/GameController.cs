using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController Instance;
    public int kills = 0;

    public AudioSource PlayerExplosion;
    public AudioSource BombExplosion;
    public AudioSource PowerUp1;
    public AudioSource PowerUp2;
    public AudioSource PowerUp3;

    public float PowerChangeTime = 5;
    public float LastChange;
    public int LastPower = 0;

    public float CurrentTime;
    public Canvas GameOverScreen;
    public PlayerController Player;

    public UnityEngine.UI.Text PowerTimer;
    public UnityEngine.UI.Text CurrentPower;
    public UnityEngine.UI.Text ShipsDestroyed;

    // Start is called before the first frame update
    void Start()
    {
        GameOverScreen.gameObject.SetActive(false);
        if (GameController.Instance != null)
        {
            throw new System.Exception("GameController should be a singleton object but more than one were found.");
        }
        GameController.Instance = this;

    }

    public void Update()
    {
        if ((LastChange + PowerChangeTime) < Time.time)
        {
            ChangePower();
            LastChange = Time.time;
        }
        CurrentTime = Time.time;
        int TimeLeft = (int)(PowerChangeTime + (LastChange - Time.time));
        PowerTimer.text = $"Power Reset: {TimeLeft}s";
        ShipsDestroyed.text = $"Ships Destroyed: {kills}s";
    }

    public void ChangePower()
    {
        int choice = LastPower;
        while (choice == LastPower)
        {
            choice = Random.Range(0, 3);
        }
        LastPower = choice;
        if (choice == 0)
        {
            Player.canBomb = false;
            Player.canFire = true;
            Player.DisableShield();
            CurrentPower.text = "Power: Laser";
            PowerUp1.Play();
        }
        else if (choice == 1)
        {
            Player.canBomb = false;
            Player.canFire = false;
            Player.SpawnShield();
            CurrentPower.text = "Power: Shield";
            PowerUp2.Play();
        }
        else if (choice == 2)
        {
            Player.canFire = false;
            Player.canBomb = true;
            Player.DisableShield();
            CurrentPower.text = "Power: Bomb";
            PowerUp3.Play();
        }
    }

    public void Restart()
    {
        Player.Restart();
        GameOverScreen.gameObject.SetActive(false);
        kills = 0;
        LastPower = 0;
        CurrentPower.text = "Power: Laser";
    }
}
