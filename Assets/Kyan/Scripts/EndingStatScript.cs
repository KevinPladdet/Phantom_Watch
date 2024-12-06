using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndingStatScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI totalGhosts;
    [SerializeField] private TextMeshProUGUI totalSpecter;
    [SerializeField] private TextMeshProUGUI totalDemonic;
    [SerializeField] private TextMeshProUGUI totalPoltergeist;
    [SerializeField] private TextMeshProUGUI totalPossession;
    [SerializeField] private TextMeshProUGUI totalTimeSurvived;
    [SerializeField] private TextMeshProUGUI totalBreakersRepaired;


    public void SetData()
    {
        string ghosts = GameStatsManager.instance.ghostsEncountered.ToString();
        string specters = GameStatsManager.instance.specterEncounters.ToString();
        string demonics = GameStatsManager.instance.demonicEncounters.ToString();
        string poltergeists = GameStatsManager.instance.poltergeistEncounters.ToString();
        string possesions = GameStatsManager.instance.possessionEncounters.ToString();

        int minutes = WinScript.instance.currentMinute;
        string minutesString = minutes.ToString();
        if(minutes < 10)
        {
            minutesString = "0" + minutes.ToString();
        }

        string time = WinScript.instance.currentHour.ToString() + ":" + minutesString + " AM";
        string breakers = GameStatsManager.instance.breakersRepaired.ToString();
        
        totalGhosts.text = "Ghosts - " + ghosts;
        totalSpecter.text = "Specter Ghosts - " + specters;
        totalDemonic.text = "Demonic Ghosts - " + demonics;
        totalPoltergeist.text = "Poltergeist Ghosts - " + poltergeists;
        totalPossession.text = "Possession Ghosts - " + possesions;
        totalTimeSurvived.text = "Survived until - " + time;
        totalBreakersRepaired.text = "Repaired Breakers - " + breakers;
    }
}
