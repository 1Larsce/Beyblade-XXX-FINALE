//using UnityEngine;
//using Photon.Pun;
//using System.Collections;

//public class PowerUpPickup : MonoBehaviourPun
//{
//    public PowerUpType powerUpType = PowerUpType.SpeedUp;

//    [Header("Effect")]
//    public float duration = 5f;
//    // For SpeedUp use < 1 (e.g., 0.5 = half drag = faster). For Slow use > 1 (e.g., 2.0 = double drag = slower)
//    public float amount = 0.5f;

//    [Header("Respawn")]
//    public bool respawn = true;
//    public float respawnDelay = 8f;

//    private Collider _col;
//    private Renderer _rend;

//    void Awake()
//    {
//        _col = GetComponent<Collider>();
//        _rend = GetComponentInChildren<Renderer>();
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        if (!other.CompareTag("Player")) return;

//        var targetPV = other.GetComponent<PhotonView>();
//        if (targetPV == null) return;

//        // Let the local owner trigger the effect for everyone
//        if (targetPV.IsMine)
//        {
//            targetPV.RPC("RPC_ApplyPowerUp", RpcTarget.All, (int)powerUpType, amount, duration);
//        }

//        // Hide/respawn the pickup
//        if (respawn)
//        {
//            StartCoroutine(RespawnRoutine());
//        }
//        else
//        {
//            if (photonView && photonView.IsMine)
//                PhotonNetwork.Destroy(gameObject);
//            else
//                gameObject.SetActive(false);
//        }
//    }

//    private IEnumerator RespawnRoutine()
//    {
//        _col.enabled = false;
//        if (_rend) _rend.enabled = false;

//        yield return new WaitForSeconds(respawnDelay);

//        _col.enabled = true;
//        if (_rend) _rend.enabled = true;
//    }
//}
