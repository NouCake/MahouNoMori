using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitInfo {

    private EntityController source;

    private Targetable target;
    private Vector3 targetPos;


    public HitInfo(EntityController source, Targetable target) {
        this.source = source;
        this.target = target;
    }

    public HitInfo(EntityController source, Vector3 TargetPosition) {
        this.source = source;
        this.targetPos = TargetPosition;
    }

    public EntityController GetSource() {
        return source;
    }

    public Targetable GetTarget() {
        return target;
    }

    public Vector3 GetTargetPosition() {
        if (target != null) return target.GetTargetPoint();
        return targetPos;
    }

}
