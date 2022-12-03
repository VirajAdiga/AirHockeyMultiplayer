using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private Vector2 openPanelPos, settingsPanelPos, quitPanelPos, unlockablesPos;

    private Vector2 creditHeadingPos, creditNamePos;

    private bool isInHomePanel = false;

    private void Start()
    {
        StorePanelPositions();
        FocusOpenPanel();
        checkForUnlockables();
    }

    private void checkForUnlockables()
    {
        WinLossData wn = LoadSaveManager.loadSaveManager.LoadGameData();
        int wins = wn.wins;
        print(wn.wins + " " + wn.losses);

        if (wins < GameData.bronzeMileStone)
        {
            MakeButtonsNonInteractable();
        }
        else if(wins>=GameData.bronzeMileStone && wins < GameData.silverMileStone)
        {
            MakeButtonsNonInteractable();
            MakeBronzeInteractable();
        }
        else if(wins>=GameData.silverMileStone && wins < GameData.goldMileStone)
        {
            MakeButtonsNonInteractable();
            MakeBronzeInteractable();
            MakeSilverInteractable();
        }
        else if(wins>=GameData.goldMileStone && wins < GameData.multiPlayerMileStone)
        {
            MakeButtonsNonInteractable();
            MakeBronzeInteractable();
            MakeSilverInteractable();
            MakeGoldInteractable();
        }
        else
        {
            MakeButtonsNonInteractable();
            MakeBronzeInteractable();
            MakeSilverInteractable();
            MakeGoldInteractable();
            MakeMultiPlayerInteractable();
            UIManagerMenu.uiManager.unlockableInstruction.SetActive(false);
        }
    }

    private void MakeButtonsNonInteractable()
    {
        UIManagerMenu.uiManager.multiPlayer.interactable = false;
        UIManagerMenu.uiManager.gold.interactable = false;
        UIManagerMenu.uiManager.silver.interactable = false;
        UIManagerMenu.uiManager.bronze.interactable = false;

        UIManagerMenu.uiManager.multiplayerLock.SetActive(true);
        UIManagerMenu.uiManager.goldLock.SetActive(true);
        UIManagerMenu.uiManager.silverLock.SetActive(true);
        UIManagerMenu.uiManager.bronzeLock.SetActive(true);
    }

    private void MakeMultiPlayerInteractable()
    {
        UIManagerMenu.uiManager.multiPlayer.interactable = true;
        UIManagerMenu.uiManager.multiplayerLock.SetActive(false);
    }

    private void MakeGoldInteractable()
    {
        UIManagerMenu.uiManager.gold.interactable = true;
        UIManagerMenu.uiManager.goldLock.SetActive(false);
    }

    private void MakeSilverInteractable()
    {
        UIManagerMenu.uiManager.silver.interactable = true;
        UIManagerMenu.uiManager.silverLock.SetActive(false);
    }

    private void MakeBronzeInteractable()
    {
        UIManagerMenu.uiManager.bronze.interactable = true;
        UIManagerMenu.uiManager.bronzeLock.SetActive(false);
    }

    private void StorePanelPositions()
    {
        openPanelPos = UIManagerMenu.uiManager.openPanel.localPosition;
        settingsPanelPos = UIManagerMenu.uiManager.settingsPanel.localPosition;
        quitPanelPos = UIManagerMenu.uiManager.quitPanel.localPosition;
        unlockablesPos = UIManagerMenu.uiManager.unlockablesPanel.localPosition;

        creditHeadingPos = UIManagerMenu.uiManager.creditHeading.localPosition;
        creditNamePos = UIManagerMenu.uiManager.creditName.localPosition;
    }

    private void FocusOpenPanel()
    {
        UIManagerMenu.uiManager.openPanel.DOAnchorPos(Vector2.zero, 0.6f, false);
        isInHomePanel = true;
    }

    public void FocusQuitPanel()
    {
        UIManagerMenu.uiManager.openPanel.DOAnchorPos(openPanelPos, 0.6f, false);
        UIManagerMenu.uiManager.quitPanel.DOAnchorPos(Vector2.zero, 0.6f, false);
        isInHomePanel = false;
    }

    public void OpenPanelCloseQuitPanel()
    {
        UIManagerMenu.uiManager.quitPanel.DOAnchorPos(quitPanelPos, 0.6f, false);
        UIManagerMenu.uiManager.openPanel.DOAnchorPos(Vector2.zero, 0.6f, false);
        isInHomePanel = true;
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void FocusSettingsPanel()
    {
        UIManagerMenu.uiManager.openPanel.DOAnchorPos(openPanelPos, 0.6f, false);
        UIManagerMenu.uiManager.settingsPanel.DOAnchorPos(Vector2.zero, 0.6f, false);
        isInHomePanel = false;
    }

    public void ResetGameData()
    {
        LoadSaveManager.loadSaveManager.SaveGameData(0, 0);

        ResetPanelPositions();
    }

    public void OpenCreditPanel()
    {
        UIManagerMenu.uiManager.resetButton.DOAnchorPosX(creditHeadingPos.x, 0.6f, false);
        UIManagerMenu.uiManager.CreditButton.DOAnchorPosX(creditNamePos.x, 0.6f, false);
        UIManagerMenu.uiManager.creditHeading.DOAnchorPosX(0, 0.6f, false);
        UIManagerMenu.uiManager.creditName.DOAnchorPosX(0, 0.6f, false);
        isInHomePanel = false;
    }

    public void FocusUnlockablesPanel()
    {
        UIManagerMenu.uiManager.openPanel.DOAnchorPos(openPanelPos, 0.6f, false);
        UIManagerMenu.uiManager.unlockablesPanel.DOAnchorPos(Vector2.zero, 0.6f, false);
        isInHomePanel = false;
    }

    public void OpenMultiPlayerGame()
    {
        GameData.MutliPlayerGame();
        LoadMultiplayerGame();
    }

    public void OpenGoldGame()
    {
        GameData.GoldUnlock();
        LoadSinglePlayerGame();
    }

    public void OpenSilverGame()
    {
        GameData.SilverUnlock();
        LoadSinglePlayerGame();
    }

    public void OpenBronzeGame()
    {
        GameData.BronzeUnlock();
        LoadSinglePlayerGame();
    }

    public void LoadSinglePlayerGame()
    {
        SceneManager.LoadScene("SinglePlayerGame");
    }

    private void LoadMultiplayerGame()
    {
        SceneManager.LoadScene("MultiPlayerMenu");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isInHomePanel)
        {
            BackToHomeScreen();
        }
    }

    private void BackToHomeScreen()
    {
        ResetPanelPositions();
        FocusOpenPanel();
    }

    private void ResetPanelPositions()
    {
        UIManagerMenu.uiManager.openPanel.localPosition = openPanelPos;
        UIManagerMenu.uiManager.quitPanel.localPosition = quitPanelPos;
        UIManagerMenu.uiManager.settingsPanel.localPosition = settingsPanelPos;
        UIManagerMenu.uiManager.unlockablesPanel.localPosition = unlockablesPos;

        UIManagerMenu.uiManager.creditHeading.localPosition = creditHeadingPos;
        UIManagerMenu.uiManager.creditName.localPosition = creditNamePos;

        UIManagerMenu.uiManager.resetButton.localPosition = new Vector2(0, creditHeadingPos.y);
        UIManagerMenu.uiManager.CreditButton.localPosition = new Vector2(0, creditNamePos.y);

        FocusOpenPanel();
    }
}
