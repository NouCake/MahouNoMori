using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Attack")]
public class AttackInfo : ScriptableObject {

    public GameObject AttackHitParticles;

    public float AttackTime; //Time for the full attack
    public float AttackHitTime; //Time until the attack hits target
    public float AttackChargeTime; //Time attack can be canceld

    public string AttackType;

}
