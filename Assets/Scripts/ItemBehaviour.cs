using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehaviour : MonoBehaviour
{
    public GameBehavior GameManager;
    public AudioSource Collecting;
   
    private void Start()
    {
        GameManager= GameObject.Find("Game Manager").GetComponent<GameBehavior>();
        Collecting=GameObject.Find("Player").GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name=="Player")
        {
            Destroy(this.transform.gameObject);
            Debug.Log("Item Collected");
            GameManager.Items += 1;
            GameManager.PrintLootReport();
            Collecting.Play();
        }
    }
  
}
