using System.Collections;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    private Vector3 targetPosition;
    private float moveSpeed;
    private bool canMove;
    private PooledObject pooledObject;
    private bool hasReleased;
    private TrailRenderer trail;

    private void Awake()
    {
        pooledObject = GetComponent<PooledObject>();
        trail = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        if (!canMove || hasReleased) return;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        if ((targetPosition - transform.position).sqrMagnitude < .01f)
            canMove = false;
    }

    public void Initialized(Vector3 targetPosition, float moveSpeed, float lifeTime)
    {
        trail.Clear();
        this.targetPosition = targetPosition;
        this.moveSpeed = moveSpeed;
        canMove = true;
        hasReleased = false;
        StartCoroutine(SelfDestroyRoutine(new WaitForSeconds(lifeTime)));
    }

    private IEnumerator SelfDestroyRoutine(WaitForSeconds waitTime)
    {
        yield return waitTime;

        if (pooledObject != null)
        {
            hasReleased = true;
            pooledObject.ReleasePool(gameObject);
        }
        else
            Destroy(gameObject);
    }
}
