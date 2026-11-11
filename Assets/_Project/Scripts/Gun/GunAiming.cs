using UnityEngine;

[System.Serializable]
public class GunAiming
{
    [Header("Aim Settings")]
    [SerializeField] private Vector3 aimPosition;
    [SerializeField] private Vector3 aimRotation;
    [SerializeField] private float aimMainCamFOV = 45;
    [SerializeField] private float aimingSpeed = 10f;

    [Header("Scope Settings")]
    [SerializeField] private Camera mainCam;
    [SerializeField] private Camera scopeCam;
    [SerializeField] private float additionFOV = 18f;
    [Range(1, 3)]
    [SerializeField] private int magnification = 1;

    [Space]
    [SerializeField] private RenderTexture renderTexture;
    [SerializeField] private Renderer renderer;
    [SerializeField] private int lensMaterialIndex;
    [SerializeField] private Material scopeMat;
    [SerializeField] private Vector2 scopeTiling = Vector2.one;
    [SerializeField] private Vector2 scopeOffset = Vector2.zero;

    [Header("Debug Scope Settings")]
    [SerializeField] private bool useScopeDebug = false;
    [SerializeField] private float lensAspectRatio = 1f;

    private static readonly int BaseMapST = Shader.PropertyToID("_BaseMap_ST");

    private Vector3 hipPosition;
    private Vector3 hipRotation;
    private Material originalLensMat;

    public float AimMainCamFOV => aimMainCamFOV;

    public void Initialized(Vector3 hipPosition, Vector3 hipRotation)
    {
        this.hipPosition = hipPosition;
        this.hipRotation = hipRotation;
    }

    public void InitializedScope(Camera mainCam, Camera scopeCam)
    {
        if (renderer == null) return;

        this.mainCam = mainCam;
        this.scopeCam = scopeCam;
        originalLensMat = renderer.sharedMaterials[lensMaterialIndex];
        CalculateScopeUV(lensAspectRatio);
    }

    private void CalculateScopeUV(float lensAspect)
    {
        if (!useScopeDebug) return;

        float textureAspect = (float)renderTexture.width / renderTexture.height;
        float ratio = lensAspect / textureAspect;

        if (ratio < 1f)
        {
            scopeTiling = new Vector2(ratio, scopeTiling.y);
            scopeOffset = new Vector2((1f - ratio) * 0.5f, 0f);
        }
    }

    public void ApplyAiming(Transform weapon, bool isAiming)
    {
        Vector3 targetPositon = isAiming ? aimPosition : hipPosition;
        Vector3 targetRotation = isAiming ? aimRotation : hipRotation;

        weapon.localPosition = Vector3.Lerp(weapon.localPosition, targetPositon, aimingSpeed * Time.deltaTime);
        weapon.localRotation = Quaternion.Slerp(weapon.localRotation, Quaternion.Euler(targetRotation), aimingSpeed * Time.deltaTime);

        SetScopeAiming(isAiming);
    }

    private void SetScopeAiming(bool isAiming)
    {
        if (mainCam == null || scopeCam == null || renderer == null) return;

        Material[] materials = renderer.sharedMaterials;
        if (isAiming)
        {
            materials[lensMaterialIndex] = scopeMat;
            renderer.sharedMaterials = materials;

            MaterialPropertyBlock block = new();
            renderer.GetPropertyBlock(block, lensMaterialIndex);
            block.SetVector(BaseMapST, new Vector4(
                scopeTiling.x,
                scopeTiling.y,
                scopeOffset.x,
                scopeOffset.y
            ));

            renderer.SetPropertyBlock(block, lensMaterialIndex);
        }
        else
        {
            materials[lensMaterialIndex] = originalLensMat;
            renderer.sharedMaterials = materials;
            renderer.SetPropertyBlock(null, lensMaterialIndex);
        }
    }

    public void HandleScopeAiming()
    {
        if (mainCam == null || scopeCam == null || renderer == null || !scopeCam.enabled)
            return;

        CalculateScopeUV(lensAspectRatio);
        if (magnification == 1)
            scopeCam.fieldOfView = mainCam.fieldOfView + additionFOV;
        else
        {
            float mainFOV = mainCam.fieldOfView + additionFOV;
            scopeCam.fieldOfView = 2f * Mathf.Atan(Mathf.Tan(mainFOV * .5f * Mathf.Deg2Rad) / magnification) * Mathf.Rad2Deg;
        }
    }
}
