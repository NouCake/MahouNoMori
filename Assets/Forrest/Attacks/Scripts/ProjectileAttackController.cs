using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttackController : AttackController {

    [SerializeField] private GameObject CastingFlash;
    [SerializeField] private GameObject HitFlash;
    [Space]
    [Header("PROJECTILE PATH")]
    public float speed = 15f;
    public float sideAngle = 25;
    public float upAngle = 20;

    private float randomUpAngle;
    private float randomSideAngle;
    public GameObject[] Detached;

    void Start() {
        flashEffect();
        newRandom();

    }

    public override void Initialize(HitInfo info) {
        this.info = info;
        transform.position = info.GetSource().transform.position;
        ParticleSystem buffPS = GetComponentInChildren<ParticleSystem>();
        Destroy(gameObject, buffPS.main.duration);
    }

    virtual protected void OnHit() {
        info.GetTarget().GetHitReceiver()?.Hit(info);
    }

    private void Update() {
        if (info == null || info.GetTarget() == null) {
            foreach (var detachedPrefab in Detached) {
                if (detachedPrefab != null) {
                    detachedPrefab.transform.parent = null;
                }
            }
            Destroy(gameObject);
            return;
        }

        Vector3 forward = (info.GetTargetPosition() - transform.position);
        Vector3 crossDirection = Vector3.Cross(forward, Vector3.up);
        Quaternion randomDeltaRotation = Quaternion.Euler(0, randomSideAngle, 0) * Quaternion.AngleAxis(randomUpAngle, crossDirection);
        Vector3 direction = randomDeltaRotation * (info.GetTargetPosition() - transform.position);

        float distanceThisFrame = Time.deltaTime * speed;

        if (direction.magnitude <= distanceThisFrame) {
            hitEffect();
            OnHit();
            return;
        }

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
        transform.rotation = Quaternion.LookRotation(direction);
    }



    private void hitEffect() {
        if (HitFlash != null) {
            var hitInstance = Instantiate(HitFlash, info.GetTargetPosition(), transform.rotation);
            var hitPs = hitInstance.GetComponent<ParticleSystem>();
            if (hitPs != null) {
                Destroy(hitInstance, hitPs.main.duration);
            } else {
                var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(hitInstance, hitPsParts.main.duration);
            }
        }
        foreach (var detachedPrefab in Detached) {
            if (detachedPrefab != null) {
                detachedPrefab.transform.parent = null;
            }
        }
        Destroy(gameObject);
    }

    private void newRandom() {
        randomUpAngle = Random.Range(0, upAngle);
        randomSideAngle = Random.Range(-sideAngle, sideAngle);
    }

    private void flashEffect() {
        if (CastingFlash != null) {
            var flashInstance = Instantiate(CastingFlash, transform.position, Quaternion.identity);
            flashInstance.transform.forward = gameObject.transform.forward;
            var flashPs = flashInstance.GetComponent<ParticleSystem>();
            if (flashPs != null) {
                Destroy(flashInstance, flashPs.main.duration);
            } else {
                var flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(flashInstance, flashPsParts.main.duration);
            }
        }
    }
}
