using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Planet : MonoBehaviour
{
    [SerializeField]
    private float radius = 11.6f;
    [SerializeField]
    private float speed = 0.8f;
    [SerializeField]
    private float rotationSpeed = 10.0f;
    [SerializeField]
    private float health = 100.0f;
    [SerializeField] 
    private Slider healthGUI; 
    [SerializeField] 
    private float hpShift;
    private RectTransform healthGUIRect;
    
    [SerializeField]
    private GameObject explosionPefab;
    
    private float startAngle = 0f;
    private float angle = 0f;
    
    public float Health => health; 
    public float Radius => radius;
    public float Angle => angle;
    public Slider HealthGUI => healthGUI;

    private bool planetDestroyed = false;
    // Start is called before the first frame update
    void Start()
    {
        startAngle = Random.Range(0.0f, 60.0f) * speed;
        transform.localPosition = GetPosition(startAngle);
        if (healthGUI != null)
        {
            healthGUIRect = healthGUI.gameObject.GetComponent<RectTransform>();
        }

        if (GetComponent<TrailRenderer>())
        {
            GetComponent<TrailRenderer>().enabled = false;
            Invoke("EnableTrailRenderer", 1f);
        }

    }
    
    // Update is called once per frame
    void Update()
    {
        angle = startAngle + (Time.time * speed);
        transform.localPosition = GetPosition(angle);
        transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed);
        if (healthGUI != null)
        {
            healthGUIRect.position = RectTransformUtility.WorldToScreenPoint(Camera.main,
                new Vector3(transform.position.x, transform.position.y, transform.position.z + hpShift));
            healthGUI.value = health / 100;
        }
    }
    
    private Vector3 GetPosition(float angle)
    {
        return new Vector3(radius * Mathf.Sin(angle), 0, radius * Mathf.Cos(angle));
    }

    public void SetDamage(float damage)
    {
        if (!planetDestroyed)
        {
            if (explosionPefab != null)
            {
                Instantiate(explosionPefab, transform.position, Quaternion.identity);
            }

            health -= damage;
            if (health <= 0)
            {
                GameController controller = FindObjectOfType<GameController>();
                if (gameObject.GetComponent<Player>())
                {
                    Debug.Log("Game ended");
                    Invoke(nameof(EndGame), 2f);
                }
                else
                {
                    controller.destroyedPlanetsCounter++;
                }

                if (healthGUI != null)
                {
                    healthGUI.gameObject.SetActive(false);
                }

                planetDestroyed = true;
                gameObject.SetActive(false);
            }

            Debug.Log(transform.name + " Add damage:" + damage);
        }
    }

    void EndGame()
    {
        GameController controller = FindObjectOfType<GameController>();
        controller.EndGameMenu(false);
    }
    
    void EnableTrailRenderer()
    {
        if (GetComponent<TrailRenderer>())
        {
            GetComponent<TrailRenderer>().enabled = true;
        }
    }
}
