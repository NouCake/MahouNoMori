using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Attack")]
public class AttackInfo : ScriptableObject {

    public Sprite AttackArtwork;
    [Space(10)]
    public string AttackType = "Melee";
    public AttackController AttackController;
    public float ManaCost;
    [Space(10)]
    public float AttackTime; //Time for the full attack
    public float AttackHitTime; //Time until the attack hits target
    public float AttackChargeTime; //Time attack can be canceld

}
