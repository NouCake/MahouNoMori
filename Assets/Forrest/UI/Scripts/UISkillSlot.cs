using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISkillSlot : MonoBehaviour {

    [SerializeField] private TMP_Text SlotNumber;
    [SerializeField] private Image SkillImage;
    [SerializeField] private GameObject Selection;

    private void Awake() {
        SetSelected(false);
    }

    public void SetSelected(bool selected) {
        Selection.SetActive(selected);
    }

    public void SetSlotNumber(int number) {
        SlotNumber.text = number + "";
    }

    public void SetSkillImage(Sprite image) {
        SkillImage.sprite = image;
    }


}
