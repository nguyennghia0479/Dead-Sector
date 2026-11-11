using System.Collections;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    private Vector3 targetPosition;
    private float moveSpeed;
    private bool canMove;

    private void Update()
    {
        if (!canMove) return;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        if ((targetPosition - transform.position).sqrMagnitude < .01f)
        {
            canMove = false;
        }
    }

    public void SetupBullet(Vector3 targetPosition, float moveSpeed, float lifeTime)
    {
        this.targetPosition = targetPosition;
        this.moveSpeed = moveSpeed;
        canMove = true;
        StartCoroutine(SelfDestroyRoutine(new WaitForSeconds(lifeTime)));
    }

    private IEnumerator SelfDestroyRoutine(WaitForSeconds waitTime)
    {
        yield return waitTime;
        Destroy(gameObject);
    }
}
