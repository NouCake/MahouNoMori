using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SensorToolkit;

public class TargetSelector : MonoBehaviour {

    private RangeSensor sensor;

    private Targetable lastTarget;

    [SerializeField] private int layermask;
    [SerializeField] private Projector projector;

    public void SetProjectionSprite(bool sprite) {
        if (!sprite) {
            projector.gameObject.SetActive(false);
            return;
        }

        projector.gameObject.SetActive(true);
        //projector.material.SetTexture("Cookie", sprite.texture);
    }

    public static Targetable Target {
        get => getTarget();
    }
    private static Targetable getTarget() {
        _instance.sensor.Pulse();
        List<Targetable> targets = _instance.sensor.GetDetectedByComponent<Targetable>();


        Targetable closest = null;
        foreach(Targetable target in targets) {
            if (target == null) continue;
            if (target.tag == "Player") continue;
            //if (Vector3.Distance(target.transform.position, _instance.transform.position) > _instance.sensor.SensorRange) return null;
            if (!target.IsCurrentlyTargetable()) continue;
            if (closest != null && Vector3.Distance(_instance.transform.position, target.GetTargetPoint()) > Vector3.Distance(_instance.transform.position, closest.GetTargetPoint())) continue;
            closest = target;
        }

        return closest;
    }
    private static TargetSelector _instance;

    void Awake() {
        _instance = this;
        sensor = GetComponent<RangeSensor>();
        layermask = LayerMask.GetMask("Default") | LayerMask.GetMask("Terrain"); 
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
        if (Physics.Raycast(r, out info, 1000, layermask)) {
            moveSelector(info.point);
        }
    }

    public void SetLayerMask(int layermask) {
        this.layermask = layermask;
    }

}
