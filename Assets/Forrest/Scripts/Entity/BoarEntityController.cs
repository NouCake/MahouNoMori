using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SensorToolkit;

[RequireComponent(typeof(EntityMovementController))]
[RequireComponent(typeof(RangeSensor))]
public class BoarEntityController : EntityController {

    [SerializeField] private string state;
    [SerializeField] private float idleTimer;


    [SerializeField] private Targetable target;
    private float attackTimer;
    [SerializeField] private GameObject AttackParticles;
    [SerializeField] private float AttackParticleTime = 0.4f;

    private RangeSensor sensor;


    protected override void Awake() {
        base.Awake();
        movementController = GetComponent<EntityMovementController>();
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

    public override void Hit() {
        base.Hit();

    }

    protected override void OnHitAnimation() {
        if (attackTimer > animationController.AttackAnimationTime - AttackParticleTime) return;
        base.OnHitAnimation();
        Stun(animationController.HitAnimationTime);
        attackTimer = 0;
    }

    private void updateAttacking() {
        float distToTarget; ;
        if (target == null || target.GetHealthController()?.GetHealth() <= 0 || (distToTarget = Vector3.Distance(targetable.GetTargetPoint(), target.GetTargetPoint())) > sensor.SensorRange + 1) {
            state = "Roaming";
            target = null;
            movementController.MovementSpeedModifier = 1;
            return;
        }
        movementController.MovementSpeedModifier = 3;

        animationController.SetMoving(movementController.IsMoving());
        animationController.SetRunning(movementController.IsMoving());
        if (distToTarget > 3f && attackTimer <= 0) {

            Vector3 targetPoint = target.GetTargetPoint() - (target.GetTargetPoint() - targetable.GetTargetPoint()).normalized * 1.5f;
            movementController.MoveToTarget(targetPoint);

            return;
        }

        if (attackTimer > 0) {

            if (attackTimer > AttackParticleTime && attackTimer - Time.deltaTime < AttackParticleTime) {
                GameObject go = Instantiate(AttackParticles, target.GetTargetPoint(), target.transform.rotation);
                ParticleSystem goPS = go.GetComponent<ParticleSystem>();
                Destroy(go, goPS.main.duration);
                target.GetHitReceiver()?.Hit();
            }
            attackTimer -= Time.deltaTime;

        } else {
            animationController.PlayAttack();
            attackTimer = animationController.AttackAnimationTime;
            movementController.StopMovement(attackTimer);
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
            attackTimer = 0;
        }
    }


    private void updateRoaming() {
        if (Health <= 0) return;
        animationController.SetMoving(movementController.IsMoving());

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
            movementController.MoveToTarget(transform.position + targetPos);
        }
    }

}
