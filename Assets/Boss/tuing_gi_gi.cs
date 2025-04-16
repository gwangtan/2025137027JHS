using UnityEngine;

public class LaunchDiagonal : MonoBehaviour
{
    public float launchForce = 5f; // 날아갈 힘의 크기
    public Vector2 launchDirection = new Vector2(1f, 1f).normalized; // 날아갈 방향 (기본: 오른쪽 위 대각선)

    private void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Start 함수에서 바로 힘을 가해 날아가도록 합니다.
            rb.AddForce(launchDirection * launchForce, ForceMode2D.Impulse);
        }
        else
        {
            Debug.LogError(gameObject.name + " 오브젝트에 Rigidbody2D 컴포넌트가 없습니다. 날아갈 수 없습니다.");
            // Rigidbody가 없으면 스크립트를 비활성화하여 불필요한 Update를 막습니다.
            enabled = false;
        }
    }

    // 필요하다면 다른 조건에 따라 발사되도록 수정할 수 있습니다.
    // 예를 들어, 특정 키 입력 시 발사되도록 하려면 Update 함수에서 입력 처리를 하면 됩니다.
    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         Rigidbody2D rb = GetComponent<Rigidbody2D>();
    //         if (rb != null)
    //         {
    //             rb.AddForce(launchDirection * launchForce, ForceMode2D.Impulse);
    //         }
    //     }
    // }
}