using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SensorToolkit;

[RequireComponent(typeof(EntityAttackController))]
[RequireComponent(typeof(EntityMovementController))]
[RequireComponent(typeof(RangeSensor))]
public class BoarEntityController : EntityController {

    [SerializeField] private string state;
    [SerializeField] private float idleTimer;

    private RangeSensor sensor;

    private Targetable target;

    [SerializeField] private AttackInfo baseAttack;

    protected override void Awake() {
        base.Awake();

        sensor = GetComponent<RangeSensor>();
    }

    protected override void Start() {
        base.Start();
        state = "Roaming";
    }

    protected override void Update() {
        base.Update();

        if (IsStunned()) {
            return;
        }

        if (state == "Roaming") {
            updateRoaming();
            updateSensor();
        } else if (state == "Attacking") {
            updateAttacking();
            updateSensor();
        }
    }

    public override void Hit(HitInfo info) {
        base.Hit(info);
        if(state == "Roaming") {
            movementController.MoveToTarget(new Vector3(info.GetSource().transform.position.x, 0, info.GetSource().transform.position.z), true);
            idleTimer = 0;
        }
    }

    protected override void OnHitAnimation() {
        base.OnHitAnimation();
        if(state != "Roaming") Stun(animationController.HitAnimationTime);
    }

    private void updateAttacking() {
        float distToTarget; ;
        if (target == null || target.GetHealthController()?.GetHealth() <= 0 || (distToTarget = Vector3.Distance(targetable.GetTargetPoint(), target.GetTargetPoint())) > sensor.SensorRange + 1) {
            state = "Roaming";
            target = null;
            movementController.MovementSpeedModifier = 1;
            animationController.SetRunning(false);
            return;
        }
        movementController.MovementSpeedModifier = 3;

        if (distToTarget > 3f && !attackController.IsAttacking()) {
            Vector3 targetDir = target.transform.position - transform.position;
            animationController.SetRunning(true);
            movementController.MoveIntoDirection(targetDir);
            return;
        }

        if (!attackController.IsAttacking()) {
            animationController.PlayAttack();
            movementController.StopMovement();
            attackController.StartAttack(target, baseAttack);
        }

    }

    private void updateSensor() {
        List<Targetable> targets = sensor.GetDetectedByComponent<Targetable>();

        float minDist = -1;

        Targetable nearest = null;
        foreach (Targetable target in targets) {
            if (target.tag != "Player" && target.tag != "Boar") continue;
            if (target == targetable) continue;
            if (minDist >= 0 && Vector3.Distance(targetable.GetTargetPoint(), target.GetTargetPoint()) > minDist) continue;

            minDist = Vector3.Distance(targetable.GetTargetPoint(), target.GetTargetPoint());
            nearest = target;
        }
        if (nearest != null && nearest != target) {
            this.target = nearest;
            state = "Attacking";
            movementController.StopMovement();
        }
    }


    private void updateRoaming() {
        if (Health <= 0) return;

        if (idleTimer > 0) {
            idleTimer -= Time.deltaTime;
            return;
        }

        if (!movementController.IsMoving()) {

            if (Random.value > 0.9) {
                idleTimer = Random.Range(2, 5);
                return;
            }

            Vector3 targetPos = Matrix4x4.Rotate(Quaternion.AngleAxis(Random.Range(-60, 60), Vector3.up)) * new Vector3(transform.forward.x, 0, transform.forward.z);
            targetPos *= Random.Range(1, 3);
            movementController.MoveToTarget(transform.position + targetPos, true);
        }
    }

}
