using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseSlot : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _fuseObject;
    [SerializeField] private int _index;

    [Space(10)]
    [SerializeField] private MeshRenderer _lightRed;
    private Material _matLightRed;
    [SerializeField] private MeshRenderer _lightGreen;
    private Material _matLightGreen;
    [SerializeField] private Material _matLightOff;

    [Space(10)]
    [SerializeField] private AudioClip _addSFX;
    [SerializeField] private AudioClip _removeSFX;

    [SerializeField] bool debugTest = false;

    private readonly string _tagName = "HoldingFuse";

    public bool IsBroken { get; private set; }

    public bool IsFilled { get; private set; }

    private void Start()
    {
        _matLightRed = _lightRed.material;
        _lightRed.material = _matLightOff; // turn off red light since all fuses start as working.
        _matLightGreen = _lightGreen.material;
    }

    private void Update()
    {
        if (debugTest) { debugTest = false; Break(); }
    }

    public void Interact()
    {
        if (IsBroken)
        {
            // Remove broken fuse
            ToggleFuseVisibility(false);
            IsBroken = false;

            AudioPool.Instance.PlaySound(_removeSFX, 1, true, transform.position);
        }
        else if (!IsFilled)
        {
            Fix();
        }
    }
    public void Break()
    {
        IsBroken = true;
        ToggleLights(false);

        if (CameraPlaneManager.instance != null)
            CameraPlaneManager.instance.DisableCam(_index);
    }

    // Adds working fuse to empty slot
    public void Fix()
    {
        // if the "HoldingFuse" child "boolean" gameObject is disabled, return. That gameObject defines whether the player is "holding" a fuse.
        GameObject obj = GameObject.FindGameObjectWithTag(_tagName);
        if (obj == null)
        {
            Debug.LogWarning("Could not find HoldingFuse gameObject! Are all the names correct?");
        }
        GameObject boolObj = null;
        foreach (Transform t in obj.transform)
        {
            boolObj = t.gameObject;
        }
        if (boolObj == null || !boolObj.activeInHierarchy)
            return;
        
        // If all the above checks passed, turn off the boolean GameObject to signify the fuse was used and continue with fixing.
        boolObj.SetActive(false);

        IsBroken = false;
        ToggleFuseVisibility(true);
        ToggleLights(true);

        GameStatsManager.instance.RepairBreaker();

        AudioPool.Instance.PlaySound(_addSFX, 1, true, transform.position);

        if (CameraPlaneManager.instance != null)
            CameraPlaneManager.instance.EnableCam(_index);
    }

    private void ToggleFuseVisibility(bool isVisible)
    {
        if (_fuseObject == null)
        {
            Debug.LogWarning("GameObject \"_fuseObject\" is missing in object " + gameObject.name + "! We kinda need that, please fix.");
            return;
        }

        _fuseObject.SetActive(isVisible);
        IsFilled = isVisible;
    }
    private void ToggleLights(bool isOn)
    {
        _lightRed.material = isOn ? _matLightOff : _matLightRed;
        _lightGreen.material = isOn ? _matLightGreen : _matLightOff;
    }
}
