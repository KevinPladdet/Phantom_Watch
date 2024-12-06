using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
public class ModeSwapper : MonoBehaviour
{
    public static ModeSwapper instance;
    private void Awake()
    {
        if(instance == null) { instance = this; } else { Destroy(this); }
    }

    bool inComputer = false;

    [SerializeField] Transform defaultCamPos;
    [SerializeField] Transform computerCamPos;
    [SerializeField] float lerpTime;
    [SerializeField] AnimationCurve lerpAnimation;

    [SerializeField] GameObject computerSystem;
    [SerializeField] GameObject fpsSystem;
    [SerializeField] WinScript winScript;
    [SerializeField] Volume PostProcessing2;

    public bool enterComputer = false;
    public bool exitComputer = false;

    bool firstTime = true;

    private void Update()
    {
        if (enterComputer) { enterComputer = false; EnterComputer(); }
        if (exitComputer) { exitComputer = false; ExitComputer(); }
    }

    private void Start()
    {
        computerSystem.SetActive(false);
        fpsSystem.SetActive(true);
    }

    void StartGame()
    {
        BreakerManager.Instance.StartScript();
        AnomalyCreation.instance.StartScript();
        WinScript.instance.StartScript();
        WinScript.instance.PlaySound();
    }

    public void EnterComputer()
    {
        if (inComputer) { return; }

        inComputer = true;

        if (firstTime)
        {
            firstTime = false;
            StartGame();
        }

        CameraMove.Instance.IsActive = false;
        CameraMove.Instance.ForceRemoveBook();

        StartCoroutine(LerpTransform(computerCamPos, EnteredComputer));
    }
    void EnteredComputer()
    {
        PostProcessing2.weight = 0.4f;
        computerSystem.SetActive(true);
        fpsSystem.SetActive(false);
        CameraMove.Instance.ToggleUI(false,false,false);
        winScript.ToggleInCam(true);
        CameraMove.Instance._canHover = false;
    }

    public void ExitComputer()
    {
        computerSystem.SetActive(false);
        fpsSystem.SetActive(true);
        PostProcessing2.weight = 0.6f;

        if (!inComputer) { return; }

        inComputer = false;

        StartCoroutine(LerpTransform(defaultCamPos, ExitedComputer));

        winScript.ToggleInCam(false);

        CameraMove.Instance.IsActive = true;
        CameraMove.Instance.ToggleUI(false, true, true);
    }

    void ExitedComputer()
    {
        CameraMove.Instance._canHover = true;
    }

    private IEnumerator LerpTransform(Transform targetTrans, Action actionCallback)
    {
        Transform startTransform = CameraMove.Instance.transform;

        Vector3 startPosition = startTransform.position;
        Quaternion startRotation = startTransform.rotation;
         
        Vector3 targetPosition = targetTrans.position;
        Quaternion targetRotation = targetTrans.rotation;

        float elapsedTime = 0f;

        while (elapsedTime < lerpTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / lerpTime;

            float curveValue = lerpAnimation.Evaluate(t);

            startTransform.position = Vector3.Lerp(startPosition, targetPosition, curveValue);
            startTransform.rotation = Quaternion.Lerp(startRotation, targetRotation, curveValue);

            yield return null;
        } 

        actionCallback?.Invoke();
    }
}
