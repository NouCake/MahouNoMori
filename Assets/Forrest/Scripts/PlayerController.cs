using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float moveSpeed = 10;
    [SerializeField]
    private float sprintModificator = 1.75f;

    private CharacterController charcon;
    private Animator animator;
    
    void Start() {
        charcon = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    void Update() {
        doMovement();
        putToGround();
    }

    private float getMoveSpeed() {
        if (Input.GetKey(KeyCode.LeftShift)) {
            animator.SetBool("Running", true);
            return moveSpeed * sprintModificator;
        }
        animator.SetBool("Running", false);
        return moveSpeed;
    }

    private Vector2 getInputVector() {
        return new Vector2(-Input.GetAxis("Horizontal") , Input.GetAxis("Vertical"));
    }

    private bool isInputOverThreshhold(Vector2 input) {
        return input.x * input.x + input.y * input.y > 0.1f;
    }

    private Vector3 getCameraForward() {
        Vector3 camPos = Camera.main.transform.position;
        Vector3 fwd = new Vector3(transform.position.x - camPos.x, 0, transform.position.z - camPos.z).normalized;
        return fwd;
    }

    private void moveCharacter(Vector3 direction, float speed) {
        charcon.Move(direction * Time.deltaTime * speed);
    }

    private void doMovement() {
        Vector2 input = getInputVector(); 
        float curMoveSpeed = getMoveSpeed();

        if (isInputOverThreshhold(input)) {
            animator.SetBool("Walking", true);
            Vector3 camFwd = getCameraForward();
            Vector3 moveDirection = input.y * camFwd + Vector3.Cross(camFwd, Vector3.up) * input.x;
            moveCharacter(moveDirection, curMoveSpeed);
            transform.rotation = Quaternion.LookRotation(moveDirection);
        } else {
            animator.SetBool("Walking", false);
        }
    }

    private void putToGround() {
        Vector3 ccOffset = Vector3.up * (charcon.height * 0.5f) - charcon.center;
        Ray r = new Ray(transform.position + ccOffset, Vector3.down);
        RaycastHit info;
        int layermask = LayerMask.GetMask("Terrain");
        if(Physics.Raycast(r, out info, 2, layermask)) {
            transform.position = info.point + ccOffset;
        }
    }

}
