using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntityController : EntityController {

    [SerializeField] private float SprintSpeedModifier = 2;
    [SerializeField] private TargetSelector TargetSelector;

    [SerializeField] private AttackInfo[] Attacks;

    private AttackInfo selectedAttack;

    protected override void Start() {
        base.Start();
        UIController.Instance.SetSkillArtwork(1, Attacks[0].AttackArtwork);
        UIController.Instance.SetSkillArtwork(2, Attacks[1].AttackArtwork);
        UIController.Instance.SetSkillArtwork(3, Attacks[2].AttackArtwork);
        SetAttack(0);
    }

    override protected void Update() {
        base.Update();

        if(Mana < GetMaxMana()) {
            Mana += Time.deltaTime;
        }

        updateInput();
        updateAttacking();
        updateMovement();
    }

    private void updateInput() {

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            SetAttack(0);
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            SetAttack(1);
        } else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            SetAttack(2);
        }
    }

    private void SetAttack(int AttackSlot) {
        if (Attacks[AttackSlot] != null) {
            selectedAttack = Attacks[AttackSlot];
            TargetSelector.SetProjectionSprite(Attacks[AttackSlot].AttackType == "Area");
        }
        UIController.Instance.SetActiveSlot(AttackSlot+1);
    }

    private void updateAttacking() {
        if (!attackController.IsAttacking() && Input.GetMouseButtonDown(0)) {
            if(selectedAttack.AttackType == "Area") {
                attackController.StartAttack(TargetSelector.transform.position, selectedAttack);
            } else {
                Targetable target = TargetSelector.Target;
                if (target != null) attackController.StartAttack(target, selectedAttack);
            }
        }
    }

    private void updateMovement() {
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
        if (attackController.IsAttacking()) {
            return 0.25f;
        }
        if (Input.GetKey(KeyCode.LeftShift)) {
            //animator.SetBool("Running", true);
            return SprintSpeedModifier;
        }

        return 1;
    }

}
