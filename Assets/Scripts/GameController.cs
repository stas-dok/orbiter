using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private List<Planet> planets = new List<Planet>();
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private MainMenuView mainMenuPrefab;
    private MainMenuView mainMenuView;
    [SerializeField]
    private EndGameView endGamePrefab;
    private EndGameView endGameView;
    [SerializeField]
    private PauseGameView pauseGamePrefab;
    private PauseGameView pauseGameView;
    [SerializeField] 
    private RectTransform playerLabel;
    [SerializeField]
    private List<BaseRocket> rocketPrefabs = new List<BaseRocket>();
    [SerializeField] 
    private RectTransform crossHair;
    
    private GameObject playerPlanet;
    
    [SerializeField] 
    private BaseRocket playerRocket;
    [SerializeField] 
    private Slider playerCoolDown;
    [SerializeField] private float coolDownPlayer = 0.5f;
    private float coolDownCounter = 0f;
    
    public int destroyedPlanetsCounter = 0;
    private int currAIPlayers = 0;
    private bool gameStarted = false;
    // Start is called before the first frame update
    void Start()
    {
        CreateMainMenu();
        gameStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerLabel != null && playerPlanet != null)
        {
            playerLabel.position = RectTransformUtility.WorldToScreenPoint(Camera.main,
                new Vector3(playerPlanet.transform.position.x, playerPlanet.transform.position.y, playerPlanet.transform.position.z));
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CreatePauseMenu();
        }

        if (gameStarted)
        {
            if (destroyedPlanetsCounter >= currAIPlayers)
            { 
                EndGameMenu(true);
            }
        
        
            Vector3 orbVector = Camera.main.WorldToScreenPoint(playerPlanet.transform.position);
            orbVector = Input.mousePosition - orbVector;
            float angle = Mathf.Atan2(orbVector.y, orbVector.x) * Mathf.Rad2Deg;
            crossHair.transform.position =  RectTransformUtility.WorldToScreenPoint(Camera.main, playerPlanet.transform.position);
            crossHair.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        

            if (Input.GetMouseButton(0) && coolDownCounter == 0f)
            {
                Shoot();
                coolDownCounter = coolDownPlayer;
            }

            if (coolDownCounter > 0)
            {
                coolDownCounter -= Time.deltaTime;
                if (coolDownCounter <= 0f)
                {
                    coolDownCounter = 0f;
                }

                playerCoolDown.value = 1 - coolDownCounter / coolDownPlayer;
            }
        }
       
    }

    //GUI section
    void CreateMainMenu()
    {
        if (!mainMenuView)
        {
            mainMenuView = Instantiate(mainMenuPrefab, transform.position, Quaternion.identity);
            SetViewPanelParams(mainMenuView.transform);
        } 
        else
        {
            mainMenuView.gameObject.SetActive(true);
        }
        Time.timeScale = 0;
    }
    
    void CreatePauseMenu()
    {
        

        if (!pauseGameView)
        {
            pauseGameView = Instantiate(pauseGamePrefab, transform.position, Quaternion.identity);
            SetViewPanelParams(pauseGameView.transform);
        }
        else
        {
            pauseGameView.gameObject.SetActive(true);
        }
        Time.timeScale = 0;
    }

    void SetViewPanelParams(Transform gameObject)
    {
        gameObject.parent = canvas.transform;
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0,0);
    }
    
    public void ContinueGame()
    {
        pauseGameView.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    
    public void EndGameMenu(bool win = false)
    {
        if (!endGameView)
        {
            endGameView = Instantiate(endGamePrefab, transform.position, Quaternion.identity);
            SetViewPanelParams(endGameView.transform);
        }
        else
        {
            endGameView.gameObject.SetActive(true);
        }
        endGameView.Init(win);
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void StartGame(int aiPlayers = 1)
    {
        Debug.Log("aiPlayers: " + aiPlayers);
        currAIPlayers = aiPlayers;
        Time.timeScale = 1;
        int playerPlanetNumber = Random.Range(0, planets.Count - 1);
        playerPlanet = planets[playerPlanetNumber].gameObject;
        playerPlanet.AddComponent<Player>();
        /*Rigidbody r = playerPlanet.AddComponent<Rigidbody>();
        r.constraints = RigidbodyConstraints.FreezeAll;
        r.useGravity = false;*/
        int counterCurrAIPlayers = 0;
        for (int i = 0; i < planets.Count; i++)
        {
            if (i != playerPlanetNumber && counterCurrAIPlayers < aiPlayers)
            {
                AIController controller = planets[i].gameObject.AddComponent<AIController>();
                int rocketType = Random.Range(0, rocketPrefabs.Count - 1);
                float range = Random.Range(400, 1000);
                controller.Init(rocketPrefabs[rocketType], range, Random.Range(1, 5), Random.Range(1, 7));
                counterCurrAIPlayers++;
            }
            else if (i != playerPlanetNumber)
            {
                planets[i].HealthGUI.gameObject.SetActive(false);
            }
        }
        mainMenuView.gameObject.SetActive(false);
        gameStarted = true;
    }
    //End gui section
    
    void Shoot()
    {
        BaseRocket rocketSpawned = Instantiate(playerRocket, playerPlanet.transform.position, crossHair.transform.rotation);
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        rocketSpawned.transform.LookAt(new Vector3(pos.x, 0, pos.z));
        //rocketSpawned.transform.rotation = Quaternion.AngleAxis(crossHair.eulerAngles.z, Vector3.forward);
        rocketSpawned.isPlayerRocket = true;
    }
}
