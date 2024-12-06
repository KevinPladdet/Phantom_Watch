using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemManager : MonoBehaviour
{

    public static ItemManager instance;

    private void Awake()
    {
        if(instance == null) { instance = this; } else { Destroy(this); }
    }

    [Header("References")]
    [SerializeField] private TextMeshProUGUI cooldownText;
    [SerializeField] private Button useItemButton;

    public bool wrongItem = false;
    public float itemCooldownTime = 4;
    private float gameTime = 10;
    
    void Update()
    {
        /*if (wrongItem)
        {
            gameTime -= Time.deltaTime;
            cooldownText.text = "! Wrong Item ! Cooldown : " + gameTime.ToString("F0");
            if (gameTime <= 0)
            {
                gameTime = 10;
                wrongItem = false;
                cooldownText.text = "Cooldown : 0";
            }
        }*/
    }

    public void UseItem(int ghostType)
    {
        int room = CameraManager.instance.currentCameraIndex + 1;

        if(AnomalyController.instance == null) { return; }

        switch (ghostType)
        {
            case 1: AnomalyController.instance.UseItem(room, GhostType.Specter);break;
            case 2: AnomalyController.instance.UseItem(room, GhostType.Demonic);break;
            case 3: AnomalyController.instance.UseItem(room, GhostType.Poltergeist);break;
            case 4: AnomalyController.instance.UseItem(room, GhostType.Possession);break;
            default:
                break;
        }
    }

    public void SetText(string text)
    {
        cooldownText.text = text;
    }

    // Call this method if the right item is used
    public void RightItem()
    {
        // Code here to banish ghost and fix anomaly
        //
    }

    // Call this method if the wrong item is used
    public void WrongItem()
    {
        //wrongItem = true;
        //cooldownText.text = "Item Cooldown: 10";
    }
}
