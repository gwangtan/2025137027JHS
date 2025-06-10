using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float scrollSpeed = 1f; // 배경 스크롤 속도
    public float tileSizeY; // 배경 타일 하나의 Y축 크기 (World Units)

    private Vector3 startPosition; // 시작 위치

    void Start()
    {
        startPosition = transform.position; // 현재 위치를 시작 위치로 저장
        // tileSizeY를 자동으로 찾으려면 Sprite Renderer의 size.y를 사용 (단, Sprite가 타일맵이라면 타일맵 전체 높이가 아닌 단일 타일 높이)
        // 예를 들어, 하나의 배경 스프라이트 오브젝트에 이 스크립트를 붙였다면:
        // SpriteRenderer sr = GetComponent<SpriteRenderer>();
        // if (sr != null)
        // {
        //     tileSizeY = sr.bounds.size.y;
        // }
        // 여러 타일이 합쳐진 배경이라면, 직접 '타일 하나의 높이'를 설정해야 합니다.
    }

    void Update()
    {
        // Y축으로 스크롤 (deltaTime을 곱하여 프레임 속도에 독립적)
        float newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileSizeY);

        // 새 위치로 이동 (시작 위치에서 offsetY만큼 이동)
        transform.position = startPosition + Vector3.down * newPosition;
        // Vector3.down 대신 Vector3.up을 쓰면 위로 스크롤됩니다.
    }
}