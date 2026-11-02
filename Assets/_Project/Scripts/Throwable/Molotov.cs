using UnityEngine;

public class Molotov : Throwable
{
    protected override void OnCollisionEnter(Collision collision)
    {
        if (hasCollided) return;

        hasCollided = true;
        AudioSource.PlayClipAtPoint(bounceClip, transform.position);
        Explode();
    }

    protected override void Explode()
    {
        explosionEffect.gameObject.SetActive(true);
        Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
