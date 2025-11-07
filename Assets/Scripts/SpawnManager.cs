using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviourPunCallbacks
{
	public GameObject[] playerPrefabs;
	public Transform[] spawnPositions;

	void Start()
	{
		object playerSelectionNumber;

		if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerBeybladeGame.PLAYER_SELECTION_NUMBER, out playerSelectionNumber))
		{
			Debug.Log("Player selection number is " + (int)playerSelectionNumber);

			// Pick spawn point based on player index (avoids overlap)
			int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
			Vector3 instantiatePosition = spawnPositions[playerIndex % spawnPositions.Length].position;

			PhotonNetwork.Instantiate(playerPrefabs[(int)playerSelectionNumber].name, instantiatePosition, Quaternion.identity);
		}
		else
		{
			Debug.LogWarning("No spinner selected! PLAYER_SELECTION_NUMBER not found.");
		}
	}
}