using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Transform Cam;
    private void LateUpdate()
    {
        transform.LookAt(transform.position+Cam.forward);
    }


}
