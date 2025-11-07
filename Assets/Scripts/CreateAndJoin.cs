using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class CreateAndJoin : MonoBehaviourPunCallbacks
{
    public GameObject Loading;
    public TMP_InputField input_Create;
    public TMP_InputField input_Join;

    public void CreateRoom()
    {
		Loading.SetActive(true);
		PhotonNetwork.CreateRoom(input_Create.text, new RoomOptions { MaxPlayers = 4, IsOpen = true, IsVisible = true }, TypedLobby.Default);
    }

    public void JoinRoom()
    {
		Loading.SetActive(true);
		PhotonNetwork.JoinRoom(input_Join.text);
    }

    public void JoinRoomInList(string RoomName)
    {
		Loading.SetActive(true);
		PhotonNetwork.JoinRoom(RoomName);
    }

    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene("Scene_PlayerSelection");
    }
}
