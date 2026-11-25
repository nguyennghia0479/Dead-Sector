using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SurfaceHitEffect
{
    [Header("Surface Hit Settings")]
    [SerializeField] private GameObject defaultVFX;
    [SerializeField] private float lifeTime = 5f;

    [Space]
    [SerializeField] private HitEffect[] hitEffects;

    private Dictionary<PhysicsMaterial, HitEffect> physicsMaterialDic = new();
    private Dictionary<string, HitEffect> tagDic = new();
    private WaitForSeconds waitTime;

    public void Initialized()
    {
        waitTime = new WaitForSeconds(lifeTime);

        foreach (var hitEffect in hitEffects)
        {
            if (hitEffect.physicsMaterials != null && hitEffect.physicsMaterials.Length > 0)
            {
                foreach (var material in hitEffect.physicsMaterials)
                {
                    if (!physicsMaterialDic.ContainsKey(material))
                        physicsMaterialDic.Add(material, hitEffect);
                }
            }

            if (hitEffect.tags != null && hitEffect.tags.Length > 0)
            {
                foreach (var tag in hitEffect.tags)
                {
                    if (!tagDic.ContainsKey(tag))
                        tagDic.Add(tag, hitEffect);
                }
            }
        }
    }

    public void ApplyHitEffect(RaycastHit hit)
    {
        if (hit.collider == null) return;

        HitEffect hitEffect = GetHitEffect(hit);
        GameObject hitVFX = hitEffect != null ? hitEffect.hitVFX : defaultVFX;
        float volume = hitEffect != null ? hitEffect.volume : 1f;

        if (hitVFX != null)
        {
            GameObject hitPool = ObjectPoolManager.Instance.GetPool(hitVFX, hit.point, Quaternion.LookRotation(hit.normal));
            hitPool.transform.SetParent(hit.transform);
            if (hitPool != null && hitPool.TryGetComponent<HitVFX>(out var newHitVFX))
            {
                newHitVFX.Initialized(waitTime);
                GameEvents.RaiseBulletHit(newHitVFX.SoundType, hit.point, volume);
            }
        }
    }

    private HitEffect GetHitEffect(RaycastHit hit)
    {
        PhysicsMaterial hitMaterial = hit.collider.material;
        if (hitMaterial != null && physicsMaterialDic.TryGetValue(hitMaterial, out HitEffect effect))
            return effect;

        if (tagDic.TryGetValue(hit.collider.tag, out effect))
            return effect;

        return null;
    }
}

[System.Serializable]
public class HitEffect
{
    public PhysicsMaterial[] physicsMaterials;
    public string[] tags;
    public GameObject hitVFX;
    public float volume = 1;
}
