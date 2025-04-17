using UnityEngine;
using Cinemachine;
using System.Collections;

public class BossCameraFocus : MonoBehaviour
{
    public CinemachineVirtualCamera playerCamera; // �÷��̾ ����ٴϴ� ī�޶�
    public CinemachineVirtualCamera bossFocusCamera; // ������ ���� ī�޶�
    public Transform bossTransform; // ���� ������Ʈ�� Transform
    public float focusDuration = 2f; // ������ ���ߴ� �ð�
    public float shakeAmplitude = 5f; // ��鸲 ����
    public float shakeFrequency = 10f; // ��鸲 ��
    public float shakeDuration = 1f; // ��鸲 ���� �ð�

    private CinemachineBasicMultiChannelPerlin playerNoise;

    void Start()
    {
        if (playerCamera != null)
        {
            playerNoise = playerCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (playerNoise == null)
            {
                Debug.LogError("Player ī�޶� CinemachineBasicMultiChannelPerlin ������Ʈ�� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogError("Player ī�޶� �Ҵ���� �ʾҽ��ϴ�.");
        }

        if (bossFocusCamera == null)
        {
            Debug.LogError("���� ��Ŀ�� ī�޶� �Ҵ���� �ʾҽ��ϴ�.");
        }

        if (bossTransform == null)
        {
            Debug.LogError("���� Transform�� �Ҵ���� �ʾҽ��ϴ�.");
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
        // 1. ���� ī�޶� Ȱ��ȭ, �÷��̾� ī�޶� ��Ȱ��ȭ
        bossFocusCamera.Priority = playerCamera.Priority + 1;

        // 2. ��� ���� ���� ���߱�
        yield return new WaitForSeconds(focusDuration);

        // 3. �÷��̾� ī�޶� Ȱ��ȭ, ���� ī�޶� ��Ȱ��ȭ
        playerCamera.Priority = bossFocusCamera.Priority + 1;

        // 4. ī�޶� ��鸲 ȿ�� ����
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