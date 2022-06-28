using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateUnitSprite : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        Vector3 targetVector = Camera.main.transform.position - transform.position;
        float newYAngle = Mathf.Atan2(targetVector.x, targetVector.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, -1 * newYAngle, 0);
    }
}
