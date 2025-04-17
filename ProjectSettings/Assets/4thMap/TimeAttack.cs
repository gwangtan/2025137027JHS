using UnityEngine;

public class MoveObjectXPositive2D : MonoBehaviour
{
    public float moveSpeed = 1f; // 이동 속도 (기본값: 1)

    void Update()
    {
        // 2D 환경에서는 Vector2를 사용하여 이동합니다.
        // Time.deltaTime을 곱하여 프레임 속도에 독립적인 움직임을 만듭니다.
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
    }
}