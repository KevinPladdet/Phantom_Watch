using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnomalyController : MonoBehaviour
{
    public static AnomalyController instance;

    private void Awake()
    {
        if(instance == null) { instance = this; } else { Destroy(this); }
    }

    public int maxAnomalies = 7;

    public List<AnomalyRoom> rooms = new List<AnomalyRoom>();
    public Dictionary<Anomaly,AnomalyRoom> activeAnomalies = new Dictionary<Anomaly, AnomalyRoom>();

    public UnityEvent playerDead;

    public bool DevelopmentCreateAnomaly = false;

    [Space(10)]
    [SerializeField] AudioClip _anomalyFoundSFX;
    [SerializeField] private AudioClip _clickSFX;
    [SerializeField] private AudioClip _clickFailSFX;

    private void Update()
    {
        if (DevelopmentCreateAnomaly) { DevelopmentCreateAnomaly = false; CreateAnomaly(); }
    }

    public void CreateAnomaly(bool isPossibleToSpawn = false)
    {
        if(rooms.Count <= 0)
        {
            Debug.Log("Tried Creating anomaly but there are no rooms to spawn them in");
            return; 
        }
        int roomI = Random.Range(0, rooms.Count + 1);
        if(roomI == 9) { roomI = 8; }
        AnomalyRoom chosenRoom = rooms[roomI];

        if(chosenRoom.activeRoomAnomalies.Count < chosenRoom.maxRoomAnomalies)
        {
            chosenRoom.CreateAnomalyInRoom();
            return;
        }

        bool isRoomLeft = isPossibleToSpawn;

        if (!isPossibleToSpawn)
        {
            foreach (var room in rooms)
            {
                //if no room left -> check if rooom left else therer is room :)
                isRoomLeft = !isRoomLeft ? room.CanSpawnAnomaly() : isRoomLeft;
                
            }
        }
         
        if (isRoomLeft)
        {
            CreateAnomaly(true);
            if(activeAnomalies.Count >= maxAnomalies)
            {
                playerDead.Invoke();
            }
            return;
        }

        Debug.Log("Could not create anomaly because there is no space left");
    }

    Coroutine waitForItem;
    bool usingItem = false;
    public void UseItem(int camera, GhostType itemUsed)
    {
        if (usingItem || ItemManager.instance.wrongItem)
        {
            AudioPool.Instance.PlaySound(_clickFailSFX, 0.75f);
            return;
        }

        AudioPool.Instance.PlaySound(_clickSFX, 0.75f, true);

        if (waitForItem != null)
        {
            StopCoroutine(waitForItem);
        }

        waitForItem = StartCoroutine(WaitforItem(camera, itemUsed));
    }

    IEnumerator WaitforItem(int camera, GhostType itemUsed)
    {
        usingItem = true;

        string waitingText = "Waiting";
        WaitForSeconds await = new WaitForSeconds(ItemManager.instance.itemCooldownTime / 6);
        for (int i = 0; i < 6; i++)
        {
            ItemManager.instance.SetText(waitingText);
            waitingText += ".";
            yield return await;
        }


        AnomalyRoom room = GetRoomFromCamera(camera);
        if (room != null) 
        {
            bool success = room.UseItemInRoom(itemUsed);

            if (success)//if you did find an item to use
            {
                AudioPool.Instance.PlaySound(_anomalyFoundSFX);
                ItemManager.instance.SetText("Exorcism Successful");
            }
            else
            {
                ItemManager.instance.SetText("Exorcism Failed");
            }
        }
        else
        {
            ItemManager.instance.SetText("Exorcism Failed");
        }

        usingItem = false;

        yield return new WaitForSeconds(ItemManager.instance.itemCooldownTime / 2);

        ItemManager.instance.SetText("");
    }

    AnomalyRoom GetRoomFromCamera(int camera)
    {
        foreach (var room in rooms)
        {
            if(room.roomNumber == camera)
            {
                return room;
            }
        }
        return null;
    }

    public void DeActivateAnomaly(Anomaly anomaly)
    {
        AnomalyRoom room = activeAnomalies[anomaly];
        bool doesRoomContainAnomaly = room.activeRoomAnomalies.Contains(anomaly);

        if (doesRoomContainAnomaly)
        {
            room.activeRoomAnomalies.Remove(anomaly);
            room.roomAnomalies.Add(anomaly);
        }

        activeAnomalies.Remove(anomaly);
    }
}

[System.Serializable]
public class AnomalyRoom
{
    public string roomName = "room";
    public int roomNumber = 0;
    public int maxRoomAnomalies = 2;
    public List<Anomaly> roomAnomalies = new List<Anomaly>();
    public List<Anomaly> activeRoomAnomalies = new List<Anomaly>();

    public bool CanSpawnAnomaly()
    {
        if(activeRoomAnomalies.Count >= maxRoomAnomalies) { return false; }
        if(roomAnomalies.Count > 0)
        {
            List<GhostType> ghostTypes = new List<GhostType>();
            foreach (var anomaly in activeRoomAnomalies)
            {
                ghostTypes.Add(anomaly.ghostType);
            }
            foreach (var anomaly in roomAnomalies)
            {
                if (!ghostTypes.Contains(anomaly.ghostType)) { return true; }
            }
        }
        return false;
    }

    public void CreateAnomalyInRoom()
    {
        if(roomAnomalies.Count <= 0)
        {
            return; 
        }

        Anomaly anomaly = roomAnomalies[Random.Range(0, roomAnomalies.Count)];

        if (!activeRoomAnomalies.Contains(anomaly))
        {
            bool canCreate = true;
            foreach (var _checkedAnomaly in activeRoomAnomalies)
            {
                if (_checkedAnomaly.ghostType == anomaly.ghostType)
                {
                    canCreate = false;
                    break;
                }
            }

            if (canCreate)
            { 
                activeRoomAnomalies.Add(anomaly);
                roomAnomalies.Remove(anomaly);

                AnomalyController.instance.activeAnomalies.Add(anomaly,this);

                CameraPlaneManager.instance.FlickerCam(roomNumber - 1);

                anomaly.ActivateAnomaly();
                Debug.Log(anomaly.gameObject.name + " is now active in room " + roomName);
                
                return;
            }
            else
            {
                bool canRetry = false;
                foreach (var _checkedAnomaly in roomAnomalies)
                {
                    if (_checkedAnomaly.ghostType != anomaly.ghostType)
                    {
                        canRetry = true;
                        break;
                    } 
                }
                if (canRetry)
                {
                    CreateAnomalyInRoom();
                }
                else
                {
                    Debug.Log("sorry there is no way to create an anomaly in this room");
                }
            }
        }
    }

    public bool UseItemInRoom(GhostType ghostType)
    {
        Anomaly anomaly = GetAnomalyByType(ghostType);
        if(anomaly == null) { return false; }

        CameraPlaneManager.instance.FlickerCam(roomNumber - 1);

        anomaly.DeactivateAnomaly();

        GameStatsManager.instance.AddGhost(anomaly.ghostType);

        return true;
    }

    Anomaly GetAnomalyByType(GhostType _ghostType)
    {
        foreach (var anomaly in activeRoomAnomalies)
        {
            if(anomaly.ghostType == _ghostType)
            {
                return anomaly;
            }
        }
        return null;
    }
}
 