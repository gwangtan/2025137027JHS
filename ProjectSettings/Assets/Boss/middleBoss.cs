using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossController : MonoBehaviour
{
    public float trackingSpeed = 1f;
    public GameObject projectilePrefab;
    public float initialProjectileSpawnInterval = 3f; // �ʱ� �߻� ����
    public float minProjectileSpawnInterval = 0.1f; // �ּ� �߻� ����
    public float spawnIntervalDecreaseAmount = 0.7f; // �߻� ���� ���ҷ�
    public float spawnIntervalDecreaseInterval = 0.2f; // �߻� ���� ���� ����
    public float initialProjectileTrackingDuration = 2f; // �ʱ� ���� �ð�
    public float projectileTrackingDecreaseRate = 0.2f; // ���� �ð� ������
    public float trackingDecreaseInterval = 5f; // ���� �ð� ���� ����
    public int maxHealth = 12;
    public GameObject spawnObjectOnDeath;
    public float deathSpawnDelay = 2f;
    public float lifeTime = 60f; // ������ ���� �ð� (��)

    private Transform playerTransform;
    private float lastProjectileSpawnTime;
    private int currentHealth;
    private float currentProjectileTrackingDuration;
    private float lastTrackingDecreaseTime;
    private float spawnTime; // ������ ������ �ð�
    private float currentProjectileSpawnInterval; // ���� �߻� ����
    private float lastSpawnIntervalDecreaseTime; // ������ �߻� ���� ���� �ð�

    void Start()
    {
        // Player ������Ʈ�� ã���ϴ�. "Player" �±װ� �����Ǿ� �־�� �մϴ�.
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (playerTransform == null)
        {
            Debug.LogError("Player ������Ʈ�� ã�� �� �����ϴ�. Player ������Ʈ�� 'Player' �±׸� �߰��ߴ��� Ȯ���ϼ���.");
            enabled = false; // ��ũ��Ʈ ��Ȱ��ȭ
            return;
        }

        lastProjectileSpawnTime = Time.time;
        currentHealth = maxHealth;
        currentProjectileTrackingDuration = initialProjectileTrackingDuration;
        lastTrackingDecreaseTime = Time.time;
        spawnTime = Time.time; // ���� �ð� ���
        currentProjectileSpawnInterval = initialProjectileSpawnInterval; // �ʱ� �߻� ���� ����
        lastSpawnIntervalDecreaseTime = Time.time; // �ʱ� ���� �ð� ����
    }

    void Update()
    {
        // �÷��̾ �����ϸ� ���� �� ���� ���� ����
        if (playerTransform != null)
        {
            // ���� �ð� Ȯ��
            if (Time.time >= spawnTime + lifeTime)
            {
                // ���� �ð��� �� �Ǹ� ���� ó��
                SelfDestruct();
                return; // �� �̻� ������Ʈ�� �������� ����
            }

            // X ��ǥ�� 10���� �����ϰ� Y ��ǥ�� ����
            transform.position = new Vector2(10f, Mathf.MoveTowards(transform.position.y, playerTransform.position.y, trackingSpeed * Time.deltaTime));

            // ����ü �߻� ���� ���� ����
            if (Time.time >= lastSpawnIntervalDecreaseTime + spawnIntervalDecreaseInterval)
            {
                currentProjectileSpawnInterval -= spawnIntervalDecreaseAmount;
                if (currentProjectileSpawnInterval < minProjectileSpawnInterval)
                {
                    currentProjectileSpawnInterval = minProjectileSpawnInterval; // �ּ� ���� ����
                }
                lastSpawnIntervalDecreaseTime = Time.time;
                Debug.Log("�߻� ���� ����: " + currentProjectileSpawnInterval); // ������ �α�
            }

            // ����ü �߻� ���� Ȯ��
            if (Time.time >= lastProjectileSpawnTime + currentProjectileSpawnInterval)
            {
                SpawnHomingProjectiles();
                lastProjectileSpawnTime = Time.time;
            }

            // ���� �ð� ���� ����
            if (Time.time >= lastTrackingDecreaseTime + trackingDecreaseInterval)
            {
                currentProjectileTrackingDuration -= projectileTrackingDecreaseRate;
                if (currentProjectileTrackingDuration < 0f)
                {
                    currentProjectileTrackingDuration = 0f; // �ּ� 0���� ����
                }
                lastTrackingDecreaseTime = Time.time;
                Debug.Log("���� �ð� ����: " + currentProjectileTrackingDuration); // ������ �α�
            }
        }
    }

    void SpawnHomingProjectiles()
    {
        if (projectilePrefab != null)
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                HomingProjectile projectileScript = projectile.GetComponent<HomingProjectile>();
                if (projectileScript != null)
                {
                    projectileScript.SetTarget(playerTransform, currentProjectileTrackingDuration);
                }
                else
                {
                    Debug.LogError("������ ����ü ������Ʈ�� 'HomingProjectile' ��ũ��Ʈ�� �����ϴ�.");
                    Destroy(projectile); // ��ũ��Ʈ�� ������ ������ ����ü ����
                }
            }
        }
        else
        {
            Debug.LogError("Projectile Prefab�� �Ҵ���� �ʾҽ��ϴ�.");
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // 2�� �ڿ� Ư�� ������Ʈ ���� (���� ����)
        if (spawnObjectOnDeath != null)
        {
            Invoke("SpawnDeathObject", deathSpawnDelay);
        }
        Destroy(gameObject);
    }

    void SpawnDeathObject()
    {
        Instantiate(spawnObjectOnDeath, new Vector2(7f, 7f), Quaternion.identity);
    }

    void SelfDestruct()
    {
        // ���� ���� ��ġ�� x��ǥ + 3
        Vector2 spawnPosition = new Vector2(transform.position.x + 3f, transform.position.y);

        // Ư�� ������Ʈ ����
        if (spawnObjectOnDeath != null)
        {
            Instantiate(spawnObjectOnDeath, spawnPosition, Quaternion.identity);
        }

        // ���� ����
        Destroy(gameObject);
    }
}