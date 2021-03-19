using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour {

    [SerializeField] private UIHealthbar PlayerHealthbar;
    [SerializeField] private UIHealthbar PlayerManabar;
    [SerializeField] private TMP_Text PlayerName;
    [Space(10)]
    [SerializeField] private MonoBehaviour PlayerStateInfo;

    private StatInfo info;

    private void Awake() {
        info = (StatInfo)PlayerStateInfo;
    }

    private void Update() {
        PlayerHealthbar.SetName(info.GetHealth()+"");
        PlayerHealthbar.SetHealthPercent(info.GetHealth() / (float)info.GetMaxHealth());
        PlayerManabar.SetHealthPercent(info.GetMana() / (float)info.GetMaxMana());
    }


}
