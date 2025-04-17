using UnityEngine;
using Cinemachine;
using System.Collections;

public class BossCameraFocus : MonoBehaviour
{
    public CinemachineVirtualCamera playerCamera; // 플레이어를 따라다니는 카메라
    public CinemachineVirtualCamera bossFocusCamera; // 보스를 비출 카메라
    public Transform bossTransform; // 보스 오브젝트의 Transform
    public float focusDuration = 2f; // 보스를 비추는 시간
    public float shakeAmplitude = 5f; // 흔들림 강도
    public float shakeFrequency = 10f; // 흔들림 빈도
    public float shakeDuration = 1f; // 흔들림 지속 시간

    private CinemachineBasicMultiChannelPerlin playerNoise;

    void Start()
    {
        if (playerCamera != null)
        {
            playerNoise = playerCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (playerNoise == null)
            {
                Debug.LogError("Player 카메라에 CinemachineBasicMultiChannelPerlin 컴포넌트가 없습니다.");
            }
        }
        else
        {
            Debug.LogError("Player 카메라가 할당되지 않았습니다.");
        }

        if (bossFocusCamera == null)
        {
            Debug.LogError("보스 포커스 카메라가 할당되지 않았습니다.");
        }

        if (bossTransform == null)
        {
            Debug.LogError("보스 Transform이 할당되지 않았습니다.");
        }
    }

    public void FocusOnBoss()
    {
        if (playerCamera != null && bossFocusCamera != null && bossTransform != null)
        {
            StartCoroutine(FocusAndFollow());
        }
    }

    IEnumerator FocusAndFollow()
    {
        // 1. 보스 카메라 활성화, 플레이어 카메라 비활성화
        bossFocusCamera.Priority = playerCamera.Priority + 1;

        // 2. 잠시 동안 보스 비추기
        yield return new WaitForSeconds(focusDuration);

        // 3. 플레이어 카메라 활성화, 보스 카메라 비활성화
        playerCamera.Priority = bossFocusCamera.Priority + 1;

        // 4. 카메라 흔들림 효과 시작
        if (playerNoise != null)
        {
            StartCoroutine(CameraShake());
        }
    }

    IEnumerator CameraShake()
    {
        playerNoise.m_AmplitudeGain = shakeAmplitude;
        playerNoise.m_FrequencyGain = shakeFrequency;

        yield return new WaitForSeconds(shakeDuration);

        playerNoise.m_AmplitudeGain = 3f;
        playerNoise.m_FrequencyGain = 3f;
    }
}