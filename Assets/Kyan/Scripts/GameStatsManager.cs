using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatsManager : MonoBehaviour
{
    public static GameStatsManager instance;

    public int ghostsEncountered = 0;
    public int specterEncounters = 0;
    public int demonicEncounters = 0;
    public int poltergeistEncounters = 0;
    public int possessionEncounters = 0;

    public int breakersRepaired = 0;

    private void Awake()
    {
        if(instance == null) { instance = this; } else { Destroy(this); }
    }

    public void AddGhost(GhostType type)
    {
        ghostsEncountered++;
        switch (type)
        {
            case GhostType.Specter:
                specterEncounters++;
                break;
            case GhostType.Demonic:
                demonicEncounters++;
                break;
            case GhostType.Poltergeist:
                poltergeistEncounters++;
                break;
            case GhostType.Possession:
                possessionEncounters++;
                break;
            default:
                break;
        }
    }

    public void RepairBreaker()
    {
        breakersRepaired++;
    }
}
