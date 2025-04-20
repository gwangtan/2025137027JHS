using UnityEngine;

public class DropOnPlayerTrigger : MonoBehaviour
{
    public string triggerTag = "LastBoss";  // 지나가는 오브젝트의 태그

    private bool hasDropped = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasDropped && other.CompareTag(triggerTag))
        {
            DropFromAbove();
            hasDropped = true;
        }
    }

    private void DropFromAbove()
    {
        Vector3 newPosition = transform.position;
        newPosition.y = 30f;
        transform.position = newPosition;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        rb.gravityScale = 1f; // 중력 적용해서 떨어지게 하기
    }
}