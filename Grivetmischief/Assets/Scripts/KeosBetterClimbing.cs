using easyInputs;
using UnityEngine;
using UnityEngine.Events;

public class KeosBetterClimbing : MonoBehaviour
{
    public Rigidbody Player;
    public EasyHand TriggerHand;

    public LayerMask ClimbLayer;
    public float GrabRange = 0.05f;
    public InteractionType type;

    [Header("Triggers")]
    public UnityEvent OnGrab;
    public UnityEvent OnRelease;

    [Header("TEST")]
    public bool TestGrab;

    //priv stuff

    Transform p;
    Vector3 vel;
    bool climb = false;
    Collider diddiyOil;

    [System.Flags]
    public enum InteractionType { None, Trigger, Grip }

    private void Update()
    {
        if (!CheckHandPressing() &!TestGrab)
            return;

        Collider[] c = Physics.OverlapSphere(transform.position, GrabRange, ClimbLayer);
        if (c.Length > 0 || diddiyOil)
        {
            if (!climb)
            {
                if (OnGrab != null)
                {
                    OnGrab.Invoke();
                }
                if (p) Destroy(p.gameObject);

                p = new GameObject("p").transform;
                p.transform.position = transform.position;
                p.SetParent(c[0].transform);

                c[0].TryGetComponent<Collider>(out Collider e);
                if (e)
                {
                    diddiyOil = e;
                    diddiyOil.enabled = false;
                }
                climb = true;
            }
            Player.linearVelocity = (p.position - transform.position) / Time.fixedDeltaTime;
        }
        else if (climb)
        {
            if (OnRelease != null)
            {
                OnRelease.Invoke();
            }
            if (p) Destroy(p.gameObject);
            p = null;
            if (diddiyOil)
            {
                diddiyOil.enabled = true;
            }
            climb = false;
        }
    }

    private bool CheckHandPressing() { return (EasyInputs.GetTriggerButtonDown(TriggerHand) && type == InteractionType.Trigger) || (EasyInputs.GetGripButtonDown(TriggerHand) && type == InteractionType.Grip); }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, GrabRange);
    }
}
