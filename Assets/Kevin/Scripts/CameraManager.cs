using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [SerializeField] float sidePaddingHoverPercentage = 20;
    [SerializeField] float BottomHoverPercentage = 5;

    private void Awake()
    {
        if (instance == null) { instance = this; } else { Destroy(this); }
    }

    [SerializeField] private List<GameObject> securityCameras = new List<GameObject>();

    [HideInInspector] public int currentCameraIndex = 0;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI cameraNumberText;
    [SerializeField] private AudioClip camSwitchAudio;

    private void Start()
    {
        // Only enable Camera1 and disable the others
        for (int i = 0; i < securityCameras.Count; i++)
        {
            securityCameras[i].SetActive(i == currentCameraIndex);
        }
        UpdateCameraText();
    }

    private void Update()
    {
        float widthPercentage = Input.mousePosition.x / Screen.width * 100;
        float heightPercentage = Input.mousePosition.y / Screen.height * 100;

        if(heightPercentage < BottomHoverPercentage)
        {
            if(widthPercentage > sidePaddingHoverPercentage && widthPercentage < 100 - sidePaddingHoverPercentage)
            {
                ModeSwapper.instance.ExitComputer();
            }
        }
    }

    public void SetCameraByIndex(int index)
    {
        // Check if you did not click the same camera
        if (index == currentCameraIndex)
        {
            return;
        }

        AudioPool.Instance.PlaySound(camSwitchAudio, 0.9f, true);

        // If int index is too little or too big, do nothing
        if (index < 0 || index >= securityCameras.Count) return;

        // Disable current camera
        securityCameras[currentCameraIndex].SetActive(false);

        // Set currentCameraIndex + enable the camera
        currentCameraIndex = index;
        securityCameras[currentCameraIndex].SetActive(true); 

        UpdateCameraText();
    }

    private void UpdateCameraText()
    {
        cameraNumberText.text = AnomalyController.instance.rooms[currentCameraIndex].roomName;
    }
}