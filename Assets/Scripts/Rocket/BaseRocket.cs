using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRocket : MonoBehaviour
{    
    [SerializeField]
    private GameObject explosion;
    [SerializeField]
    private float timeOut= 3.0f;	
    [SerializeField]
    private float damage = 1.0f;
    [SerializeField]
    private float speed = 5.0f;

    public bool isPlayerRocket = false;
    
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Kill", timeOut);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * Vector3.forward * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Sun")
        {
            Instantiate(explosion, other.GetContact(0).point, Quaternion.identity);
            Kill();
        }

        if (isPlayerRocket && other.gameObject.GetComponent<AIController>())
        {
            Planet planet = other.gameObject.GetComponent<Planet>();
            planet.SetDamage(damage);
            Instantiate(explosion, other.GetContact(0).point, Quaternion.identity);
            Kill();
        }
        
        if (!isPlayerRocket && other.gameObject.GetComponent<Player>())
        {
            Planet planet = other.gameObject.GetComponent<Planet>();
            planet.SetDamage(damage);
            Instantiate(explosion, other.GetContact(0).point, Quaternion.identity);
            Kill();
        }
    }

    void Kill()
    {
        Destroy(gameObject);
    }
}
