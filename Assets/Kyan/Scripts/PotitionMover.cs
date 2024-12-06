using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


public class PotitionMover : MonoBehaviour
{
    [SerializeField] float moveTime = 1;
    [SerializeField] AnimationCurve animationCurve;
    [SerializeField] GameObject objectToMove;

    [SerializeField] Transform defaultTransform;
    [SerializeField] Transform enteredTransform;

    [SerializeField] bool entered = false;
    [SerializeField] bool isEntering = false;
    [SerializeField] bool isExiting = false;

    [SerializeField] bool enter = false;
    [SerializeField] bool exit = false;

    //Gebruik Enter() en Exit() om te gebruiken
    //Verander animation curve voor smoothness
    //verander moveTime voor length movement

    private void Update()
    {
        if (enter) { enter = false; Enter(); }
        if (exit) { exit = false; Exit(); }
    }

    public void Enter()
    {
        if (isEntering || entered) { return; }
        isEntering = true;

        //Doe hier code voordat je erin gaat
        StartCoroutine(LerpTransform(enteredTransform, Entered));
    }
    void Entered()
    {
        isEntering = false;
        entered = true;
        //Doe hier wat code om zegmaar te doen wanneer je er bent
    }

    public void Exit()
    {
        if (isExiting || !entered) { return; }
        isExiting = true;

        //Doe hier code net voordat je eruit gaat
        StartCoroutine(LerpTransform(defaultTransform, Exited));
    }

    void Exited()
    {
        isExiting = false;
        entered = false;
        //Doe hier code als je eruit komt
    }

    private IEnumerator LerpTransform(Transform targetTrans, Action actionCallback)
    {
        Transform startTransform = objectToMove.transform;

        Vector3 startPosition = startTransform.position;
        Quaternion startRotation = startTransform.rotation;

        Vector3 targetPosition = targetTrans.position;
        Quaternion targetRotation = targetTrans.rotation;

        float elapsedTime = 0f;

        while (elapsedTime < moveTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveTime;

            float curveValue = animationCurve.Evaluate(t);

            startTransform.position = Vector3.Lerp(startPosition, targetPosition, curveValue);
            startTransform.rotation = Quaternion.Lerp(startRotation, targetRotation, curveValue);

            yield return null;
        }

        actionCallback?.Invoke();
    }
}
