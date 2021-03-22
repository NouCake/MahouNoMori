using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityController))]
public class EntityAttackController : MonoBehaviour {

    private EntityController controller;

    private Targetable target;
    private Vector3 targetPos;

    private AttackInfo currentAttack;
    [SerializeField] private float attackTimer;
    private bool hasAttacked;


    public bool charging;
    public bool hitting;

    private void Awake() {
        controller = GetComponent<EntityController>();
    }

    private void Update() {
        if(attackTimer > 0) {
            attackTimer -= Time.deltaTime;
            if(!hasAttacked && isHitting()) {
                hasAttacked = true;
                OnAttack();
            }
        }

        charging = isCharging();
        hitting = isHitting();
    }

    private void OnAttack() {
        HitInfo info;
        if (target == null)
            info = new HitInfo(controller, targetPos);
        else
            info = new HitInfo(controller, target);

        Instantiate(currentAttack.AttackController).Initialize(info);
    }

    public void StartAttack(Targetable target, AttackInfo attack) {
        if (controller.GetMana() < attack.ManaCost) return;
        controller.AddMana(-attack.ManaCost);

        SetTarget(target);
        targetPos = Vector3.zero;
        currentAttack = attack;
        attackTimer = currentAttack.AttackTime;
        hasAttacked = false;
    }

    public void StartAttack(Vector3 target, AttackInfo attack) {
        if (controller.GetMana() < attack.ManaCost) return;
        controller.AddMana(-attack.ManaCost);

        this.target = null;
        this.targetPos = target;
        currentAttack = attack;
        attackTimer = currentAttack.AttackTime;
        hasAttacked = false;
    }



    public bool TryTumble() {
        if (isCharging()) {
            attackTimer = 0;
            return true;
        }
        return false;
    }

    private bool isCharging() {
        return IsAttacking() && attackTimer > currentAttack.AttackTime - currentAttack.AttackChargeTime;
    }

    private bool isHitting() {
        return IsAttacking() && attackTimer < currentAttack.AttackTime - currentAttack.AttackHitTime;
    }

    public bool IsAttacking() {
        return currentAttack != null && attackTimer > 0;
    }

    public void SetTarget(Targetable target) {
        if (this.target = target) return;
        attackTimer = 0;
        this.target = target;
    }

}
