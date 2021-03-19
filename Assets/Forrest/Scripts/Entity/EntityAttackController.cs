using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAttackController : MonoBehaviour {

    private Targetable target;

    private AttackInfo currentAttack;
    private float attackTimer;
    private bool hasAttacked;

    private void Update() {
        if(attackTimer > 0) {
            attackTimer -= Time.deltaTime;
            if(!hasAttacked && isHitting()) {
                hasAttacked = true;
                OnAttack();
            }
        }
    }

    private void OnAttack() {
        target.GetHitReceiver().Hit();
        spawnParticles();
    }

    private void spawnParticles() {
        if(currentAttack.AttackHitParticles != null) {
            GameObject go = Instantiate(currentAttack.AttackHitParticles, target.GetTargetPoint(), transform.rotation);
            ParticleSystem goPS = go.GetComponent<ParticleSystem>();
            Destroy(go, goPS.main.duration);
        }
    }

    public void StartAttack(Targetable target, AttackInfo attack) {
        SetTarget(target);
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
        return IsAttacking() && attackTimer < currentAttack.AttackTime - currentAttack.AttackChargeTime;
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
