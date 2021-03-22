using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthbar : MonoBehaviour {

    public TMP_Text text;
    private Image healthbar;

    private Targetable target;
    private StatInfo health;

    private void Awake() {
        text = GetComponentInChildren<TextMeshProUGUI>();
        healthbar = transform.Find("Full").GetComponent<Image>();
    }

    private void Update() {
        if (target != null) {
            transform.position = target.transform.position + Vector3.up * 2;
            transform.LookAt(transform.position + (transform.position - Camera.main.transform.position));
            SetHealthPercent(health.GetHealth() / (float)health.GetMaxHealth());
        }
    }

    public void SetTarget(Targetable target) {
        this.target = target;
        this.health = target.GetHealthController();
    }

    public void SetName(string name) {
        text.text = name;
    }

    public void SetHealthPercent(float percent) {
        healthbar.fillAmount = percent;
    }

}
