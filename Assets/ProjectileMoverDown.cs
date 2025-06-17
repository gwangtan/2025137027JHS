// ProjectileMoverDown.cs
using UnityEngine;

public class ProjectileMoverDown : MonoBehaviour
{
    public float moveSpeed = 4f;

    void Update()
    {
        transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("End"))
        {
            Debug.Log("Mover�� 'End' �±� ������Ʈ�� �浹!");
            // Mover(����ü) �ڽ��� �ı��մϴ�.
            Destroy(gameObject);
        }
        // ... (������ ���� �� �ʿ��ϸ� �߰�)
    }
}