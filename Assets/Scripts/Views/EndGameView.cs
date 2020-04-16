using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameView : MonoBehaviour
{
    public Button restartGame;
    public Text youWin;
    public Text youLoose;
    public Button exit;
    // Start is called before the first frame update
    void Start()
    {
        restartGame.onClick.AddListener(RestartGame);
        exit.onClick.AddListener(Application.Quit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(bool win)
    {
        youWin.gameObject.SetActive(win);
        youLoose.gameObject.SetActive(!win);
    }
    
    void RestartGame()
    {
        GameController controller = FindObjectOfType<GameController>();
        controller.RestartGame();
    }
}
