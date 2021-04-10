using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts;


//derived from https://www.youtube.com/watch?v=h5GFoI38DOg&t=1313s, glenrhodes.com
public class DayCycleManagerScript : MonoBehaviour
{
    float dayLength = 180;
    int time = 8;
    float maxIntensity = 3f;
    float minIntensity = 0f;
    float minPoint = -.2f;
    public Vector3 dayRotateSpeed;

    public Gradient sunlightColor;
    public Light sun;


    // Start is called before the first frame update
    void Start()
    {
        Prefs.SetLevelProfit(0);
        Invoke("EndDay", dayLength);
        UpdateTimeOfDay();
    }

    // Update is called once per frame
    void Update()
    {
        float tRange = 1 - minPoint;
        float dotproduct = Mathf.Clamp01((Vector3.Dot(sun.transform.forward, Vector3.down) - minPoint) / tRange);
        float intensity = ((maxIntensity - minIntensity) * dotproduct) + minIntensity;

        sun.intensity = intensity;
        sun.color = sunlightColor.Evaluate(dotproduct);
        sun.transform.Rotate(dayRotateSpeed * Time.deltaTime);
    }

    void EndDay()
    {
        if (Prefs.GetLevelProfit() > 0)
        {
            SceneManager.LoadScene("EndOfDayScene");
        }
        else
        {
            Prefs.SetGameInProgress(false);
            SceneManager.LoadScene("GameOverScene");
        }
    }

    public void UpdateTimeOfDay()
    {
        Messenger.Broadcast<int>(GameEvent.CHANGED_TIME_OF_DAY, (time++)%12 +1);
        Invoke("UpdateTimeOfDay", dayLength/12);
    }
}
