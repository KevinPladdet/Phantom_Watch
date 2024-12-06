using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlaneManager : MonoBehaviour
{
    public static CameraPlaneManager instance;

    [SerializeField] List<CameraPlaneObject> planes = new List<CameraPlaneObject>();
    [SerializeField] List<CameraPlaneObject> flickerLlanes = new List<CameraPlaneObject>();
    
    [System.Serializable]
    public class CameraPlaneObject
    {
        public GameObject plane;
        public bool forceDisabled = false;
    }

    [SerializeField] float cameraFlickerDelay = 1;
    [SerializeField] float flickerLength = 0.1f;

    private void Awake()
    {
        if(instance == null) { instance = this; }else { Destroy(this); }
    }

    private void Start()
    {
        StartCoroutine(FlickerCameras());
    }

    IEnumerator FlickerCameras()
    {
        yield return new WaitForSeconds(Random.Range(cameraFlickerDelay - cameraFlickerDelay / 3, cameraFlickerDelay));
        
        int randomCamera = Random.Range(0, planes.Count);
        FlickerCam(randomCamera);

        StartCoroutine(FlickerCameras());
    }

    public void FlickerCam(int number)
    {
        if (number > planes.Count) { return; }

        StartCoroutine(FlickerCamCoroutine(number));
    }

    IEnumerator FlickerCamCoroutine(int number) 
    {
        if (!planes[number].forceDisabled)
        {
            DisableCam(number, false, true);
            yield return new WaitForSeconds(flickerLength);
            EnableCam(number, false, true);
        }
    }

    public void EnableCam(int number, bool forceEnable = true, bool flicker = false)
    {
        if(number > planes.Count) { return; }

        if(planes[number].forceDisabled && forceEnable == false) { return; }

        if (!flicker)
        {
            planes[number].plane.SetActive(false);
            planes[number].forceDisabled = false;
        }
        else
        {
            flickerLlanes[number].plane.SetActive(false);
            flickerLlanes[number].forceDisabled = false;
        }
    }

    public void DisableCam(int number, bool forceDisable = true , bool flicker = false)
    {
        if (number > planes.Count) { return; }

        if (!flicker)
        {
            planes[number].forceDisabled = forceDisable;
            planes[number].plane.SetActive(true);
        }
        else
        {
            flickerLlanes[number].forceDisabled = forceDisable;
            flickerLlanes[number].plane.SetActive(true);
        }
    }

}
