using UnityEngine;

public class ScopeCamera : MonoBehaviour
{
    private Camera scopeCam;

    public Camera Camera => scopeCam;

    private void Awake()
    {
        scopeCam = GetComponent<Camera>();
    }
}
