using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager networkManager;

    private bool isConnectedToServer;

    public string RoomName
    {
        get;
        private set;
    }

    public string MasterClient
    {
        get;
        private set;
    }

    public string OtherPlayer
    {
        get;
        private set;
    }

    private void Awake()
    {
        if(networkManager!=null && networkManager != this)
        {
            gameObject.SetActive(false);
        }
        else
        {
            networkManager = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        isConnectedToServer = false;
        SetNamesEmpty();
    }

    private void SetNamesEmpty()
    {
        RoomName = MasterClient = OtherPlayer = string.Empty;
    }

    public void ConnectToGameServer()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        isConnectedToServer = true;
        
        UIManagerMultiplayer.uiManager.ModifyCreateRoomText("CONNECTED");
        UIManagerMultiplayer.uiManager.ModifyJoinRoomText("CONNECTED");
    }

    public void JoinRoom()
    {
        bool isNameEmpty = UIManagerMultiplayer.uiManager.isTextEmpty(UIManagerMultiplayer.uiManager.joinRoomInfo) || UIManagerMultiplayer.uiManager.isTextEmpty(UIManagerMultiplayer.uiManager.joinRoomPlayerInfo);
        if (isNameEmpty)
        {
            UIManagerMultiplayer.uiManager.ModifyJoinRoomText("NAME CANNOT BE EMPTY");
        }
        else
        {
            if (!PhotonNetwork.IsConnected)
            {
                ConnectToGameServer();
                UIManagerMultiplayer.uiManager.ModifyJoinRoomText("CONNECTING...");
            }
            StartCoroutine(waitUntilJoined());
        }
    }

    public void CreateRoom()
    {
        bool isNameEmpty = UIManagerMultiplayer.uiManager.isTextEmpty(UIManagerMultiplayer.uiManager.createRoomInfo) || UIManagerMultiplayer.uiManager.isTextEmpty(UIManagerMultiplayer.uiManager.createRoomPlayerInfo);
        if (isNameEmpty)
        {
            UIManagerMultiplayer.uiManager.ModifyCreateRoomText("NAME CANNOT BE EMPTY");
        }
        else
        {
            if (!PhotonNetwork.IsConnected)
            {
                ConnectToGameServer();
                UIManagerMultiplayer.uiManager.ModifyCreateRoomText("CONNECTING...");
            }
            StartCoroutine(waitUntilConnected());
        }
    }

    public override void OnCreatedRoom()
    {
        UIManagerMultiplayer.uiManager.ModifyCreateRoomText("CREATED ROOM ");
        StartCoroutine(LoadLobbyScene());
    }

    public override void OnJoinedRoom()
    {
        UIManagerMultiplayer.uiManager.ModifyJoinRoomText("JOINED ROOM...");
        StartCoroutine(LoadLobbyScene());
    }

    private bool IsConnectedToServer()
    {
        if (isConnectedToServer)
            return true;
        return false;
    }

    IEnumerator waitUntilConnected()
    {
        yield return new WaitUntil(IsConnectedToServer);
        RoomOptions roomOptions=new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(UIManagerMultiplayer.uiManager.createRoomInfo.text,roomOptions);

        PhotonNetwork.NickName = UIManagerMultiplayer.uiManager.createRoomPlayerInfo.text;

        RoomName = UIManagerMultiplayer.uiManager.createRoomInfo.text;
        MasterClient = UIManagerMultiplayer.uiManager.createRoomPlayerInfo.text;
    }

    IEnumerator waitUntilJoined()
    {
        yield return new WaitUntil(IsConnectedToServer);

        PhotonNetwork.JoinRoom(UIManagerMultiplayer.uiManager.joinRoomInfo.text);
        PhotonNetwork.NickName = UIManagerMultiplayer.uiManager.joinRoomPlayerInfo.text;

        OtherPlayer = UIManagerMultiplayer.uiManager.joinRoomPlayerInfo.text;
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        UIManagerMultiplayer.uiManager.ModifyJoinRoomText(message.ToUpper());
    }

    IEnumerator LoadLobbyScene()
    {
        yield return new WaitForSeconds(1f);
        MultiPlayerMenuManager.multiPlayerMenuManager.isInOpenPanel = true;
        LoadLobby();
    }

    private void LoadLobby()
    {
        photonView.RPC("UpdateLobbyData", RpcTarget.All);
        MultiPlayerMenuManager.multiPlayerMenuManager.ResetPanelPositions();

        UIManagerMultiplayer.uiManager.ModifyCreateRoomText(string.Empty);
        UIManagerMultiplayer.uiManager.ModifyJoinRoomText(string.Empty);

        UIManagerMultiplayer.uiManager.lobbyPanel.gameObject.SetActive(true);
    }

    [PunRPC]
    private void UpdateLobbyData()
    {
        if (string.IsNullOrEmpty(UIManagerMultiplayer.uiManager.createRoomInfo.text))
        {
            UIManagerMultiplayer.uiManager.roomName.text = "ROOM NAME:\n" + UIManagerMultiplayer.uiManager.joinRoomInfo.text.ToUpper();
        }
        else
        {
            UIManagerMultiplayer.uiManager.roomName.text = "ROOM NAME:\n" + UIManagerMultiplayer.uiManager.createRoomInfo.text.ToUpper();
        }

        UIManagerMultiplayer.uiManager.playersName.text = "PLAYERS:\n\n";

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            UIManagerMultiplayer.uiManager.playersName.text += (player.NickName.ToUpper() + "\n");
        }

        if (PhotonNetwork.IsMasterClient)
        {
            UIManagerMultiplayer.uiManager.startGame.interactable = true;
        }
        else
        {
            UIManagerMultiplayer.uiManager.startGame.interactable = false;
        }

        photonView.RPC("waitForTheHostToStart", RpcTarget.All, "WAITING FOR THE HOST TO START THE GAME");
    }

    [PunRPC]
    public void waitForTheHostToStart(string message)
    {
        UIManagerMultiplayer.uiManager.ChangeGameStatus(message);
    }


    public void LoadMultiplayerGame()
    {
        if (PhotonNetwork.InRoom)
        {
            if(PhotonNetwork.CurrentRoom.PlayerCount == 2)
                photonView.RPC("LoadGameScene", RpcTarget.All);
            else
            {
                StartCoroutine(displayMsg("NO OTHER PLAYER IN THE LOBBY"));
            }
        }
    }

    [PunRPC]
    public IEnumerator LoadGameScene()
    {
        UIManagerMultiplayer.uiManager.gameStatus.text = "STARTING THE GAME...";
        yield return new WaitForSeconds(3f);
        
        PhotonNetwork.LoadLevel("MultiPlayerGame");
    }

    public void LeaveRoom()
    {
        string playerName = PhotonNetwork.NickName;
        photonView.RPC("leaveTheRoom", RpcTarget.All, playerName);
    }

    IEnumerator displayMsg(string msg)
    {
        UIManagerMultiplayer.uiManager.gameStatus.text = msg;
        yield return new WaitForSeconds(4f);
        UIManagerMultiplayer.uiManager.gameStatus.text = "WAITING FOR THE HOST TO START THE GAME";
    }

    public void quitMultiplayerGame()
    {
        UIManagerMultiplayer.uiManager.lobbyPanel.gameObject.SetActive(false);
        UIManagerMultiplayer.uiManager.joinRoomInfo.text = string.Empty;
        UIManagerMultiplayer.uiManager.joinRoomPlayerInfo.text = string.Empty;
        UIManagerMultiplayer.uiManager.createRoomInfo.text = string.Empty;
        UIManagerMultiplayer.uiManager.createRoomPlayerInfo.text = string.Empty;

        SetNamesEmpty();

        MultiPlayerMenuManager.multiPlayerMenuManager.FocusOpenPanel();
    }

    public void leaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    [PunRPC]
    IEnumerator leaveTheRoom(string playerName)
    {
        StartCoroutine(displayMsg(playerName.ToUpper()+" QUIT THE GAME"));
        yield return new WaitForSeconds(3f);
        quitMultiplayerGame();
        leaveRoom();
    }
}
