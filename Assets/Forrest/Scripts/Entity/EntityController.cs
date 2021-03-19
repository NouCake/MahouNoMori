using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Targetable))]
public class EntityController : MonoBehaviour, StatInfo, HitReceiver {

    protected Targetable targetable;
    protected CharacterController characterController;

    protected EntityAnimationController animationController;
    protected EntityMovementController movementController;

    public GameObject DeathParticles;

    public bool ShowHealthbar;
    private UIHealthbar healthbar;


    private float lastHit;
    [SerializeField] private float HitImmuneTime = 2.0f;

    //StatInfo
    [SerializeField] protected int Level;
    [SerializeField] protected int Health;
    private int MaxHealth;

    private float deathTimer;
    private float stunTimer;


    public void Stun(float time) {
        if (stunTimer > time) Debug.Log("Stuntime was shortend!" + gameObject);
        stunTimer = time;
    }

    //Virtual
    protected virtual void Awake() {
        targetable = GetComponent<Targetable>();
        animationController = GetComponent<EntityAnimationController>();
        movementController = GetComponent<EntityMovementController>();
    }

    protected virtual void Start() {
        MaxHealth = Health;
        if (ShowHealthbar) {
            createHealthbar();
        }
    }

    virtual public void Hit() {
        Health--;
        if (ShowHealthbar) {
            if (healthbar == null) createHealthbar();
            healthbar.gameObject.SetActive(true);
        }


        if (Time.time - lastHit > HitImmuneTime) {
            OnHitAnimation();
        }

        if (Health <= 0) {
            StartDeath();
        }
    }

    protected virtual void Update() {
        updateStunTimer();
        updateDeathTimer();
    }

    virtual protected void OnHitAnimation() {
        lastHit = Time.time;
        animationController?.PlayHit();
    }

    virtual protected void OnDeath() {
        PlayDeathParticles();
        Destroy(gameObject);
    }

    //Getter
    protected bool IsStunned() {
        return stunTimer > 0;
    }

    public int GetLevel() {
        return Level;
    }

    public int GetHealth() {
        return Health;
    }

    public int GetMaxHealth() {
        return MaxHealth;
    }

    //private methods
    private void updateStunTimer() {
        if(stunTimer > 0) {
            stunTimer -= Time.deltaTime;
            movementController?.StopMovement();
        }
    }

    private void updateDeathTimer() {
        if(deathTimer > 0) {
            movementController?.StopMovement();
            deathTimer -= Time.deltaTime;
            if (deathTimer <= 0) OnDeath();
        }
    }

    private void createHealthbar() {
        healthbar = HUDController.Instance.CreateHealthbar(targetable);
        healthbar.gameObject.SetActive(false);

    }

    private void StartDeath() {
        if (deathTimer > 0) return;

        if(animationController != null) {
            animationController.PlayDeath();
            deathTimer = animationController.DeathAnimationTime;
            Stun(deathTimer);
        } else {
            OnDeath();
        }
    }

    private void PlayDeathParticles() {
        if (DeathParticles != null) {
            GameObject go = Instantiate(DeathParticles, transform.position, transform.rotation);
            ParticleSystem goPS = go.GetComponent<ParticleSystem>();
            Destroy(go, goPS.main.duration);
        }
    }

    private void OnDestroy() {
        if (healthbar != null) Destroy(healthbar.gameObject);
    }

}
