using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] float Onscreendelay = 1f;

    void Start()
    {
        Destroy(this.gameObject, Onscreendelay);
    }

   
}
