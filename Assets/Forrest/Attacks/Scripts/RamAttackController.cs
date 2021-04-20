using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamAttackController : AttackController {

    private Transform target;

    public override void Initialize(HitInfo info) {
        //base.Initialize(info);
        this.info = info;
        transform.position = info.GetSource().targetable.GetTargetPoint();
        transform.rotation = Quaternion.LookRotation(info.GetTargetPosition() - transform.position);

        target = transform.Find("Target");
        Destroy(gameObject, 1.0f);
    }

    private void Update() {
        float dist = Vector3.Distance(new Vector3(info.GetSource().targetable.GetTargetPoint().x, 0, info.GetSource().targetable.GetTargetPoint().z), new Vector3(target.position.x, 0, target.position.z));

        if(dist < 0.1) {
            info.GetTarget().GetHitReceiver()?.Hit(info);
            Destroy(gameObject);
        }
    }

}
