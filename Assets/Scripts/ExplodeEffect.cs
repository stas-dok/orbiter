using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeEffect : MonoBehaviour
{
    [SerializeField]
    private float timeOut= 3.0f;	
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Kill", timeOut);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void Kill()
    {
        Destroy(gameObject);
    }
}
