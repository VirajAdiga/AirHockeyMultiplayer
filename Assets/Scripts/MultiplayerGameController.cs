using System.Collections;
using UnityEngine;
using Photon.Pun;

public class MultiplayerGameController : MonoBehaviourPun
{

    public GameObject player;

    public GameObject puck;

    public Transform up, down, left, right, middle;

    private Vector2[] spawnPoints;

    private int masterClientScore, otherPlayerScore;

    private readonly int maxScoreNeeded = 11;

    [SerializeField]
    private GameObject gamePanel;


    private MultiplayerGameController()
    {

    }

    static MultiplayerGameController instance;

    public static MultiplayerGameController gameController
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
        spawnPoints = new Vector2[2];
        spawnPoints[0] = new Vector2(2.2f, -6.13f);
        spawnPoints[1] = new Vector2(-2.2f, 6.13f);

        masterClientScore = otherPlayerScore = 0;
    }

    void Start()
    {
        photonView.RPC("DisplayInstruction", RpcTarget.All);
        SpawnPlayer();
    }

    [PunRPC]
    IEnumerator DisplayInstruction()
    {
        photonView.RPC("ChangeCameraColorToWhite", RpcTarget.All);
        MultiplayerGameUIManager.UIManager.photonView.RPC("ChangeInstructionText", RpcTarget.All, "SCORE " + maxScoreNeeded + " TO WIN");
        MultiplayerGameUIManager.UIManager.photonView.RPC("DisableGamePanel", RpcTarget.All);
        yield return new WaitForSeconds(3f);
        photonView.RPC("ChangeCameraColorToBlack", RpcTarget.All);
        MultiplayerGameUIManager.UIManager.photonView.RPC("EnableGamePanel", RpcTarget.All);
    }

    [PunRPC]
    private void ChangeCameraColorToBlack()
    {
        Camera.main.backgroundColor = Color.black;
    }

    [PunRPC]
    private void ChangeCameraColorToWhite()
    {
        Camera.main.backgroundColor = Color.white;
    }

    private void ResetGamePlay()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PlayerController.localPlayer.transform.position = spawnPoints[0];
        }
        else
        {
            PlayerController.localPlayer.transform.position = spawnPoints[1];
        }

        puck.transform.position = Vector2.zero;
    }

    [PunRPC]
    IEnumerator resetGame()
    {
        byte alpha = 0;
        Color32 panelColor = new Color32(255, 255, 255, 0);

        MultiplayerGameUIManager.UIManager.panel.color = panelColor;

        ResetGamePlay();

        while (alpha < 255)
        {
            alpha += 5;
            panelColor = new Color32(255, 255, 255, alpha);
            MultiplayerGameUIManager.UIManager.panel.color = panelColor;
            yield return null;
        }

        while (alpha > 0)
        {
            alpha -= 5;
            panelColor = new Color32(255, 255, 255, alpha);
            MultiplayerGameUIManager.UIManager.panel.color = panelColor;
            yield return null;
        }

        panelColor = new Color32(255, 255, 255, 0);
        MultiplayerGameUIManager.UIManager.panel.color = panelColor;

        yield return new WaitForSeconds(0.1f);
    }

    private void SpawnPlayer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PlayerController.localPlayer = PhotonNetwork.Instantiate(this.player.name, spawnPoints[0], Quaternion.identity);
        }
        else
        {
            PlayerController.localPlayer = PhotonNetwork.Instantiate(this.player.name, spawnPoints[1], Quaternion.identity);
        }
    }

    [PunRPC]
    public void OtherPlayerHasScored()
    {
        ++otherPlayerScore;
        MultiplayerGameUIManager.UIManager.photonView.RPC("SetOtherPlayerScore", RpcTarget.All, otherPlayerScore);
        if (otherPlayerScore >= maxScoreNeeded)
        {
            photonView.RPC("MasterClientLost", RpcTarget.All);
        }
        else
        {
            photonView.RPC("resetGame", RpcTarget.All);
        }
    }

    [PunRPC]
    public void MasterClientHasScored()
    {
        ++masterClientScore;
        MultiplayerGameUIManager.UIManager.photonView.RPC("SetMasterClientScore", RpcTarget.All, masterClientScore);
        if (masterClientScore >= maxScoreNeeded)
        {
            photonView.RPC("OtherPlayerLost", RpcTarget.All);
        }
        else
        {
            photonView.RPC("resetGame", RpcTarget.All);
        }
    }

    [PunRPC]
    private void MasterClientLost()
    {
        photonView.RPC("displayMsgg", RpcTarget.All, MultiplayerGameUIManager.UIManager.otherPlayerName.ToUpper() + " WON");
    }

    [PunRPC]
    private IEnumerator displayMsgg(string text)
    {
        PlayerController.localPlayer.SetActive(false);
        MultiplayerGameUIManager.UIManager.DisableGame();
        MultiplayerGameUIManager.UIManager.pausePanel.SetActive(false);
        MultiplayerGameUIManager.UIManager.gameStatus.gameObject.SetActive(true);
        MultiplayerGameUIManager.UIManager.gameStatus.text = text;
        yield return new WaitForSeconds(4f);
        PhotonNetwork.LoadLevel("MultiplayerMenu");
        leaveRoom();
    }

    [PunRPC]
    private void OtherPlayerLost()
    {
        photonView.RPC("displayMsgg", RpcTarget.All, MultiplayerGameUIManager.UIManager.masterClientName.ToUpper()+ " WON");
    }

    private void DisableGamePlay()
    {
        MultiplayerGameUIManager.UIManager.DisableGame();
    }

    public void EnableGamePlay()
    {
        MultiplayerGameUIManager.UIManager.DisablePause();
    }

    public void LeaveRoom()
    {
        string playerName = PhotonNetwork.NickName;
        photonView.RPC("leaveTheRoom", RpcTarget.All, playerName);
    }

    IEnumerator displayMsg(string msg)
    {
        MultiplayerGameUIManager.UIManager.gameStatus.gameObject.SetActive(true);
        MultiplayerGameUIManager.UIManager.gameStatus.text = msg;
        yield return new WaitForSeconds(4f);
        MultiplayerGameUIManager.UIManager.gameStatus.text = "WAITING FOR THE HOST TO START THE GAME";
    }

    public void quitMultiplayerGame()
    {
        MultiplayerGameUIManager.UIManager.SetRoomName(string.Empty);
        MultiplayerGameUIManager.UIManager.SetMasterClientName(string.Empty);
        MultiplayerGameUIManager.UIManager.SetMasterClientScore(0);
        MultiplayerGameUIManager.UIManager.SetOtherPlayerName(string.Empty);
        MultiplayerGameUIManager.UIManager.SetOtherPlayerScore(0);
        //PhotonNetwork.Destroy(NetworkManager.networkManager.gameObject);

        PhotonNetwork.LoadLevel("MultiplayerMenu");
    }

    public void leaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        Destroy(NetworkManager.networkManager.gameObject);
    }

    [PunRPC]
    IEnumerator leaveTheRoom(string playerName)
    {
        MultiplayerGameUIManager.UIManager.DisableGame();
        MultiplayerGameUIManager.UIManager.pausePanel.SetActive(false);
        StartCoroutine(displayMsg(playerName.ToUpper() + " QUIT THE GAME"));
        yield return new WaitForSeconds(3f);
        quitMultiplayerGame();
        leaveRoom();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DisableGamePlay();
        }
    }
}
