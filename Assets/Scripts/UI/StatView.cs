using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class StatView: MonoBehaviour
{
    [SerializeField]
    protected TextMeshProUGUI statText; 
    public abstract void updateStat();

    protected virtual void Start() {
        statText = GetComponent<TextMeshProUGUI>();
        updateStat();
    }

}
