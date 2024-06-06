using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class CubeRaycastSpawner : MonoBehaviour
{
    public GameObject prefabToInstantiate; // 클릭 시 생성할 프리팹
    public Vector3 spawnOffset = new Vector3(0, 1, 0); // 생성 위치 오프셋
    public GameObject Controller; // VR 컨트롤러

    private InputDevice rightController;
    private bool isRTriggerPressed = false;
    private bool prevRTriggerPressed = false;

    void Start()
    {
        InitializeController();
    }

    void InitializeController()
    {
        rightController = GetController(XRNode.RightHand);
    }

    InputDevice GetController(XRNode hand)
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(hand, devices);
        if (devices.Count > 0)
        {
            return devices[0];
        }
        return new InputDevice();
    }

    void Update()
    {
        if (!rightController.isValid)
        {
            InitializeController();
        }

        rightController.TryGetFeatureValue(CommonUsages.triggerButton, out isRTriggerPressed);

        if (isRTriggerPressed && !prevRTriggerPressed)
        {
            RaycastHit hit;
            if (Physics.Raycast(Controller.transform.position, Controller.transform.forward, out hit))
            {
                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    HandleSpawn();
                }
            }
        }
        prevRTriggerPressed = isRTriggerPressed;
    }

    void HandleSpawn()
    {
        if (prefabToInstantiate != null)
        {
            Vector3 spawnPosition = transform.position + spawnOffset;
            Instantiate(prefabToInstantiate, spawnPosition, Quaternion.identity);
            Debug.Log("Prefab instantiated at position: " + spawnPosition);
        }
        else
        {
            Debug.LogWarning("No prefab assigned to instantiate.");
        }
    }
}
