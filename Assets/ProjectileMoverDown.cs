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
            Debug.Log("Mover가 'End' 태그 오브젝트와 충돌!");
            // Mover(투사체) 자신을 파괴합니다.
            Destroy(gameObject);
        }
        // ... (데미지 로직 등 필요하면 추가)
    }
}