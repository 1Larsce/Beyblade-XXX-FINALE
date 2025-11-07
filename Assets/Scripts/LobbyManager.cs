using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("Login UI")]
    public GameObject Loading;
    public InputField playerNameInputField;
    public GameObject uI_LoginGameobject;

    [Header("Lobby UI")]
    public GameObject uI_LobbyGameobject;
    public GameObject uI_3DGameobject;

    [Header("Connection Status UI")]
    public GameObject uI_ConnectionStatusGameObject;
    public Text connectionStatusText;
    public bool showConnectionStatus = false;

    #region UNITY Methods
    // Start is called before the first frame update
    void Start()
    {

        if (PhotonNetwork.IsConnected)
        {
            //Activating only Lobby UI
            uI_LobbyGameobject.SetActive(true);
            uI_3DGameobject.SetActive(true);


            uI_ConnectionStatusGameObject.SetActive(false);

            uI_LoginGameobject.SetActive(false);

        }
        else
        {
            //Activating only Login UI since we did noy connect to Photon yet.

            uI_LobbyGameobject.SetActive(false);
            uI_3DGameobject.SetActive(false);
            uI_ConnectionStatusGameObject.SetActive(false);

            uI_LoginGameobject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (showConnectionStatus)
        {
            connectionStatusText.text = "Connecion Status: " + PhotonNetwork.NetworkClientState;
        }

    }
    #endregion



    #region UI Callback Methods
    public void OnEnterGameButtonClicked()
    {

        string playerName = playerNameInputField.text;

        if (!string.IsNullOrEmpty(playerName))
        {
            uI_LobbyGameobject.SetActive(false);
            uI_3DGameobject.SetActive(false);
            uI_LoginGameobject.SetActive(false);


            showConnectionStatus = true;
            uI_ConnectionStatusGameObject.SetActive(true);


            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.LocalPlayer.NickName = playerName;

                PhotonNetwork.ConnectUsingSettings();
            }
        }
        else
        {
            Debug.Log("Player name is invalid or empty!");
        }
    }

    public void OnQuickMatchButtonClicked()
    {
        //SceneManager.LoadScene("Scene_Loading");
        //SceneLoader.Instance.LoadScene("Scene_PlayerSelection");
        QuickMatch();
    }
    #endregion

    public void QuickMatch()
    {
		Loading.SetActive(true);
		PhotonNetwork.JoinOrCreateRoom("room" + Random.Range(112, 331), new Photon.Realtime.RoomOptions { MaxPlayers = 2, IsOpen = true, IsVisible = true}, Photon.Realtime.TypedLobby.Default);
    }
	public override void OnJoinedRoom()
	{
        print("[Joined Game]");
        SceneManager.LoadScene(1);
	}

	#region PHOTON Callback Methods
	public override void OnConnected()
    {
        Debug.Log("Connected to the Internet.");
    }

    public override void OnConnectedToMaster()
    {
        Loading.SetActive(false);

		print("[Joined Lobby]");

		PhotonNetwork.JoinLobby();

		Debug.Log(PhotonNetwork.LocalPlayer.NickName+ " is connected to Photon Server");

        uI_LobbyGameobject.SetActive(true);
        uI_3DGameobject.SetActive(true);


        uI_LoginGameobject.SetActive(false);
        uI_ConnectionStatusGameObject.SetActive(false);
    }

    #endregion
}
