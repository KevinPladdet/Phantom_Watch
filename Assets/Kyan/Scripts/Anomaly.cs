using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GhostType { Specter, Demonic, Poltergeist, Possession }

public class Anomaly : MonoBehaviour
{
    [HideInInspector] public AnomalyRoom room;
    public GhostType ghostType;

    public bool developmentEnableAnomaly = false;
    public bool developmentRemoveAnomaly = false;
    private void Update()
    {
        if (developmentEnableAnomaly) { developmentEnableAnomaly = false; ActivateAnomaly(); }
        if (developmentRemoveAnomaly) { developmentRemoveAnomaly = false; DeactivateAnomaly(); }
    }

    public virtual void ActivateAnomaly()
    {

    }
    public virtual void DeactivateAnomaly()
    {

    }

    protected void RemoveActiveState()
    {
        AnomalyController.instance.DeActivateAnomaly(this);
    }
}
