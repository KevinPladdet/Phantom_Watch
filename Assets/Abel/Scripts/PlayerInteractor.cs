using UnityEngine;

interface IInteractable
{
    public void Interact();
}

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private float _maxClickDistance = 100f;

    private void Update()
    {
        // this nesting sucks. someone please restructure this as I have no idea how to make it better without breaking it
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, _maxClickDistance))
            {
                IInteractable interactObj;
                if (hit.transform.TryGetComponent(out interactObj))
                {
                    interactObj.Interact();
                }
                if (hit.transform.parent != null)
                {
                    if (hit.transform.parent.TryGetComponent(out interactObj))
                    {
                        interactObj.Interact();
                    }
                }
            }
        }
    }
}
