using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{

    public float speedX = 0;
    public float speedY = 10;
    public int damage = 1;

    // Update is called once per frame
    public virtual void Update()
    {
        float newX = this.transform.position.x + (speedX * Time.deltaTime);
        float newY = this.transform.position.y + (speedY * Time.deltaTime);
        transform.position = new Vector2(newX, newY);

        CleanUp();
    }

    public void CleanUp()
    {
        if (this.transform.position.y > 10 ||
            this.transform.position.y < -10 ||
            this.transform.position.x > 10 ||
            this.transform.position.x < -10)
        {
            UnityEngine.Object.Destroy(this.gameObject);
        }
    }
}
