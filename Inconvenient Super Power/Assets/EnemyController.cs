using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyController : MonoBehaviour
{
    public static List<EnemyController> Enemies = new List<EnemyController>();
    public int health = 1;
    public FlightPlan _FlightPlan;
    public Queue<Waypoint> Flight;

    public Vector2 From;
    public Vector2 To;

    public float StartTime;
    public float EndTime;

    public static void DestroyAllEnemies()
    {
        List<EnemyController> newList = new List<EnemyController>(EnemyController.Enemies);
        foreach(EnemyController enemy in newList)
        {
            if (enemy == null) continue;
            enemy.BlowUp();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InitFlightPlan();
    }

    public void InitFlightPlan()
    {
        EnemyController.Enemies.Add(this);
        Flight = new Queue<Waypoint>(_FlightPlan._FlightPlan);
        StartTime = Time.time;
        EndTime = Time.time;
        GetNextMove();
    }

    public void GetNextMove()
    {
        if (Flight.Count == 1)
        {
            EnemyController.Enemies.Remove(this);
            UnityEngine.Object.Destroy(this.gameObject);
            return;
        }
        Waypoint nextWayPoint = Flight.Dequeue();
        From = nextWayPoint.Target.position;
        To = Flight.Peek().Target.position;
        StartTime = EndTime;
        EndTime += Flight.Peek().Duration;

    }

    // Update is called once per frame
    void Update()
    {
        float percent = 1 - ((EndTime - Time.time) / (EndTime - StartTime));
        if (percent >= 1)
        {
            GetNextMove();
            // I think I need to calculate the next transform here to be perfect.
        }
        else
        {
            transform.position = Vector2.Lerp(From, To, percent);
        }
    }

    public void TakeHit(LaserController laser)
    {
        UnityEngine.Object.Destroy(laser.gameObject);
        health -= laser.damage;
        if (health <= 0)
        {
            BlowUp();
        }
    }

    public void BlowUp()
    {
        EnemyController.Enemies.Remove(this);
        if (this.gameObject != null)
        {
            GameController.Instance.kills++;
            EnemySpawnerController.Instance.EnemyExplosion.Play();
            ExplosionController explosion = UnityEngine.Object.Instantiate<ExplosionController>(GameController.Instance.ExplosionTemplate);
            explosion.transform.position = this.transform.position;
            explosion.gameObject.SetActive(true);
            UnityEngine.Object.Destroy(this.gameObject);
            


        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {

        LaserController laser = other.GetComponent<LaserController>();
        if (laser != null)
        {
            TakeHit(laser);
        }
    }
}

[Serializable]
public class Waypoint
{
    public Transform Target;
    public float Duration;
}