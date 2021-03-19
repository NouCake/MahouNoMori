﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntityController : EntityController {

    [SerializeField] private float SprintSpeedModifier = 2;

    override protected void Update() {
        base.Update();

        movementController.MovementSpeedModifier = calculateSpeedModifier();
        animationController.SetRunning(movementController.MovementSpeedModifier > 1);
        Vector2 input = getInputVector();

        if (isInputOverThreshhold(input)) {
            Vector3 camFwd = getCameraForward();
            Vector3 moveDirection = input.y * camFwd + Vector3.Cross(camFwd, Vector3.up) * input.x;
            movementController.MoveIntoDirection(moveDirection);
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }

    }


    private Vector3 getCameraForward() {
        Vector3 camPos = Camera.main.transform.position;
        Vector3 fwd = new Vector3(transform.position.x - camPos.x, 0, transform.position.z - camPos.z).normalized;
        return fwd;
    }


    private Vector2 getInputVector() {
        return new Vector2(-Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
    private bool isInputOverThreshhold(Vector2 input) {
        return input.x * input.x + input.y * input.y > 0.1f;
    }


    private float calculateSpeedModifier() {
        /*if (magicController.isCasting()) {
            animator.SetBool("Running", false);
            return moveSpeed * 0.75f;
        }*/
        if (Input.GetKey(KeyCode.LeftShift)) {
            //animator.SetBool("Running", true);
            return SprintSpeedModifier;
        }

        return 1;
    }

}