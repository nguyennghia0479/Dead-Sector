using UnityEngine;

public class ADS : MonoBehaviour
{
    private Vector3 defaultPosition;
    private Vector3 defaultRotation;

    public Vector3 DefaultPosition => defaultPosition;
    public Vector3 DefaultRotation => defaultRotation;

    private void Awake()
    {
        defaultPosition = transform.localPosition;
        defaultRotation = transform.localEulerAngles;
    }
}
