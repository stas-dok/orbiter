using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseGameView : MonoBehaviour
{
    public Button continueGame;
    public Button exit;
    // Start is called before the first frame update
    void Start()
    {
        continueGame.onClick.AddListener(ContinueGame);
        exit.onClick.AddListener(Application.Quit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ContinueGame()
    {
        
        GameController controller = FindObjectOfType<GameController>();
        controller.ContinueGame();
    }
}
