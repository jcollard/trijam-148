using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController Instance;

    public ExplosionController ExplosionTemplate;
    public int kills = 0;

    public AudioSource Music;
    public AudioSource PlayerExplosion;
    public AudioSource BombExplosion;
    public AudioSource PowerUp1;
    public AudioSource PowerUp2;
    public AudioSource PowerUp3;

    public int TimeBonus;
    public float StartAt;

    public float PowerChangeTime = 5;
    public float LastChange;
    public int LastPower = 0;

    public float CurrentTime;
    public Canvas Overlay;
    public Canvas GameOverScreen;
    public Canvas TitleScreen;
    public PlayerController Player;

    public RectTransform PowerTimer;
    public UnityEngine.UI.Text CurrentPower;
    public UnityEngine.UI.Text ShipsDestroyed;

    // Start is called before the first frame update
    void Start()
    {
        StartAt = Time.time;
        GameOverScreen.gameObject.SetActive(false);
        Overlay.gameObject.SetActive(false);
        TitleScreen.gameObject.SetActive(true);
        if (GameController.Instance != null)
        {
            throw new System.Exception("GameController should be a singleton object but more than one were found.");
        }
        GameController.Instance = this;

    }

    public void Update()
    {
        if ((LastChange + PowerChangeTime) < Time.time && Player.lives > 0)
        {
            ChangePower();
            LastChange = Time.time;
        }
        if (Player.lives > 0)
        {
            TimeBonus = (int)(Time.time - StartAt);
        }
        CurrentTime = Time.time;
        int TimeLeft = (int)(PowerChangeTime + (LastChange - Time.time));
        
        ShipsDestroyed.text = $"{kills*100 + TimeBonus*10}".PadLeft(8, '0');
        // Debug.Log($"Min: {PowerTimer.offsetMin}, Max: {PowerTimer.offsetMax}.");
        float percent = ((LastChange + PowerChangeTime) - Time.time)/PowerChangeTime;
        PowerTimer.sizeDelta = new Vector2(percent * 300, 28);
        // PowerTimer.offsetMin = new Vector2(0, -6);
        // PowerTimer.offsetMax = new Vector2(200, 28);
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
            CurrentPower.text = "Laser";
            PowerUp1.Play();
        }
        else if (choice == 1)
        {
            Player.canBomb = false;
            Player.canFire = false;
            Player.SpawnShield();
            CurrentPower.text = "Shield";
            PowerUp2.Play();
        }
        else if (choice == 2)
        {
            Player.canFire = false;
            Player.canBomb = true;
            Player.DisableShield();
            CurrentPower.text = "Bomb";
            PowerUp3.Play();
        }
    }

    public void Restart()
    {
        if (!Music.isPlaying)
            Music.Play();
        EnemyController.DestroyAllEnemies();
        Player.Restart();
        Overlay.gameObject.SetActive(true);
        GameOverScreen.gameObject.SetActive(false);
        TitleScreen.gameObject.SetActive(false);
        kills = 0;
        LastPower = 0;
        StartAt = Time.time;
        TimeBonus = 0;
        LastChange = Time.time;
        Player.canBomb = false;
        Player.canFire = true;
        Player.DisableShield();
        CurrentPower.text = "Laser";
    }
}
