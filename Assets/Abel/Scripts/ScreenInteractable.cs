using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenInteractable : MonoBehaviour, IInteractable
{
    public static ScreenInteractable instance;

    [SerializeField] private GameObject ghoul;
    [SerializeField] private GameObject ghoul2;

    private void Awake()
    {
        if(instance == null) { instance = this; } else
        {
            Destroy(this);
        }
    }

    bool canInteract = true;
    public void TempDisable()
    {
        StartCoroutine(DelayEnable());
    }
    public void Disable()
    {
        canInteract = false;
        StartCoroutine(DelayEnable());
    }
    IEnumerator DelayEnable()
    {
        canInteract = false;
        yield return new WaitForSeconds(1);
        canInteract = true;
    }

    bool interacted = false;
    public void Interact()
    {
        if (!canInteract) { return; }

        if (!interacted)
        {
            interacted = true;

            ModeSwapper.instance.EnterComputer();
            StartCoroutine(ToggleInteractable());
        }
    }

    IEnumerator ToggleInteractable()
    {
        yield return new WaitForSeconds(0.3f);
        // Disable ghoul when entering computer
        if (ghoul.activeInHierarchy)
        {
            ghoul.SetActive(false);
        }
        if (ghoul2.activeInHierarchy)
        {
            ghoul2.SetActive(false);
        }
        yield return new WaitForSeconds(1.7f);
        interacted = false;
    }
}
