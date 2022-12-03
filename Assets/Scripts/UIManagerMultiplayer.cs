using UnityEngine;
using UnityEngine.UI;

public class UIManagerMultiplayer : MonoBehaviour
{
    public RectTransform openPanel, quitPanel, createRoomPanel, joinRoomPanel, lobbyPanel;

    public Text createRoomText, joinRoomText;

    public InputField createRoomInfo, createRoomPlayerInfo;

    public InputField joinRoomInfo, joinRoomPlayerInfo;

    public Text roomName, playersName;

    public Button startGame, leaveGame;

    public Text gameStatus;

    private UIManagerMultiplayer()
    {

    }

    static UIManagerMultiplayer instance;

    public static UIManagerMultiplayer uiManager
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
        createRoomText.text = joinRoomText.text = string.Empty;
    }

    public void ModifyCreateRoomText(string text)
    {
        createRoomText.text = text;
    }

    public void ModifyJoinRoomText(string text)
    {
        joinRoomText.text = text;
    }

    public bool isTextEmpty(InputField inputField)
    {
        if (string.IsNullOrEmpty(inputField.text))
            return true;

        return false;
    }

    public void ChangeGameStatus(string text)
    {
        gameStatus.text = text;
    }
}
