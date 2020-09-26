using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagicController : MonoBehaviour {

    [SerializeField]
    private GameObject projectile;

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

    // Update is called once per frame
    void Update() {
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0)) {
            animator.SetLayerWeight(1, 1);
            armUpTime = 0.5f;
            RaycastHit info;
            int layermask = LayerMask.GetMask("Terrain");
            if (Physics.Raycast(r, out info, 30, layermask)) {
                GameObject p = Instantiate(projectile, spellOffset.position, transform.rotation);
                GameObject target = new GameObject();
                target.transform.position = info.point;
                p.GetComponent<TargetProjectile>().UpdateTarget(target.transform, Vector3.zero);
            }
        }
        armUpTime -= Time.deltaTime;
        if(armUpTime <= 0) {
            animator.SetLayerWeight(1, 0);
        }
    }

    private void OnDrawGizmos() {
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        Gizmos.DrawRay(r);
    }

}
