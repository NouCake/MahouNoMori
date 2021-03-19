using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagicController : MonoBehaviour {

    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject bomb;

    private Animator animator;
    private Transform spellOffset;

    float armUpTime;
    void Start() {
        animator = GetComponentInChildren<Animator>();
        Transform af = transform.Find("Model").Find("AnimeGirl").Find("skelton");
        Transform f = af.Find("Root").Find("J_Bip_C_Hips").Find("J_Bip_C_Spine").Find("J_Bip_C_Chest");
        Transform f2 = f.Find("J_Bip_C_UpperChest").Find("J_Bip_R_Shoulder").Find("J_Bip_R_UpperArm");
        spellOffset = f2.Find("J_Bip_R_LowerArm").Find("J_Bip_R_Hand");
    }

    public bool isCasting() {
        return armUpTime > 0;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Targetable target = TargetSelector.Target;
            if(target != null) {
                animator.SetLayerWeight(1, 1);
                armUpTime = 0.5f;
                GameObject p = Instantiate(projectile, spellOffset.position, transform.rotation);
                p.GetComponent<TargetProjectile>().UpdateTarget(target, Vector3.zero);
            }
        }

        if (Input.GetMouseButtonDown(1)) {
            Targetable target = TargetSelector.Target;
            if(target != null) {
                GameObject buff = Instantiate(bomb, target.transform);
                ParticleSystem buffPS = buff.GetComponent<ParticleSystem>();
                Destroy(buff, buffPS.main.duration);
            }

        }

        armUpTime -= Time.deltaTime;
        if(armUpTime <= 0) {
            animator.SetLayerWeight(1, 0);
        }
    }

}
