using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float timeInDay = 5f;

    public Light sun;
    public Light moon;
    public float timeOfDay;

    void Start()
    {
    }

    void Update()
    {
        timeOfDay += Time.deltaTime;
        if(timeOfDay >= timeInDay) {
            timeOfDay = 0f;
        }

        sun.transform.rotation = Quaternion.AngleAxis((1f - timeOfDay/timeInDay) * 360f, Vector3.forward) * Quaternion.FromToRotation(Vector3.forward, Vector3.right);
        moon.transform.rotation = Quaternion.AngleAxis((1f - timeOfDay/timeInDay) * 360f, Vector3.forward) * Quaternion.FromToRotation(Vector3.forward, Vector3.left);
        
    }
}
