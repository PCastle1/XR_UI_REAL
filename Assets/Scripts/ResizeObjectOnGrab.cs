using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ResizeObjectOnGrab : XRGrabInteractable
{
    public float resizeFactor = 0.5f; // ��ü�� ũ�⸦ ������ ����

    private Vector3 originalScale;
    private bool isResizing = false;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        originalScale = transform.localScale; // ��ü�� ���� ũ�� ���
        isResizing = true; // ũ�� ���� ����
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        isResizing = false; // ũ�� ���� ����
    }

    void Update()
    {
        // VR ��Ʈ�ѷ��� Ʈ���� �Է��� �����Ͽ� ũ�� ����
        if (isResizing && selectingInteractor is XRDirectInteractor && Input.GetAxis("Oculus_CrossPlatform_SecondaryIndexTrigger") > 0.5f)
        {
            ResizeObject();
        }
    }

    // ��ü�� ũ�⸦ �����ϴ� �޼���
    private void ResizeObject()
    {
        Vector3 newScale = originalScale * resizeFactor;

        // ũ�Ⱑ �ʹ� ���� �ʵ��� �ּ� ũ�� ����
        if (newScale.magnitude > Vector3.one.magnitude * 0.1f)
        {
            transform.localScale = newScale;
        }
    }
}
