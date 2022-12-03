using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Transform up, down, left, right, middle;

    public Transform player, opponent, puck;

    private Vector2 startPositonOfPlayer, startPositionOfOpponent, startPositionOfPuck;

    private float offsetPos;

    [HideInInspector]
    public float speed
    {
        get;
        private set;
    }

    [HideInInspector]
    public int maxScoreNeeded
    {
        get;
        private set;
    }

    private Camera mainCamera;

    private int wins, losses;

    private bool isGameFinished;


    private GameController()
    {

    }

    static GameController instance;

    public static GameController gameController
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        isGameFinished = false;

        startPositonOfPlayer = player.position;
        startPositionOfOpponent = opponent.position;
        startPositionOfPuck = puck.position;

        mainCamera = Camera.main;

        UIManager.uiManager.gameResultPanel.SetActive(false);

        WinLossData wn = LoadSaveManager.loadSaveManager.LoadGameData();
        wins = wn.wins;
        losses = wn.losses;

        SetSpeedForEnemy();
        StartCoroutine(displayInstruction());
    }

    private void SetSpeedForEnemy()
    {
        if(wins < GameData.bronzeMileStone)
        {
            speed = 25;
            maxScoreNeeded = 9;
        }
        else if(wins>=GameData.bronzeMileStone && wins < GameData.silverMileStone)
        {
            speed = 30;
            maxScoreNeeded = 13;
        }
        else if(wins>=GameData.silverMileStone && wins < GameData.goldMileStone)
        {
            speed = 45;
            maxScoreNeeded = 17;
        }
        else if(wins>=GameData.goldMileStone && wins <= GameData.multiPlayerMileStone)
        {
            speed = 55;
            maxScoreNeeded = 21;
        }
        else
        {
            speed = 60;
            maxScoreNeeded = 25;
        }
    }

    public void AIHasScored()
    {
        UIManager.uiManager.IncrementAIPoint();
        if (UIManager.uiManager.getAIPoints() >= maxScoreNeeded)
        {
            PlayerLost();
        }
        else
        {
            StartCoroutine(resetGame());
        }
    }

    public void PlayerHasScored()
    {
        UIManager.uiManager.IncrementPlayerPoint();
        if (UIManager.uiManager.getPlayerScore() >= maxScoreNeeded)
        {
            PlayerWon();
        }
        else
        {
            StartCoroutine(resetGame());
        }
    }

    public void ResetGamePlay()
    {
        offsetPos = Random.Range(-4f, 2f);

        player.position = new Vector2(startPositonOfPlayer.x + offsetPos, startPositonOfPlayer.y+offsetPos);
        opponent.position = new Vector2(startPositionOfOpponent.x + offsetPos, startPositionOfOpponent.y+offsetPos);
        puck.position = new Vector2(startPositionOfPuck.x + offsetPos, startPositionOfPuck.y + offsetPos);
    }

    IEnumerator resetGame()
    {
        byte alpha = 0;
        Color32 panelColor = new Color32(255, 255, 255, 0);

        UIManager.uiManager.panel.color = panelColor;

        ResetGamePlay();
        StartCoroutine(PerformCameraAnim());

        while (alpha < 255)
        {
            alpha += 5;
            panelColor = new Color32(255, 255, 255, alpha);
            UIManager.uiManager.panel.color = panelColor;
            yield return null;
        }

        while (alpha > 0)
        {
            alpha -= 5;
            panelColor = new Color32(255, 255, 255, alpha);
            UIManager.uiManager.panel.color = panelColor;
            yield return null;
        }

        panelColor = new Color32(255, 255, 255, 0);
        UIManager.uiManager.panel.color = panelColor;

        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator PerformCameraAnim()
    {
        float minFocus = 1f, maxFocus = 13;

        mainCamera.orthographicSize = maxFocus;

        float cameraSize = maxFocus;

        while (cameraSize > minFocus)
        {
            cameraSize -= 0.4f;
            mainCamera.orthographicSize = cameraSize;
            yield return null;
        }

        while (cameraSize < maxFocus)
        {
            cameraSize += 0.4f;
            mainCamera.orthographicSize = cameraSize;
            yield return null;
        }

        mainCamera.orthographicSize = maxFocus;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DisableGamePlay();
            EnablePausePanel();
        }

        if (isGameFinished)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                LoadMainMenu();
            }
        }
    }

    private void DisableGamePlay()
    {
        UIManager.uiManager.GamePlay.SetActive(false);
        UIManager.uiManager.playerScore.gameObject.SetActive(false);
        UIManager.uiManager.AIScore.gameObject.SetActive(false);
    }

    public void EnableGamePlay()
    {
        DisablePausePanel();
        UIManager.uiManager.GamePlay.SetActive(true);
        UIManager.uiManager.playerScore.gameObject.SetActive(true);
        UIManager.uiManager.AIScore.gameObject.SetActive(true);
    }

    private void DisablePausePanel()
    {
        UIManager.uiManager.pauseMenu.SetActive(false);
        mainCamera.backgroundColor = Color.black;
    }

    private void EnablePausePanel()
    {
        mainCamera.backgroundColor = Color.white;
        UIManager.uiManager.pauseMenu.SetActive(true);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("SinglePlayerGame");
    }

    private void PlayerWon()
    {
        wins += 1;
        DisableGamePlay();
        UIManager.uiManager.gameResultPanel.SetActive(true);
        StartCoroutine(panelFade());

        if (wins == GameData.bronzeMileStone)
        {
            UIManager.uiManager.gameResult.text = "WON\nYOU HAVE UNLOCKED BRONZE";
        }
        else if (wins == GameData.silverMileStone)
        {
            UIManager.uiManager.gameResult.text = "WON\nYOU HAVE UNLOCKED SILVER";
        }
        else if (wins == GameData.goldMileStone)
        {
            UIManager.uiManager.gameResult.text = "WON\nYOU HAVE UNLOCKED GOLD";
        }
        else if (wins == GameData.multiPlayerMileStone)
        {
            UIManager.uiManager.gameResult.text = "WON\nYOU HAVE UNLOCKED MULTIPLAYER";
        }
        else
        {
            UIManager.uiManager.gameResult.text = "WON";
        }

        UIManager.uiManager.wins.text = wins.ToString();
        UIManager.uiManager.losses.text = losses.ToString();
        AudioManager.audioManager.PlayGameWin();

        LoadSaveManager.loadSaveManager.SaveGameData(wins, losses);

        WinLossData wn = LoadSaveManager.loadSaveManager.LoadGameData();
        print(wn.wins + " " + wn.losses);

        isGameFinished = true;
    }

    private void PlayerLost()
    {
        losses += 1;

        DisableGamePlay();
        UIManager.uiManager.gameResultPanel.SetActive(true);

        StartCoroutine(panelFade());
        UIManager.uiManager.gameResult.text = "LOST";
        UIManager.uiManager.wins.text = wins.ToString();
        UIManager.uiManager.losses.text = losses.ToString();
        AudioManager.audioManager.PlayGameLost();

        LoadSaveManager.loadSaveManager.SaveGameData(wins, losses);
        WinLossData wn = LoadSaveManager.loadSaveManager.LoadGameData();
        print(wn.wins + " " + wn.losses);

        isGameFinished = true;
    }

    IEnumerator panelFade()
    {
        Color32 color = new Color32(255, 255, 255, 0);
        byte alpha = 0;
        UIManager.uiManager.resultPanel.color = color;

        while (alpha < 255)
        {
            alpha += 5;
            color = new Color32(255, 255, 255, alpha);
            UIManager.uiManager.resultPanel.color = color;
            yield return null;
        }
    }

    IEnumerator displayInstruction()
    {
        DisableGamePlay();
        mainCamera.backgroundColor = Color.white;
        UIManager.uiManager.instruction.text = "SCORE " + maxScoreNeeded + " \nTO WIN";
        UIManager.uiManager.instruction.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        UIManager.uiManager.instruction.gameObject.SetActive(false);
        EnableGamePlay();
    }
}
