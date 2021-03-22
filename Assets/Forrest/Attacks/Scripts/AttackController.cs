using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour {

    protected HitInfo info;

    virtual public void Initialize(HitInfo info) {
        this.info = info;
        transform.position = info.GetTargetPosition();
        ParticleSystem buffPS = GetComponentInChildren<ParticleSystem>();
        Destroy(gameObject, buffPS.main.duration);
        info.GetTarget().GetHitReceiver()?.Hit(info);
    }

    private void OnDestroy() {
    }

}
