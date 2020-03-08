using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private const float MAX_Y = 4f;
    private const float MIN_Y = -4f;
    private const float MAX_X = 2.17f;
    private const float MIN_X = -2.17f;

    public GameObject back;
    void Start()
    {
        float x = Random.Range(MIN_X, MAX_X);
        float y = Random.Range(MIN_Y, MAX_Y);
        back.transform.position = new Vector3(x, y, 10);
    }

}
