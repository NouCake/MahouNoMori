using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour {

    public static UIController Instance;

    [SerializeField] private MonoBehaviour PlayerStateInfo;
    [Space(10)]
    [SerializeField] private UIHealthbar PlayerHealthbar;
    [SerializeField] private UIHealthbar PlayerManabar;
    [SerializeField] private TMP_Text PlayerName;
    [Space(10)]
    [SerializeField] private UISkillSlot Skill1;
    [SerializeField] private UISkillSlot Skill2;
    [SerializeField] private UISkillSlot Skill3;

    private StatInfo info;

    private void Awake() {
        Instance = this;
        info = (StatInfo)PlayerStateInfo;

        Skill1.SetSlotNumber(1);
        Skill2.SetSlotNumber(2);
        Skill3.SetSlotNumber(3);
    }

    private void Update() {
        PlayerHealthbar.SetName(info.GetHealth()+"");
        PlayerHealthbar.SetHealthPercent(info.GetHealth() / (float)info.GetMaxHealth());
        PlayerManabar.SetHealthPercent(info.GetMana() / (float)info.GetMaxMana());

    }

    public void SetSkillArtwork(int slot, Sprite artwork) {
        switch (slot) {
            case 1:
                Skill1.SetSkillImage(artwork);
                break;
            case 2:
                Skill2.SetSkillImage(artwork);
                break;
            case 3:
                Skill3.SetSkillImage(artwork);
                break;
        }
    }

    public void SetActiveSlot(int slot) {
        switch (slot) {
            case 1:
                Skill1.SetSelected(true);
                Skill2.SetSelected(false);
                Skill3.SetSelected(false);
                break;
            case 2:
                Skill1.SetSelected(false);
                Skill2.SetSelected(true);
                Skill3.SetSelected(false);
                break;
            case 3:
                Skill1.SetSelected(false);
                Skill2.SetSelected(false);
                Skill3.SetSelected(true);
                break;

        }
    }


}
