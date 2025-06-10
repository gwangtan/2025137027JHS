using UnityEngine;

public class CursorLocker : MonoBehaviour
{
    void Start()
    {
        // 커서를 게임 창 중앙에 고정하고 숨깁니다.
        // Cursor.lockState = CursorLockMode.Locked; 
        // Cursor.visible = false; 

        // 2D 탑뷰 게임에서는 보통 커서가 보여야 하므로,
        // 아래와 같이 '클립 모드'만 설정하는 것이 더 적합할 수 있습니다.
        Cursor.lockState = CursorLockMode.Confined; // 커서를 게임 창 안으로 제한합니다.
        Cursor.visible = true; // 커서를 보이게 합니다.
    }

    void Update()
    {
        // 예를 들어 Esc 키를 누르면 커서 제한을 해제
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None; // 커서 제한 해제
            Cursor.visible = true; // 커서 보이기
        }
    }
}