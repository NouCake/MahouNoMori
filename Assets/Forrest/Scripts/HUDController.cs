using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour {

    public static HUDController Instance;

    [SerializeField] private Canvas UICanvas;
    [SerializeField] private UIHealthbar HealthbarInstance;

    private void Awake() {
        Instance = this;
    }



    public UIHealthbar CreateHealthbar(Targetable target) {
        UIHealthbar healthbar =  Instantiate<UIHealthbar>(HealthbarInstance, UICanvas.transform);
        healthbar.SetName(target.name);
        healthbar.SetTarget(target);
        return healthbar;
    }

}
