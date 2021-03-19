using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetable : MonoBehaviour {

    public MonoBehaviour HitReceiverScript;
    public MonoBehaviour HealthControllerScript;
    private HitReceiver receiver;
    private StatInfo health;
    [SerializeField] private Vector3 center;

    private bool targetable;
    
    private void Awake() {
        if (HitReceiverScript is HitReceiver) receiver = (HitReceiver)HitReceiverScript;
        if (HealthControllerScript is StatInfo) health = (StatInfo)HealthControllerScript;
    }

    private void Start() {
        if (center == Vector3.zero) center = getCenterFromCollider();
        targetable = true;
    }

    private Vector3 getCenterFromCollider() {
        Collider collider = GetComponent<Collider>();
        if(collider is CharacterController) {
            return ((CharacterController)collider).center;
        }
        if (collider is SphereCollider) {
            return ((SphereCollider)collider).center;
        }
        if (collider is CapsuleCollider) {
            return ((CapsuleCollider)collider).center;
        }
        throw new System.Exception("Unsupported Collider:" + collider.GetType() + " : " + collider);
    }

    /*
    private HitReceiver getHitReceiverFromController() {
        BoarController controler = GetComponent<BoarController>();
        if (controler != null) return controler;

        return null;
    }
    */

    public Vector3 GetTargetPoint() {
        return transform.position + center;
    }

    public HitReceiver GetHitReceiver() {
        return receiver;
    }

    public StatInfo GetHealthController() {
        return health;
    }

    public bool IsCurrentlyTargetable() {
        return targetable;
    }

    public void SetTargetable(bool targetable) {
        this.targetable = targetable;
    } 

}
