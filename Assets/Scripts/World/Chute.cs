using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Chute : MonoBehaviour
{
    [SerializeField] private Chute nextChute;
    [SerializeField] private ColliderEvents teleportTrigger;
    [SerializeField] private ColliderEvents nudgeTrigger;
    [SerializeField] private Vector3 localExitVelocity = Vector3.up;
    [SerializeField] private Vector3 localExitNudge = Vector3.zero;
    [SerializeField] private float releaseDelay = 1f;
    [SerializeField] private float maxReleasedTimeInTrigger = 1f;
    private List<IChuteAble> doNotTeleport;
    CancellationTokenSource cancellationTokenSource;

    void Awake()
    {
        doNotTeleport = new List<IChuteAble>();
        teleportTrigger.onTriggerEnter.AddListener((collider) => ExecuteIfChuteAble(collider.gameObject, OnTeleportTriggerEnter));
        teleportTrigger.onTriggerExit.AddListener((collider) => ExecuteIfChuteAble(collider.gameObject, OnTeleportTriggerExit));
        nudgeTrigger.onTriggerExit.AddListener((collider) => ExecuteIfChuteAble(collider.gameObject, OnNudgeTriggerExit));
        cancellationTokenSource = new CancellationTokenSource();
    }

    delegate void TriggerDelegate(IChuteAble chuteAble);
    private void ExecuteIfChuteAble(GameObject obj, TriggerDelegate triggerDelegate)
    {
        if (obj.TryGetComponent<IChuteAble>(out IChuteAble chuteAble))
        {
            triggerDelegate(chuteAble);
        }
    }

    void OnDestroy()
    {
        cancellationTokenSource.Cancel();
    }

    private void OnNudgeTriggerExit(IChuteAble chuteAble)
    {
        chuteAble.AddVelocity(transform.TransformVector(localExitNudge));    
    }

    private void OnTeleportTriggerEnter(IChuteAble chuteAble)
    {
        if (!doNotTeleport.Contains(chuteAble))
        {
            nextChute.TeleportTo(chuteAble);
        }
    }

    void OnTeleportTriggerExit(IChuteAble chuteAble)
    {
        doNotTeleport.Remove(chuteAble);
    }

    public void TeleportTo(IChuteAble chuteAble)
    {
        if (!doNotTeleport.Contains(chuteAble))
        {
            doNotTeleport.Add(chuteAble);
            chuteAble.SetFrozen(true);
            chuteAble.SetPosition(teleportTrigger.transform.position);
            chuteAble.SetVelocity(Vector3.zero);
            UnfreezeInTime(chuteAble, releaseDelay, cancellationTokenSource.Token);
        }
    }

    private async void UnfreezeInTime(IChuteAble chuteAble, float seconds, CancellationToken cancellationToken)
    {
        await Task.Delay(Mathf.FloorToInt(seconds * 1000));
        if (!cancellationToken.IsCancellationRequested)
        {
            chuteAble.SetFrozen(false);
            chuteAble.SetVelocity(transform.TransformVector(localExitVelocity));

            TeleportInTimeIfStillInTrigger(chuteAble, maxReleasedTimeInTrigger);
        }
    }

    /// <summary>
    /// Teleport the chuteAble to the nextChute if it hasn't left the teleportTrigger 
    ///  (still in doNotTeleport) after some time (seconds).
    /// </summary>
    private async void TeleportInTimeIfStillInTrigger(IChuteAble chuteAble, float seconds)
    {
        await Task.Delay(Mathf.FloorToInt(seconds * 1000));
        if (doNotTeleport.Contains(chuteAble))
        {
            nextChute.TeleportTo(chuteAble);
        }
    }
}
