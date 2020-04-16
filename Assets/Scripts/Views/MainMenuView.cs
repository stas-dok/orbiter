using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : MonoBehaviour
{
    public Button startGame;
    public Slider slider;
    public Button exit;
    public Text playerCount;
    
    private int aiPlayers;
    // Start is called before the first frame update
    void Start()
    {
        startGame.onClick.AddListener(StartGameClick);
        slider.onValueChanged.AddListener(ChangePlayers);
        exit.onClick.AddListener(Application.Quit);
        ChangePlayers(1);
    }

    void ChangePlayers(float val)
    {
        aiPlayers = (int) val;
        playerCount.text = aiPlayers.ToString();
    }

    void StartGameClick()
    {
        GameController controller = FindObjectOfType<GameController>();
        controller.StartGame(aiPlayers);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
