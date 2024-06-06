using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShrinkOnGrab : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private Vector3 originalScale;
    public Vector3 shrunkenScale = new Vector3(0.5f, 0.5f, 0.5f); // 원하는 크기로 변경

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable == null)
        {
            Debug.LogError("XRGrabInteractable component is missing from this game object.");
            return;
        }

        originalScale = transform.localScale;

        grabInteractable.selectEntered.AddListener(OnSelectEntered);
        grabInteractable.selectExited.AddListener(OnSelectExited);
    }

    void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
            grabInteractable.selectExited.RemoveListener(OnSelectExited);
        }
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        transform.localScale = shrunkenScale;
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        transform.localScale = originalScale;
    }
}
