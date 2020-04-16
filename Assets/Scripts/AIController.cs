using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    [SerializeField] private float attackRadius = 500f;
    [SerializeField] private float shotCooldown = 4f;
    [SerializeField] private float attackInterval = 5f;
    [SerializeField] private BaseRocket rocket;
    
    private float shotCoolDownCount; 
    private float attackIntervalCount;
    private Player player;

    private Planet planet;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        attackIntervalCount = attackInterval;
        shotCoolDownCount = shotCooldown;
        planet = GetComponent<Planet>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && planet != null && planet.Health > 0)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < attackRadius)
            {
                if (attackIntervalCount > 0)
                {
                    attackIntervalCount -= Time.deltaTime;
                }
                else
                {
                    //The rate of fire is set by the value of ShotCooldown, and is set in frame updates
                    if (shotCoolDownCount > 0)
                    {
                        //Decrease the cooldown counter
                        shotCoolDownCount -= Time.deltaTime;
                    }
                    else
                    {
                        Shoot();
                        attackIntervalCount = attackInterval;
                    }
                }

                //}
            }
        }
    }

    void Shoot()
    {
        Vector3 dir = transform.position - player.transform.position;
        BaseRocket rocketSpawned = Instantiate(rocket, transform.position, Quaternion.identity);
        rocketSpawned.isPlayerRocket = false;
        Planet playerPlanet = player.gameObject.GetComponent<Planet>();
        Vector3 pos = new Vector3(playerPlanet.Radius * Mathf.Sin(playerPlanet.Angle + Random.Range(0.2f, 0.7f)), 0, playerPlanet.Radius * Mathf.Cos(playerPlanet.Angle));
        rocketSpawned.transform.LookAt(pos);
    }

    public void Init(BaseRocket rocketPrefab, float attackRadius = 400, float coolDown = 4, float attackInterVal = 5)
    {
        this.rocket = rocketPrefab;
        this.attackRadius = attackRadius;
        this.shotCooldown = coolDown;
        this.attackInterval = attackInterVal;
    }
}
