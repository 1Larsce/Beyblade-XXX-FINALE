using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class PlayerSelectionManager : MonoBehaviourPunCallbacks
{
    public Transform playerSwitcherTransform;


    public int playerSelectionNumber;

    [Header("UI")]
    public GameObject waiting;
    public TMPro.TMP_Text current_players;
    public TextMeshProUGUI playerModelType_Text;
    public Button next_Button;
    public Button previous_Button;

    public GameObject[] spinnerTopModels;

    public GameObject uI_Selection;
    public GameObject uI_AfterSelection;



    #region UNITY Methods
    void Start()
    {
        uI_Selection.SetActive(true);
        uI_AfterSelection.SetActive(false);


        playerSelectionNumber = 0;

        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.InRoom) {
            current_players.text = "[" + PhotonNetwork.CurrentRoom.PlayerCount.ToString() + "/" + PhotonNetwork.CurrentRoom.MaxPlayers.ToString()+ "]";
        }
    }
    #endregion

    #region UI Callback Methods

    public void NextPlayer()
    {
        playerSelectionNumber += 1;

        if (playerSelectionNumber >= spinnerTopModels.Length)
        {
            playerSelectionNumber = 0;
        }
        Debug.Log(playerSelectionNumber);




        next_Button.enabled = false;
        previous_Button.enabled = false;

        StartCoroutine(Rotate(Vector3.up, playerSwitcherTransform, 90, 1.0f));

        if (playerSelectionNumber == 0 || playerSelectionNumber == 1)
        {
            //This means the player model type is ATTACK.
            playerModelType_Text.text = "Offense";

        }
        else
        {
            //This means the player model type is DEFEND.
            playerModelType_Text.text = "Defense";

        }
    }

    public void PreviousPlayer()
    {
        playerSelectionNumber -= 1;
        if (playerSelectionNumber < 0)
        {
            playerSelectionNumber = spinnerTopModels.Length - 1;
        }


        Debug.Log(playerSelectionNumber);




        next_Button.enabled = false;
        previous_Button.enabled = false;

        StartCoroutine(Rotate(Vector3.up, playerSwitcherTransform, -90, 1.0f));

        if (playerSelectionNumber == 0 || playerSelectionNumber == 1)
        {
            //This means the player model type is ATTACK.
            playerModelType_Text.text = "Offense";

        }
        else
        {
            //This means the player model type is DEFEND.
            playerModelType_Text.text = "Defense";

        }
    }

	public void OnSelectButtonClicked()
	{
		uI_Selection.SetActive(false);
		uI_AfterSelection.SetActive(true);
	}

	public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
	{
		// Check if both players are ready
        if (AllPlayersReady() && PhotonNetwork.CurrentRoom.PlayerCount == 2)
		{
			// Load the gameplay scene (only by the MasterClient to prevent double-load)
			if (PhotonNetwork.IsMasterClient)
			{
				PhotonNetwork.LoadLevel("Scene_Gameplay");
			}
		}
		if (AllPlayersReady() && PhotonNetwork.CurrentRoom.PlayerCount == 2)
		{
            waiting.SetActive(true);
            Invoke("StartMatchOnTimer", 20);
		}
	}

    private void StartMatchOnTimer()
    {
		// Load the gameplay scene (only by the MasterClient to prevent double-load)
		if (PhotonNetwork.IsMasterClient)
		{
			PhotonNetwork.LoadLevel("Scene_Gameplay");
		}
	}

	private bool AllPlayersReady()
	{
		foreach (Player player in PhotonNetwork.PlayerList)
		{
			if (player.CustomProperties.TryGetValue("IsReady", out object isReady))
			{
				if (!(bool)isReady)
					return false;
			}
			else
			{
				return false; // no property = not ready
			}
		}

		return true;
	}

	public void OnReSelectButtonClicked()
	{
		uI_Selection.SetActive(true);
		uI_AfterSelection.SetActive(false);

		ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
	{
		{ "IsReady", false }
	};
		PhotonNetwork.LocalPlayer.SetCustomProperties(props);
	}

	public void OnBattleButtonClicked()
    {
		ExitGames.Client.Photon.Hashtable playerSelectionProp = new ExitGames.Client.Photon.Hashtable
	{
		{ MultiplayerBeybladeGame.PLAYER_SELECTION_NUMBER, playerSelectionNumber },
		{ "IsReady", true } // ✅ add this
    };
		PhotonNetwork.LocalPlayer.SetCustomProperties(playerSelectionProp);

		waiting.SetActive(true);
    }

    public void OnBackButtonClicked()
    {
		Photon.Pun.PhotonNetwork.LoadLevel("Scene_Lobby");
    }

    #endregion

    #region Private Methods
    IEnumerator Rotate(Vector3 axis, Transform transformToRotate, float angle, float duration = 1.0f)
    {
        Quaternion originalRotation = transformToRotate.rotation;
        Quaternion finalRotation = transformToRotate.rotation * Quaternion.Euler(axis * angle);

        float elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            transformToRotate.rotation = Quaternion.Slerp(originalRotation, finalRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transformToRotate.rotation = finalRotation;

        next_Button.enabled = true;
        previous_Button.enabled = true;
    }
    #endregion
}
