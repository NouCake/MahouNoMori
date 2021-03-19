using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EntityMovementController : MonoBehaviour {

    private readonly float TARGET_EPSILON = 0.1f;

    private CharacterController characterController;

    [SerializeField] public float MovementSpeed = 10;
    [SerializeField] public float MovementSpeedModifier = 1;

    private bool targetMoving;
    private Vector3 targetPosition;
    private float targetMoveTime;
    private float lastTargetUpdateTime;
    public float stopTime;

    private void Awake() {
        characterController = GetComponent<CharacterController>();
    }

    public void StopMovement() {
        StopMovement(0);
    }

    public void StopMovement(float time) {
        targetMoving = false;
        stopTime = time;
    }

    public void SimpleMoveToTarget(Vector3 targetPos) {
        targetMoving = true;
        this.targetPosition = targetPos;
        lastTargetUpdateTime = Time.time;
    }

    public void MoveToTarget(Vector3 targetPos) {
        SimpleMoveToTarget(targetPos);
        targetMoveTime = Vector3.Distance(transform.position, targetPos) / (MovementSpeed * MovementSpeedModifier);
    }

    private void Update() {
        if(stopTime > 0) {
            stopTime -= Time.deltaTime;
            return;
        }


        if (targetMoving) updateTargetMove();
        putToGround();
    }

    private void rotateTowards(Vector3 direction) {
        transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(direction), transform.rotation, 0.9f);
    }

    private void putToGround() {
        Vector3 ccOffset = Vector3.up * (characterController.height * 0.5f) - characterController.center;
        Ray r = new Ray(transform.position - ccOffset, Vector3.down);
        RaycastHit info;
        int layermask = LayerMask.GetMask("Terrain");
        if (Physics.Raycast(r, out info, 2, layermask)) {
            transform.position = info.point + ccOffset;
        }
    }

    private void updateTargetMove() {
        float targetDistance = Vector3.Distance(transform.position, targetPosition);
        if (targetDistance > TARGET_EPSILON && targetMoveTime > 0) {
            Vector3 direction = (targetPosition - transform.position).normalized;
            direction = new Vector3(direction.x, 0, direction.z);
            characterController.Move(direction * (MovementSpeed * MovementSpeedModifier) * Time.deltaTime);
            rotateTowards(direction);
            targetMoveTime -= Time.deltaTime;
        } else {
            targetMoving = false;
        }
    }

    public bool IsMoving() {
        return targetMoving;
    }

}
