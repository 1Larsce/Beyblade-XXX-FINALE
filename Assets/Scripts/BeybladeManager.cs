using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class BeybladeManager : MonoBehaviourPunCallbacks
{

    [Header("UI")]
    public GameObject uI_InformPanelGameobject;

    public TextMeshProUGUI uI_InformText;
    public GameObject searchForGamesButtonGameobject;

    public GameObject leaveMenu;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (leaveMenu.gameObject.activeSelf)
            {
                leaveMenu.gameObject.SetActive(false);
            }
            else
            {
				leaveMenu.gameObject.SetActive(true);
			}
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1) {
            LeaveRoom();
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

	public override void OnLeftRoom()
	{
        SceneManager.LoadScene(0);
	}

	#region UI Callback Methods
	public void JoinRandomRoom()
    {
        uI_InformText.text = "Searching for available rooms...";

        PhotonNetwork.JoinRandomRoom();

        searchForGamesButtonGameobject.SetActive(false);

    }
    #endregion

    #region PHOTON Callback Methods
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(message);
        uI_InformText.text = message;


        CreateAndJoinRoom();

    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            uI_InformText.text = "Joined to " + PhotonNetwork.CurrentRoom.Name + ". Waiting for other players...";

        }
        else
        {
            uI_InformText.text = "Joined to " + PhotonNetwork.CurrentRoom.Name;
            StartCoroutine(DeactivateAfterSeconds(uI_InformPanelGameobject, 2.0f));
        }
        Debug.Log(PhotonNetwork.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {

        Debug.Log(newPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + " Player count " + PhotonNetwork.CurrentRoom.PlayerCount);
        uI_InformText.text = newPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + " Player count " + PhotonNetwork.CurrentRoom.PlayerCount;

        StartCoroutine(DeactivateAfterSeconds(uI_InformPanelGameobject, 2.0f));

    }


    #endregion

    #region PRIVATE Methods
    void CreateAndJoinRoom()
    {
        string randomRoomName = "Room" + Random.Range(0, 1000);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;

        //Creatin the room
        PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
    }

    IEnumerator DeactivateAfterSeconds(GameObject _gameObject, float _seconds)
    {
        yield return new WaitForSeconds(_seconds);
        _gameObject.SetActive(false);

    }
    #endregion

}