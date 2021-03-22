using SensorToolkit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAttackController : AttackController {

    private RangeSensor sensor;

    private void Awake() {
        sensor = GetComponent<RangeSensor>();
    }

    public override void Initialize(HitInfo info) {
        //base.Initialize(info);
        transform.position = info.GetTargetPosition();
        putToGround();
        Destroy(gameObject, GetComponentInChildren<ParticleSystem>().main.duration);

        sensor.Pulse();
        List<Targetable> targets = sensor.GetDetectedByComponent<Targetable>();
        foreach (Targetable target in targets) {
            target.GetHitReceiver()?.Hit(info);
        }
        //Debug.Break();
    }



    private void putToGround() {
        //Vector3 ccOffset = Vector3.up * (characterController.height * 0.5f) - characterController.center;
        Vector3 ccOffset = Vector3.zero;
        Ray r = new Ray(transform.position - ccOffset + Vector3.up * 0.1f, Vector3.down);
        RaycastHit info;
        int layermask = LayerMask.GetMask("Terrain");
        if (Physics.Raycast(r, out info, 2, layermask)) {
            transform.position = info.point + ccOffset;
        }
    }

    private void OnDestroy() {
    }

}
