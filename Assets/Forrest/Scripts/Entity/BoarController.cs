using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Targetable))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class BoarController : MonoBehaviour, HitReceiver, StatInfo {

    private enum STATE {
        IDLE,
        WALK,
        BATTLE
    }

    private int health = 10;

    [SerializeField]
    private float moveSpeed = 2;

    private Animator animator;
    private CharacterController charcon;
    private Targetable targetable;

    private UIHealthbar healthbar;

    [SerializeField] private GameObject DeathParticles;

    [SerializeField] private float timeSinceLastStateChange;
    [SerializeField] private STATE state = STATE.IDLE;
    private List<StateBehaviour> states;
    private StateBehaviour currentBehaviour;
    private Vector3 posInLastFrame;

    private void Awake() {
        charcon = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        targetable = GetComponent<Targetable>();
    }

    void Start() {
        states = new List<StateBehaviour>();
        states.Add(new IdleBehaviour(this));
        states.Add(new RandomWalkBehaviour(this));
        setState(STATE.IDLE);
        posInLastFrame = transform.position;
    }

    void Update() {
        timeSinceLastStateChange += Time.deltaTime;
        currentBehaviour?.update();
    }

    private void LateUpdate() {
        if(health <= 0) {
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            if (info.IsName("Death") && info.normalizedTime >= 1) {
                Destroy(gameObject);

                if (DeathParticles != null) {
                    var hitInstance = Instantiate(DeathParticles, transform.position, transform.rotation);
                    var hitPs = hitInstance.GetComponent<ParticleSystem>();
                    if (hitPs != null) {
                        Destroy(hitInstance, hitPs.main.duration);
                    } else {
                        var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                        Destroy(hitInstance, hitPsParts.main.duration);
                    }
                }
            }
        }
    }

    private void moveToTargetPosition(Vector3 target) {
        Vector3 direction = (target - transform.position).normalized;
        direction = new Vector3(direction.x, 0, direction.z);
        charcon.Move(direction * moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void putToGround() {
        Vector3 ccOffset = Vector3.up * (charcon.height * 0.5f) - charcon.center;
        Ray r = new Ray(transform.position - ccOffset, Vector3.down);
        RaycastHit info;
        int layermask = LayerMask.GetMask("Terrain");
        if (Physics.Raycast(r, out info, 2, layermask)) {
            transform.position = info.point + ccOffset;
        }
    }

    private void setState(STATE state) {
        this.state = state;
        timeSinceLastStateChange = 0;
        currentBehaviour = getBehaviourFromState(state);
        currentBehaviour?.enter();
    }

    private StateBehaviour getBehaviourFromState(STATE state) {
        foreach (StateBehaviour sb in states) {
            if (sb.state == state) return sb;
        }
        return null;
    }

    public void Hit() {
        setState(STATE.IDLE);
        health--;
        if(health <= 0) {
            targetable.SetTargetable(false);
            animator.Play("Death", 0, 0.0f);
        } else {
            animator.Play("hit", 0, 0.0f);
        }

        if (healthbar == null) healthbar = HUDController.Instance.CreateHealthbar(targetable);
        healthbar.SetHealthPercent(GetHealth() / GetMaxHealth());
    }


    private void OnDestroy() {
        if (healthbar != null) Destroy(healthbar.gameObject);
    }

    public int GetHealth() {
        return health;
    }

    public int GetMaxHealth() {
        return 10;
    }

    public int GetLevel() {
        throw new System.NotImplementedException();
    }

    private class RandomWalkBehaviour : StateBehaviour {

        Vector3 targetPos;
        float targetTime = 0.0f;

        public RandomWalkBehaviour(BoarController boar) : base(boar, STATE.WALK) {

        }
        
        public override void enter() {
            targetPos = Matrix4x4.Rotate(Quaternion.AngleAxis(Random.value * 90 - 45, Vector3.up)) * new Vector3(boar.transform.forward.x, 0, boar.transform.forward.z);
            targetPos *= Random.Range(5, 10);
            targetTime = targetPos.magnitude / boar.moveSpeed;
            targetPos += boar.transform.position;
            boar.animator.SetBool("Walking", true);
        }

        public override void update() {
            targetTime -= Time.deltaTime;
            float distanceToTarget = Vector3.Distance(boar.transform.position, targetPos);
            if (targetTime <= 0 || distanceToTarget < 0.1f) {
                gambleNextState();
            }
            boar.moveToTargetPosition(targetPos);
            boar.putToGround();
        }

        private void gambleNextState() {
            boar.animator.SetBool("Walking", false);
            float rnd = Random.value;
            if(rnd > 0.5) {
                boar.setState(STATE.WALK);
            }
            boar.setState(STATE.IDLE);
        }

    }

    private class IdleBehaviour : StateBehaviour {
        private float nextActionTime = 0;

        public IdleBehaviour(BoarController boar) : base(boar, STATE.IDLE) { }

        public override void enter() {
            nextActionTime = Random.Range(1, 5);
        }

        public override void update() {
            nextActionTime -= Time.deltaTime;
            if (nextActionTime < 0) {
                if (Random.value > 0.8) {
                    boar.animator.SetTrigger("IDLE_SPECIAL");
                }
                if(Random.value > 0.3) {
                    boar.setState(STATE.WALK);
                } else {
                    boar.setState(STATE.IDLE);
                }
            }
        }
    }

    private abstract class StateBehaviour {
        readonly protected BoarController boar;
        readonly public STATE state;
        protected StateBehaviour(BoarController boar, STATE state) {
            this.boar = boar;
            this.state = state;
        }

        public abstract void enter();
        public abstract void update();

    }


}
