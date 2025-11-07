//using System.Collections;
//using UnityEngine;
//using Photon.Pun;

//public enum PowerUpType { SpeedUp = 0, Slow = 1 }

//[RequireComponent(typeof(Rigidbody))]
//public class PowerUpHandler : MonoBehaviourPun
//{
//    [Header("Refs")]
//    public Spinner spinner;                 // assign your Spinner component in Inspector
//    private Rigidbody rb;

//    [Header("Tuning")]
//    public float speedImpulse = 6f;         // instant forward kick when you get SpeedUp
//    public float speedDragMultiplier = 0.5f;// lower drag = faster (0.5 means 50% of normal)
//    public float slowDragMultiplier = 2.0f; // higher drag = slower (2x normal)

//    [Header("Optional Visuals (doesn't affect 'health' spinSpeed)")]
//    public float visualSpinBonus = 800f;    // add to visual spin while buff is active

//    private float _baseDrag;
//    private float _visualBonusActive = 0f;
//    private Coroutine _speedCR;
//    private Coroutine _slowCR;

//    void Awake()
//    {
//        rb = GetComponent<Rigidbody>();
//        _baseDrag = rb.drag;
//    }

//    void LateUpdate()
//    {
//        // apply temporary visual-only spin bonus safely
//        if (spinner && _visualBonusActive != 0f)
//        {
//            spinner.extraVisualSpin = _visualBonusActive; // see Spinner edit below
//        }
//        else if (spinner && spinner.extraVisualSpin != 0f)
//        {
//            spinner.extraVisualSpin = 0f;
//        }
//    }

//    [PunRPC]
//    public void RPC_ApplyPowerUp(int type, float amount, float duration)
//    {
//        var pType = (PowerUpType)type;

//        switch (pType)
//        {
//            case PowerUpType.SpeedUp:
//                if (_speedCR != null) StopCoroutine(_speedCR);
//                _speedCR = StartCoroutine(DoSpeedUp(amount, duration));
//                break;

//            case PowerUpType.Slow:
//                if (_slowCR != null) StopCoroutine(_slowCR);
//                _slowCR = StartCoroutine(DoSlow(amount, duration));
//                break;
//        }
//    }

//    private IEnumerator DoSpeedUp(float amount, float duration)
//    {
//        // amount is a drag multiplier (usually 0.5) — clamp to sane range
//        amount = Mathf.Clamp(amount, 0.1f, 1f);

//        // physics boost
//        float oldDrag = rb.drag;
//        rb.drag = _baseDrag * amount;
//        rb.AddForce(transform.forward * speedImpulse, ForceMode.VelocityChange);

//        // visual spin candy
//        _visualBonusActive = visualSpinBonus;

//        yield return new WaitForSeconds(duration);

//        rb.drag = oldDrag;
//        _visualBonusActive = 0f;
//    }

//    private IEnumerator DoSlow(float amount, float duration)
//    {
//        // amount is a drag multiplier (usually 2.0) — clamp to sane range
//        amount = Mathf.Clamp(amount, 1f, 5f);

//        float oldDrag = rb.drag;
//        rb.drag = _baseDrag * amount;

//        // optional: tiny visual penalty (don’t touch spinner.spinSpeed health)
//        _visualBonusActive = -visualSpinBonus * 0.4f;

//        yield return new WaitForSeconds(duration);

//        rb.drag = oldDrag;
//        _visualBonusActive = 0f;
//    }
//}
