using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundGenerator : MonoBehaviour
{
    public BackgroundMover StarRef;
    public Transform StarContainer;
    public int MaxStars = 500;
    public float MaxSpeed = -1;
    public float MinSpeed = -5;
    public float MinX = -3;
    public float MaxX = 3;
    public float MinY = 6;
    public float MaxY = 7;
    public float MinScale = 0.1F;
    public float MaxScale = 0.5F;
    public float SpawnRate;
    public float LastSpawn;

    // Update is called once per frame
    void Update()
    {
        if ((LastSpawn + SpawnRate) < Time.time)
        {
            Spawn();
            LastSpawn = Time.time;
        }
    }

    public void Spawn()
    {
        for (int i = 0; i < 20; i++)
        {
            if (StarContainer.childCount >= MaxStars)
            {
                return;
            }
            BackgroundMover newMover = UnityEngine.Object.Instantiate<BackgroundMover>(StarRef);
            float newX = Random.Range(MinX, MaxX);
            float newY = Random.Range(MinY, MaxY);
            
            Vector2 newPosition = new Vector2(newX, newY);
            newMover.transform.position = newPosition;
            newMover.transform.parent = StarContainer;
            newMover.speedX = Random.Range(MinSpeed, MaxSpeed);
            float scale = Random.Range(MinScale, MaxScale);
            newMover.transform.localScale = new Vector3(scale, scale, scale);
            newMover.gameObject.SetActive(true);
        }
    }
}
