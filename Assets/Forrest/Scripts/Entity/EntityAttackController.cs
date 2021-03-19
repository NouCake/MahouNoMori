using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityController))]
public class EntityAttackController : MonoBehaviour {

    private EntityController controller;

    private Targetable target;

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
        if(currentAttack.AttackType == "Melee") {
            target.GetHitReceiver().Hit();
            spawnParticles();
        } else if(currentAttack.AttackType == "Ranged") {
            ProjectileAttackInfo info = (ProjectileAttackInfo)currentAttack;
            GameObject p = Instantiate(info.Projectile, transform.position, transform.rotation);
            p.GetComponent<TargetProjectile>().UpdateTarget(target, Vector3.zero);
        }
    }

    private void spawnParticles() {
        if(currentAttack.AttackHitParticles != null) {
            GameObject go = Instantiate(currentAttack.AttackHitParticles, target.GetTargetPoint(), transform.rotation);
            ParticleSystem goPS = go.GetComponent<ParticleSystem>();
            Destroy(go, goPS.main.duration);
        }
    }

    public void StartAttack(Targetable target, AttackInfo attack) {
        if (controller.GetMana() < attack.ManaCost) return;
        controller.AddMana(-attack.ManaCost);

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
