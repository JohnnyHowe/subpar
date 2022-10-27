using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Chute : MonoBehaviour
{
    [SerializeField] private Chute nextChute;
    [SerializeField] private ColliderEvents trigger;
    [SerializeField] private Vector3 localExitVelocity = Vector3.up;
    [SerializeField] private float releaseDelay = 1f;
    private List<IChuteAble> doNotTeleport;

    void Awake()
    {
        doNotTeleport = new List<IChuteAble>();
        trigger.onTriggerEnter.AddListener((c) => OnTriggerEnter(c));
        trigger.onTriggerExit.AddListener((c) => OnTriggerExit(c));
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.TryGetComponent<IChuteAble>(out IChuteAble chuteAble))
        {
            if (!doNotTeleport.Contains(chuteAble))
            {
                nextChute.TeleportTo(chuteAble);
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.TryGetComponent<IChuteAble>(out IChuteAble chuteAble))
        {
            doNotTeleport.Remove(chuteAble);
        }
    }

    public void TeleportTo(IChuteAble chuteAble)
    {
        if (!doNotTeleport.Contains(chuteAble))
        {
            doNotTeleport.Add(chuteAble);
            chuteAble.SetFrozen(true);
            chuteAble.SetPosition(trigger.transform.position);
            chuteAble.SetVelocity(Vector3.zero);
            UnfreezeInTime(chuteAble, releaseDelay);
        }
    }

    private async void UnfreezeInTime(IChuteAble chuteAble, float seconds)
    {
        await Task.Delay(Mathf.FloorToInt(seconds * 1000));
        chuteAble.SetFrozen(false);
        chuteAble.SetVelocity(transform.TransformVector(localExitVelocity));
    }
}
