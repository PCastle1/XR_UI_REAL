using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ResizeObjectOnGrab : XRGrabInteractable
{
    public float resizeFactor = 0.5f; // 물체의 크기를 조절할 비율

    private Vector3 originalScale;
    private bool isResizing = false;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        originalScale = transform.localScale; // 물체의 원래 크기 기억
        isResizing = true; // 크기 조절 시작
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        isResizing = false; // 크기 조절 종료
    }

    void Update()
    {
        // VR 컨트롤러의 트리거 입력을 감지하여 크기 조절
        if (isResizing && selectingInteractor is XRDirectInteractor && Input.GetAxis("Oculus_CrossPlatform_SecondaryIndexTrigger") > 0.5f)
        {
            ResizeObject();
        }
    }

    // 물체의 크기를 조절하는 메서드
    private void ResizeObject()
    {
        Vector3 newScale = originalScale * resizeFactor;

        // 크기가 너무 작지 않도록 최소 크기 제한
        if (newScale.magnitude > Vector3.one.magnitude * 0.1f)
        {
            transform.localScale = newScale;
        }
    }
}
