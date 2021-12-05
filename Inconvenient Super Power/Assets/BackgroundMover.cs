using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMover : MonoBehaviour
{
    public float speedX;

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(new Vector2(0, speedX * Time.deltaTime));
        if (this.transform.position.y < -10)
        {
            UnityEngine.Object.Destroy(this.gameObject);
        }
    }
}
