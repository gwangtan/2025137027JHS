using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // 플레이어 이동 속도

    private Rigidbody2D rb;
    private Camera mainCamera; // 메인 카메라 참조

    // 화면 경계 계산을 위한 변수
    private float minX, maxX, minY, maxY;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("PlayerMovement: Rigidbody2D 컴포넌트가 플레이어에 없습니다!");
            enabled = false; // Rigidbody2D 없으면 스크립트 비활성화
            return;
        }

        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera를 찾을 수 없습니다. 'MainCamera' 태그가 있는지 확인하세요.");
            enabled = false; // 카메라 없으면 스크립트 비활성화
            return;
        }

        // 게임 시작 시 카메라 화면 경계 계산
        CalculateCameraBounds();
    }

    void CalculateCameraBounds()
    {
        // 카메라 뷰포트 (0,0)은 좌측 하단, (1,1)은 우측 상단
        // ViewportToWorldPoint는 Z값을 필요로 합니다.
        // 플레이어의 Z 위치를 사용하여 2D 평면과 일치시킵니다.
        float zDepth = transform.position.z;

        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, zDepth));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, zDepth));

        minX = bottomLeft.x;
        maxX = topRight.x;
        minY = bottomLeft.y;
        maxY = topRight.y;

        // 플레이어 콜라이더의 절반 크기를 경계에 추가하여 
        // 플레이어의 가장자리가 화면 경계에 닿도록 조정 (옵션)
        // BoxCollider2D playerCollider = GetComponent<BoxCollider2D>();
        // if (playerCollider != null)
        // {
        //     float halfWidth = playerCollider.size.x / 2f;
        //     float halfHeight = playerCollider.size.y / 2f;
        //     minX += halfWidth;
        //     maxX -= halfWidth;
        //     minY += halfHeight;
        //     maxY -= halfHeight;
        // }
    }

    void FixedUpdate() // 물리 업데이트는 FixedUpdate에서 처리하는 것이 좋습니다.
    {
        Vector3 mouseScreenPosition = Input.mousePosition;

        // Z값을 플레이어의 Z 위치로 설정하여 2D 평면에서 변환
        mouseScreenPosition.z = mainCamera.WorldToScreenPoint(transform.position).z;
        // 또는 그냥 mouseScreenPosition.z = transform.position.z; 로 해도 되지만 정확한 Zdepth는 이렇게 구하는 것이 좋습니다.

        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);

        // --- 새로 추가된 부분: 목표 위치를 화면 경계 안으로 제한 ---
        // Mathf.Clamp를 사용하여 마우스 목표 위치를 카메라 뷰포트 내로 제한
        mouseWorldPosition.x = Mathf.Clamp(mouseWorldPosition.x, minX, maxX);
        mouseWorldPosition.y = Mathf.Clamp(mouseWorldPosition.y, minY, maxY);
        // --- 끝 ---

        // 현재 위치에서 마우스 위치로 부드럽게 이동할 목표 지점 계산
        Vector3 targetLerpPosition = Vector3.Lerp(rb.position, mouseWorldPosition, moveSpeed * Time.fixedDeltaTime);

        // Rigidbody2D.MovePosition을 사용하여 물리적으로 이동
        rb.MovePosition(targetLerpPosition);
    }

    // OnCollisionEnter2D와 OnCollisionExit2D는 그대로 두세요.
    // 벽에 닿았을 때 물리적으로 막히는 것은 Rigidbody2D.MovePosition이 처리합니다.
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("플레이어가 충돌! 상대방: " + collision.gameObject.name + " (Layer: " + collision.gameObject.layer + ")");
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("플레이어가 충돌 해제! 상대방: " + collision.gameObject.name);
    }
}