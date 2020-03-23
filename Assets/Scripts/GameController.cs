using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float timeInDay = 5f;

    public Color dawn;
    public Color noon;
    public Color dusk;
    public Color midnight;

    Light sunLight;
    public float timeOfDay;

    // Start is called before the first frame update
    void Start()
    {
        sunLight = GameObject.FindGameObjectWithTag("SunLight").GetComponent<Light>();
        timeOfDay = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeOfDay += Time.deltaTime;
        if(timeOfDay >= timeInDay) {
            timeOfDay = 0f;
        }

        Color sky;
        if(timeOfDay < timeInDay/4f) {
            sky = Color.Lerp(dawn, noon, 4f * timeOfDay/timeInDay);
            sunLight.gameObject.transform.rotation = Quaternion.Lerp(Quaternion.AngleAxis(0f, Vector3.right), Quaternion.AngleAxis(90f, Vector3.right), 4f * timeOfDay/timeInDay);
        }
        else if(timeOfDay < timeInDay/2f){
            sky = Color.Lerp(noon, dusk, 4f * timeOfDay/timeInDay - 1f);
            sunLight.gameObject.transform.rotation = Quaternion.Lerp(Quaternion.AngleAxis(90f, Vector3.right), Quaternion.AngleAxis(180f, Vector3.right), 4f * timeOfDay/timeInDay - 1f);
        }
        else if(timeOfDay < 3f * timeInDay/4f) {
            sky = Color.Lerp(dusk, midnight, 4f * timeOfDay/timeInDay - 2f);
            sunLight.gameObject.transform.rotation = Quaternion.Lerp(Quaternion.AngleAxis(180f, Vector3.right), Quaternion.AngleAxis(90f, Vector3.right), 4f * timeOfDay/timeInDay - 2f);
        }
        else {
            sky = Color.Lerp(midnight, dawn, 4f * timeOfDay/timeInDay - 3f);
            sunLight.gameObject.transform.rotation = Quaternion.Lerp(Quaternion.AngleAxis(90f, Vector3.right), Quaternion.AngleAxis(0f, Vector3.right), 4f * timeOfDay/timeInDay - 3f);
        }
        
        sunLight.color = sky;
        
    }
}
