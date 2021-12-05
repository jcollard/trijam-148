using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{

    public float Duration = 2F;
    public float StartAt;
    // Start is called before the first frame update
    void Start()
    {
        StartAt = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (StartAt + Duration < Time.time)
        {
            UnityEngine.Object.Destroy(this.gameObject);
        }
    }
}
