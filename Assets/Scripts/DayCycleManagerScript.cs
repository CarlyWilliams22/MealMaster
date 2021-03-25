using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//derived from https://www.youtube.com/watch?v=h5GFoI38DOg&t=1313s, glenrhodes.com
public class DayCycleManagerScript : MonoBehaviour
{
    float dayLength = 100;
    float maxIntensity = 3f;
    float minIntensity = 0f;
    float minPoint = -.2f;
    public Vector3 dayRotateSpeed;

    public Gradient sunlightColor;
    public Light sun;


    // Start is called before the first frame update
    void Start()
    {
        Invoke("EndDay", dayLength);
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
        SceneManager.LoadScene("EndOfDayScene");
    }
}
