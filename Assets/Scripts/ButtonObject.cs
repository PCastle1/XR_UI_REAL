using UnityEngine;
using UnityEngine.Events;

public class ButtonObject : MonoBehaviour
{
    public UnityEvent onClick;

    void OnMouseDown()
    {
        onClick.Invoke();
    }
}