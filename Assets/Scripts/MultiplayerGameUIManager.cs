using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MultiplayerGameUIManager : MonoBehaviourPun
{
    private MultiplayerGameUIManager()
    {

    }

    static MultiplayerGameUIManager instance;

    public static MultiplayerGameUIManager UIManager
    {
        get
        {
            return instance;
        }
    }

    public Text Instruction;

    public Text roomName;

    public Text gameStatus;

    public Text masterClient, otherPlayer;

    public Text masterClientScore, otherPlayerScore;

    public GameObject gamePanel;

    public GameObject pausePanel;

    public Image panel;

    [HideInInspector]
    public string masterClientName, otherPlayerName;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            if (player.IsMasterClient)
            {
                masterClientName = player.NickName;
                SetMasterClientName(player.NickName.ToUpper());
            }
            else
            {
                otherPlayerName = player.NickName;
                SetOtherPlayerName(player.NickName.ToUpper());
            }
        }

        SetMasterClientScore(0);
        SetOtherPlayerScore(0);
    }

    public void SetRoomName(string name)
    {
        roomName.text = name.ToUpper();
    }

    public void SetMasterClientName(string name)
    {
        masterClient.text = name.ToUpper();
    }

    public void SetOtherPlayerName(string name)
    {
        otherPlayer.text = name.ToUpper();
    }

    [PunRPC]
    public void SetMasterClientScore(int score)
    {
        masterClientScore.text = score.ToString();
    }

    [PunRPC]
    public void SetOtherPlayerScore(int score)
    {
        otherPlayerScore.text = score.ToString();
    }

    [PunRPC]
    public void EnableGamePanel()
    {
        Instruction.gameObject.SetActive(false);
        roomName.gameObject.SetActive(false);

        gamePanel.SetActive(true);

        masterClient.gameObject.SetActive(true);
        masterClientScore.gameObject.SetActive(true);
        otherPlayer.gameObject.SetActive(true);
        otherPlayerScore.gameObject.SetActive(true);
    }

    [PunRPC]
    public void DisableGamePanel()
    {
        gamePanel.SetActive(false);

        masterClient.gameObject.SetActive(false);
        masterClientScore.gameObject.SetActive(false);
        otherPlayer.gameObject.SetActive(false);
        otherPlayerScore.gameObject.SetActive(false);

        Instruction.gameObject.SetActive(true);
        roomName.gameObject.SetActive(true);
    }

    [PunRPC]
    public void ChangeInstructionText(string text)
    {
        SetRoomName(PhotonNetwork.CurrentRoom.Name.ToUpper());
        Instruction.text = text;
    }

    public void DisableGame()
    {
        gamePanel.SetActive(false);
        masterClient.gameObject.SetActive(false);
        masterClientScore.gameObject.SetActive(false);
        otherPlayer.gameObject.SetActive(false);
        otherPlayerScore.gameObject.SetActive(false);
        PlayerController.localPlayer.SetActive(false);

        pausePanel.SetActive(true);
    }

    public void DisablePause()
    {
        pausePanel.SetActive(false);

        gamePanel.SetActive(true);
        masterClient.gameObject.SetActive(true);
        masterClientScore.gameObject.SetActive(true);
        otherPlayer.gameObject.SetActive(true);
        otherPlayerScore.gameObject.SetActive(true);
        PlayerController.localPlayer.SetActive(true);
    }
}
