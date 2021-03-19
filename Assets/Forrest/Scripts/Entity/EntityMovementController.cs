using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EntityMovementController : MonoBehaviour {

    private readonly float TARGET_EPSILON = 0.1f;

    private CharacterController characterController;

    [SerializeField] public float MovementSpeed = 10;
    [SerializeField] public float MovementSpeedModifier = 1;

    private EntityAnimationController animationController;

    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private float moveTimer;
    private float stopTime;
    [SerializeField] private bool moving;
    private bool perciceMovement;

    public float godPls;

    private void Awake() {
        characterController = GetComponent<CharacterController>();
        animationController = GetComponent<EntityAnimationController>();
    }

    public void StopMovement() {
        StopMovement(0);
    }

    public void StopMovement(float time) {
        animationController?.SetMoving(false);
        moving = false;
        targetPosition = transform.position;
        stopTime = time;
        moveTimer = 0.0f;
    }

    public void MoveToTarget(Vector3 targetPos) {
        MoveToTarget(targetPos, false);
    }

    public void MoveToTarget(Vector3 targetPos, bool withExitTime) {
        if(withExitTime) moveTimer = Vector3.Distance(transform.position, targetPos) / (MovementSpeed * MovementSpeedModifier);
        this.targetPosition = targetPos;
        moving = true;
        perciceMovement = true;
    }

    public void MoveIntoDirection(Vector3 direction) {
        direction = new Vector3(direction.x, 0, direction.z);
        MoveToTarget(transform.position + direction.normalized * TARGET_EPSILON * 2, true);
        perciceMovement = false;
    }


    private void Update() {
        if(stopTime > 0) {
            stopTime -= Time.deltaTime;
            return;
        }

        updateMovement();
        putToGround();
    }

    private void updateMovement() {
        if (moving) {
            Vector3 targetDist = targetPosition - transform.position;
            float distanceToTarget = targetDist.magnitude;
            godPls = distanceToTarget;

            Vector3 directon = new Vector3(targetDist.x, 0, targetDist.z); ;

            if (distanceToTarget < TARGET_EPSILON) {
                StopMovement();
                return;
            }

            animationController?.SetMoving(true);
            float moveDistance = (MovementSpeed * MovementSpeedModifier) * Time.deltaTime;
            if(perciceMovement && moveDistance > distanceToTarget) {
                transform.position = targetPosition;
                StopMovement();
                return;
            }

            characterController.Move(targetDist.normalized * moveDistance);
            rotateTowards(directon);

            if (moveTimer > 0) {
                moveTimer -= Time.deltaTime;
                if (moveTimer <= 0) StopMovement();
            }
        }
    }

    private void rotateTowards(Vector3 direction) {
        transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(direction), transform.rotation, 0.9f);
    }

    private void putToGround() {
        Vector3 ccOffset = Vector3.up * (characterController.height * 0.5f) - characterController.center;
        Ray r = new Ray(transform.position - ccOffset + Vector3.up * 0.1f, Vector3.down);
        RaycastHit info;
        int layermask = LayerMask.GetMask("Terrain");
        if (Physics.Raycast(r, out info, 2, layermask)) {
            transform.position = info.point + ccOffset;
        }
    }

    public bool IsMoving() {
        return moving;
    }

}
