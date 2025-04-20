using UnityEngine;

public class DropOnPlayerTrigger : MonoBehaviour
{
    public string triggerTag = "LastBoss";  // �������� ������Ʈ�� �±�

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
        rb.gravityScale = 1f; // �߷� �����ؼ� �������� �ϱ�
    }
}