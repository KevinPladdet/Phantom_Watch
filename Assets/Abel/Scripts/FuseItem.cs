using UnityEngine;

public class FuseItem : MonoBehaviour, IInteractable
{
    private readonly string _tagName = "HoldingFuse";
    [SerializeField] private AudioClip _grabSFX;

    public void Interact()
    {
        // Debug.Log("Clicked on " + gameObject.name);

        GameObject obj = GameObject.FindGameObjectWithTag(_tagName);
        if (obj != null)
        {
            GameObject boolObj = null;
            foreach (Transform t in obj.transform)
            {
                boolObj = t.gameObject;
            }

            if (boolObj == null || boolObj.activeInHierarchy)
                return;

            AudioPool.Instance.PlaySound(_grabSFX, 0.75f, false, transform.position);

            boolObj.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
