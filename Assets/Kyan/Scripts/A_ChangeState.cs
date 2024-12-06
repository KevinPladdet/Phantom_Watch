using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_ChangeState : Anomaly
{
    public GameObject defaultState;
    public GameObject changedState;

    private void Start()
    {
        ShowAnomaly(false);
    }

    public override void ActivateAnomaly()
    {
        ShowAnomaly(true);
    }

    public override void DeactivateAnomaly()
    {
        ShowAnomaly(false);
        RemoveActiveState();
    }

    void ShowAnomaly(bool show)
    {
        defaultState.SetActive(!show);
        changedState.SetActive(show);
    }
}
