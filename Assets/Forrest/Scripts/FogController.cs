using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogController : MonoBehaviour
{

    public Transform target;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        if (target) {
            transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
            putToGround();;
        }
    }

    private void putToGround() {
        Ray r = new Ray(transform.position, Vector3.down);
        RaycastHit info;
        int layermask = LayerMask.GetMask("Terrain");
        if (Physics.Raycast(r, out info, 2, layermask)) {
            transform.position = info.point;
        }
    }

}
