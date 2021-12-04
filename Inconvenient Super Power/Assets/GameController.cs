using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController Instance;

    public Canvas GameOverScreen;
    public PlayerController Player;

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

    public void Restart()
    {
        Player.Restart();
        GameOverScreen.gameObject.SetActive(false);
    }
}
