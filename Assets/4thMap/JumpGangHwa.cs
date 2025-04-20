using UnityEngine;
using System.Collections;

public class ItemEffect : MonoBehaviour
{
    public float effectDuration = 3f; // 효과 지속 시간 (초)
    public float minJumpBoostMultiplier = 0.5f; // 최소 점프력 강화 배율
    public float maxJumpBoostMultiplier = 2.5f; // 최대 점프력 강화 배율
    public GameObject floatingImagePrefab; // 띄울 이미지 프리팹 (선택 사항)
    public string targetTag = "Player"; // 효과를 적용할 대상 태그

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌한 오브젝트의 태그를 확인하여 효과를 적용할 대상인지 확인합니다.
        if (collision.CompareTag(targetTag))
        {
            // 충돌한 오브젝트에서 PlayerMoveScript 컴포넌트를 가져옵니다.
            PlayerMoveScript playerMoveScript = collision.GetComponent<PlayerMoveScript>();
            if (playerMoveScript != null)
            {
                // PlayerMoveScript에 점프 부스트 효과를 적용합니다.
                StartCoroutine(ApplyRandomJumpBoostEffect(playerMoveScript, collision.gameObject));
                // 아이템 효과가 적용되었으므로 아이템 오브젝트를 제거합니다.
                Destroy(gameObject);
            }
            else
            {
                // PlayerMoveScript가 없다면 경고 메시지를 콘솔에 출력합니다.
                Debug.LogWarning("PlayerMoveScript를 찾을 수 없습니다. 태그를 확인하고, PlayerMoveScript가 오브젝트에 있는지 확인하세요.");
            }
        }
    }

    IEnumerator ApplyRandomJumpBoostEffect(PlayerMoveScript playerMoveScript, GameObject targetObject)
    {
        // 원래 점프 힘 저장
        float originalJumpForce = playerMoveScript.jumpForce;

        // 랜덤한 점프력 강화 배율 생성
        float randomJumpBoostMultiplier = Random.Range(minJumpBoostMultiplier, maxJumpBoostMultiplier);

        // 점프 힘 증가 적용
        playerMoveScript.jumpForce *= randomJumpBoostMultiplier;

        // 이미지 띄우기
        if (floatingImagePrefab != null)
        {
            GameObject floatingImage = Instantiate(floatingImagePrefab, targetObject.transform.position + new Vector3(0, 0.6f, 0), Quaternion.identity);
            floatingImage.transform.SetParent(targetObject.transform); // 플레이어의 자식으로 설정
            Destroy(floatingImage, effectDuration); // 이미지를 효과 지속 시간 후에 제거
        }

        yield return new WaitForSeconds(effectDuration);

        // 점프 힘 원래대로 복원
        playerMoveScript.jumpForce = originalJumpForce;
    }
}