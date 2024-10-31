using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] Transform Player;
    [SerializeField] Transform PatrolRoute;
    [SerializeField] List<Transform> Locations;
    private int _LocationIndex = 0;
    private NavMeshAgent Agent;
    [SerializeField] GameObject Bullet;
    private float BulletSpeed= 30f;
    private bool IsShooting=true;
    [SerializeField] int lives = 3;
    public Button WinButton;
    [SerializeField] Slider slider;
    [SerializeField] Image Fill;
    [SerializeField] Gradient EnemyColor;
    private int time=0;


    public int EnemyLives
    {
        get { return lives; }
        set
        {
            lives = value;
            slider.value = EnemyLives;
            Fill.color= EnemyColor.Evaluate(slider.normalizedValue);
            if (lives <= 0)
            {
                int NumOfEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
                Debug.Log(NumOfEnemies);
                Destroy(this.gameObject);
                Debug.Log("The enemy has been vanquished");
                if (NumOfEnemies <=1)
                {
                 WinButton.gameObject.SetActive(true);
                Time.timeScale = 0f;

                }
                

            }
        }
    }

    
    private void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        InitializePatrolRoute();
        MoveToNextPatrolLocation();
        Player = GameObject.Find("Player").transform;
        Fill.color = new Color(0.2149121f, 0.8018868f, 0.02017319f);

    }
     void Update()
    {

        if (Agent.remainingDistance < 0.2f && !Agent.pathPending)
        {
            MoveToNextPatrolLocation();
        }
        time += 1;
        if(time >= 15)
        {
            time = 0;
        }
    }
    private void InitializePatrolRoute()
    {
        foreach (Transform child in PatrolRoute)
        {
            Locations.Add(child);
        }
    }
     void  MoveToNextPatrolLocation()
    {
        if (Locations.Count == 0)
            return;

        Agent.destination += Locations[_LocationIndex].position;
        _LocationIndex = (_LocationIndex + 1) % Locations.Count;
    }
    private void EnemyRotation()
    {
        Vector3 lookat = Player.position - this.transform.position;
        lookat.y = 0f;
        Quaternion rotation = Quaternion.LookRotation(lookat);
    }

    public void OnTriggerStay(Collider other)
    {
        IsShooting = true;
        if (other.name == "Player")
        {
            Agent.destination = Player.position;
            
            EnemyRotation();
            this.transform.rotation=new Quaternion(Player.rotation.x,Player.rotation.y,Player.rotation.z,0);
          if (IsShooting && time>=14)
            {
                 GameObject BulletBB = Instantiate(Bullet, this.transform.position + new Vector3(0, 0, 1), Player.rotation);
                  Rigidbody newBullet = BulletBB.GetComponent<Rigidbody>();
                 newBullet.velocity = Vector3.forward * BulletSpeed;
                IsShooting = false;
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            Debug.Log("Player out of range, resume patrol");
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player Bullet(Clone)")
        {
            EnemyLives -= 1;
            Debug.Log("Enemy Hit");
        }
    }

}
