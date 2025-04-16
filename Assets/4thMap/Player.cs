using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    // ... 기존 코드 ...

    public bool isInvincible = false;
    public float invincibleDuration = 5f;

    public GameObject effectImagePrefab;
    public float effectDisplayTime = 5f;
    public float effectOffsetY = 0.2f;

    public void ApplyItemEffect()
    {
        StartCoroutine(ActivateInvincible());
        StartCoroutine(DisplayEffectImage()); // DisplayEffectImage 함수 호출
    }

    IEnumerator ActivateInvincible()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleDuration);
        isInvincible = false;
    }

    IEnumerator DisplayEffectImage() // DisplayEffectImage 함수 정의
    {
        Vector3 effectPosition = transform.position + Vector3.up * effectOffsetY;
        GameObject effectImage = Instantiate(effectImagePrefab, effectPosition, Quaternion.identity);

        float timer = 0f;
        while (timer < effectDisplayTime)
        {
            timer += Time.deltaTime;
            effectImage.transform.position = transform.position + Vector3.up * effectOffsetY;
            yield return null;
        }

        Destroy(effectImage);
    }
}