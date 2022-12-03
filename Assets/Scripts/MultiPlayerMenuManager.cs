using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MultiPlayerMenuManager : MonoBehaviour
{
    [HideInInspector]
    public bool isInOpenPanel;

    private Vector2 openPanelPos, quitPanelPos, createRoomPanelPos, joinRoomPanelPos;

    private MultiPlayerMenuManager()
    {

    }

    static MultiPlayerMenuManager instance;

    public static MultiPlayerMenuManager multiPlayerMenuManager
    {
        get{
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        isInOpenPanel = true;

        StorePanelPositions();
        FocusOpenPanel();
    }

    private void StorePanelPositions()
    {
        openPanelPos = UIManagerMultiplayer.uiManager.openPanel.localPosition;
        quitPanelPos = UIManagerMultiplayer.uiManager.quitPanel.localPosition;
        createRoomPanelPos = UIManagerMultiplayer.uiManager.createRoomPanel.localPosition;
        joinRoomPanelPos = UIManagerMultiplayer.uiManager.joinRoomPanel.localPosition;
    }

    private void Update()
    {
        if (!isInOpenPanel)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ResetPanelPositions();
                FocusOpenPanel();
            }
        }
    }

    public void FocusOpenPanel()
    {
        isInOpenPanel = true;

        UIManagerMultiplayer.uiManager.openPanel.DOAnchorPos(Vector2.zero, 0.6f, false);
    }

    public void FocusQuitPanel()
    {
        isInOpenPanel = false;

        UIManagerMultiplayer.uiManager.openPanel.DOAnchorPos(openPanelPos, 0.6f, false);
        UIManagerMultiplayer.uiManager.quitPanel.DOAnchorPos(Vector2.zero, 0.6f, false);
    }

    public void CloseQuitFocusOpenPanel()
    {
        UIManagerMultiplayer.uiManager.quitPanel.DOAnchorPos(quitPanelPos, 0.6f, false);
        FocusOpenPanel();
    }

    public void FocusCreateRoomPanel()
    {
        isInOpenPanel = false;

        UIManagerMultiplayer.uiManager.openPanel.DOAnchorPos(openPanelPos, 0.6f, false);
        UIManagerMultiplayer.uiManager.createRoomPanel.DOAnchorPos(Vector2.zero, 0.6f, false);
    }

    public void FocusJoinRoomPanel()
    {
        isInOpenPanel = false;
        UIManagerMultiplayer.uiManager.openPanel.DOAnchorPos(openPanelPos, 0.6f, false);
        UIManagerMultiplayer.uiManager.joinRoomPanel.DOAnchorPos(Vector2.zero, 0.6f, false);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ResetPanelPositions()
    {
        UIManagerMultiplayer.uiManager.openPanel.localPosition = openPanelPos;
        UIManagerMultiplayer.uiManager.quitPanel.localPosition = quitPanelPos;
        UIManagerMultiplayer.uiManager.createRoomPanel.localPosition = createRoomPanelPos;
        UIManagerMultiplayer.uiManager.joinRoomPanel.localPosition = joinRoomPanelPos;
    }

    public void StartMultiPlayerGame()
    {
        NetworkManager.networkManager.LoadMultiplayerGame();
    }
}
