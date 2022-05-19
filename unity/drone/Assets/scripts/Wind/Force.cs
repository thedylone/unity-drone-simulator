using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Force : MonoBehaviour
{
    public GameObject windObject;
    Rigidbody body;
    int windLevel;
    void Start () {
        body = gameObject.GetComponent<Rigidbody> ();
        windLevel = PlayerPrefs.GetInt("wind level");
    }

    void FixedUpdate () {
    StartCoroutine(activateRandomWind());
    // GetComponent<Rigidbody>().AddRelativeForce(Random.onUnitSphere * 20f);
    }
    IEnumerator activateRandomWind()
    {   
        float randomChance = UnityEngine.Random.Range(1,5);
        if(randomChance == 1)
        {
            float minWait = 1f;
            float maxWait = 5f;
            float rangeWait = maxWait - minWait;
            float randomWait = UnityEngine.Random.Range(minWait,maxWait);
            
            yield return new WaitForSeconds(randomWait);

            // float minSpeed = 10f;
            // float maxSpeed = 30f;
            // float rangeSpeed = maxSpeed - minSpeed;
            // float randomSpeed = UnityEngine.Random.Range(minSpeed,maxSpeed);
            // float scaledSpeed = (float) ((randomSpeed * rangeSpeed) + minSpeed);

            GetComponent<Rigidbody>().AddRelativeForce(windLevel * Random.onUnitSphere * 50f);
        }

    }

}
