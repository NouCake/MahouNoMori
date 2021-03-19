using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SensorToolkit;

public class TargetSelector : MonoBehaviour {

    private RangeSensor sensor;

    private Targetable lastTarget;

    public static Targetable Target {
        get => getTarget();
    }
    private static Targetable getTarget() {
        _instance.sensor.Pulse();
        Targetable target = _instance.sensor.GetNearestByComponent<Targetable>();

        if (target == null) return null;
        if (target.tag == "Player") return null;
        if (Vector3.Distance(target.transform.position, _instance.transform.position) > _instance.sensor.SensorRange) return null;
        if (!target.IsCurrentlyTargetable()) return null;
        return target;
    }
    private static TargetSelector _instance;

    void Awake() {
        _instance = this;
        sensor = GetComponent<RangeSensor>();
    }

    private void moveSelector(Vector3 newPos) {
        transform.position = newPos;
        
        Targetable target = getTarget();

        if (target == null && lastTarget != null) {
            removeOutline(lastTarget);
            lastTarget = null;
        }
        if(target != null && target != lastTarget) {
            if(lastTarget != null) removeOutline(lastTarget);
            addOutline(target);
            lastTarget = target;
        }

    }

    private void removeOutline(Targetable target) {
        Transform outline = target.transform.Find("Outline");
        if(outline != null) outline.gameObject.SetActive(false);
    }

    private void addOutline(Targetable target) {
        Transform outline = target.transform.Find("Outline");
        if (outline != null) outline.gameObject.SetActive(true);
    }

    void Update() {
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit info;
        int layermask = LayerMask.GetMask("Default") | LayerMask.GetMask("Terrain");
        if (Physics.Raycast(r, out info, 1000, layermask)) {
            moveSelector(info.point);
        }
    }

}
