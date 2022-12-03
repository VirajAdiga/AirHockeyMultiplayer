using UnityEngine;
using UnityEngine.UI;

public class UIManagerMenu : MonoBehaviour
{
    private UIManagerMenu()
    {

    }

    static UIManagerMenu instance;

    public static UIManagerMenu uiManager
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

    public RectTransform openPanel, quitPanel, settingsPanel, unlockablesPanel;

    public GameObject multiplayerLock, goldLock, silverLock, bronzeLock;

    public Button multiPlayer, gold, silver, bronze;

    public RectTransform creditHeading, creditName, resetButton, CreditButton;

    public GameObject unlockableInstruction;
}
