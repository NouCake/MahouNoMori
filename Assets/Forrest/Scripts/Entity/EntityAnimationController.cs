using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAnimationController : MonoBehaviour {

    private Animator animator;

    private int animatorMovingId = Animator.StringToHash("Walking");
    private int animatorRunningId = Animator.StringToHash("Running");
    private bool moving;
    private bool running;
    private bool dead;

    private float lastHit;
    [SerializeField] private float HitRecoverTime = 1.0f;
    [SerializeField] public float DeathAnimationTime = 0.967f;
    [SerializeField] public float HitAnimationTime = 1.767f;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    public void PlayDeath() {
        dead = true;
        animator.Play("Death");
    }

    public void PlayHit() {
        if (dead) return;
        if(Time.time - lastHit > HitRecoverTime) {
            animator.Play("Hit", 0, 0.0f);
            lastHit = Time.time;
        }
    }

    public void PlayAttack() {
        if (dead) return;
        animator.Play("Attack", 0, 0.0f);
    }

    public void SetMoving(bool moving) {
        if(this.moving != moving) animator.SetBool(animatorMovingId, moving);
        this.moving = moving;
    }

    public void SetRunning(bool running) {
        if (this.running != running) animator.SetBool(animatorRunningId, moving);
        this.running = running;

    }

}
