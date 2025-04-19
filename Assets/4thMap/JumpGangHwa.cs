using UnityEngine;
using System.Collections;

public class ItemEffect : MonoBehaviour
{
    public float effectDuration = 3f; // 효과 지속 시간
    public float jumpBoostMultiplier = 1.66f; // 점프력 강화 배율
    public GameObject floatingImagePrefab; // 띄울 이미지 프리팹
    public string targetTag = "Player"; // 효과를 적용할 대상 태그

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 지정된 태그를 가진 오브젝트와 충돌했을 때만 효과 적용
        if (collision.CompareTag(targetTag))
        {
            PlayerMoveScript playerMoveScript = collision.GetComponent<PlayerMoveScript>();
            if (playerMoveScript != null)
            {
                StartCoroutine(ApplyEffects(playerMoveScript, collision.gameObject));
                Destroy(gameObject); // 아이템 오브젝트 제거
            }
            else
            {
                Debug.LogWarning("PlayerMoveScript를 찾을 수 없습니다. 태그를 확인하고, PlayerMoveScript가 오브젝트에 있는지 확인하세요.");
            }
        }
    }

    IEnumerator ApplyEffects(PlayerMoveScript playerMoveScript, GameObject targetObject)
    {
        // 원래 점프력 저장
        float originalJumpForce = playerMoveScript.jumpForce;

        // 점프력 증가 적용
        playerMoveScript.jumpForce *= jumpBoostMultiplier;

        // 이미지 띄우기
        if (floatingImagePrefab != null)
        {
            GameObject floatingImage = Instantiate(floatingImagePrefab, targetObject.transform.position + new Vector3(0, 0.6f, 0), Quaternion.identity);
            floatingImage.transform.SetParent(targetObject.transform); // 플레이어의 자식으로 설정
            Destroy(floatingImage, effectDuration); // 이미지를 효과 지속 시간 후에 제거
        }


        yield return new WaitForSeconds(effectDuration);

        // 점프력 원래대로 복원
        playerMoveScript.jumpForce = originalJumpForce;
    }
}
