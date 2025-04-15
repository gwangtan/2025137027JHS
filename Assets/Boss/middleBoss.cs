using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossController : MonoBehaviour
{
    public float trackingSpeed = 1f;
    public GameObject projectilePrefab;
    public float initialProjectileSpawnInterval = 3f; // 초기 발사 간격
    public float minProjectileSpawnInterval = 0.1f; // 최소 발사 간격
    public float spawnIntervalDecreaseAmount = 0.7f; // 발사 간격 감소량
    public float spawnIntervalDecreaseInterval = 0.2f; // 발사 간격 감소 간격
    public float initialProjectileTrackingDuration = 2f; // 초기 유도 시간
    public float projectileTrackingDecreaseRate = 0.2f; // 유도 시간 감소율
    public float trackingDecreaseInterval = 5f; // 유도 시간 감소 간격
    public int maxHealth = 12;
    public GameObject spawnObjectOnDeath;
    public float deathSpawnDelay = 2f;
    public float lifeTime = 60f; // 보스의 생존 시간 (초)

    private Transform playerTransform;
    private float lastProjectileSpawnTime;
    private int currentHealth;
    private float currentProjectileTrackingDuration;
    private float lastTrackingDecreaseTime;
    private float spawnTime; // 보스가 생성된 시간
    private float currentProjectileSpawnInterval; // 현재 발사 간격
    private float lastSpawnIntervalDecreaseTime; // 마지막 발사 간격 감소 시간

    void Start()
    {
        // Player 오브젝트를 찾습니다. "Player" 태그가 설정되어 있어야 합니다.
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (playerTransform == null)
        {
            Debug.LogError("Player 오브젝트를 찾을 수 없습니다. Player 오브젝트에 'Player' 태그를 추가했는지 확인하세요.");
            enabled = false; // 스크립트 비활성화
            return;
        }

        lastProjectileSpawnTime = Time.time;
        currentHealth = maxHealth;
        currentProjectileTrackingDuration = initialProjectileTrackingDuration;
        lastTrackingDecreaseTime = Time.time;
        spawnTime = Time.time; // 생성 시간 기록
        currentProjectileSpawnInterval = initialProjectileSpawnInterval; // 초기 발사 간격 설정
        lastSpawnIntervalDecreaseTime = Time.time; // 초기 감소 시간 설정
    }

    void Update()
    {
        // 플레이어가 존재하면 추적 및 공격 로직 실행
        if (playerTransform != null)
        {
            // 생존 시간 확인
            if (Time.time >= spawnTime + lifeTime)
            {
                // 생존 시간이 다 되면 자폭 처리
                SelfDestruct();
                return; // 더 이상 업데이트를 진행하지 않음
            }

            // X 좌표를 10으로 고정하고 Y 좌표만 따라감
            transform.position = new Vector2(10f, Mathf.MoveTowards(transform.position.y, playerTransform.position.y, trackingSpeed * Time.deltaTime));

            // 투사체 발사 간격 감소 로직
            if (Time.time >= lastSpawnIntervalDecreaseTime + spawnIntervalDecreaseInterval)
            {
                currentProjectileSpawnInterval -= spawnIntervalDecreaseAmount;
                if (currentProjectileSpawnInterval < minProjectileSpawnInterval)
                {
                    currentProjectileSpawnInterval = minProjectileSpawnInterval; // 최소 간격 유지
                }
                lastSpawnIntervalDecreaseTime = Time.time;
                Debug.Log("발사 간격 감소: " + currentProjectileSpawnInterval); // 디버깅용 로그
            }

            // 투사체 발사 간격 확인
            if (Time.time >= lastProjectileSpawnTime + currentProjectileSpawnInterval)
            {
                SpawnHomingProjectiles();
                lastProjectileSpawnTime = Time.time;
            }

            // 유도 시간 감소 로직
            if (Time.time >= lastTrackingDecreaseTime + trackingDecreaseInterval)
            {
                currentProjectileTrackingDuration -= projectileTrackingDecreaseRate;
                if (currentProjectileTrackingDuration < 0f)
                {
                    currentProjectileTrackingDuration = 0f; // 최소 0으로 유지
                }
                lastTrackingDecreaseTime = Time.time;
                Debug.Log("유도 시간 감소: " + currentProjectileTrackingDuration); // 디버깅용 로그
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
                    Debug.LogError("생성된 투사체 오브젝트에 'HomingProjectile' 스크립트가 없습니다.");
                    Destroy(projectile); // 스크립트가 없으면 생성된 투사체 삭제
                }
            }
        }
        else
        {
            Debug.LogError("Projectile Prefab이 할당되지 않았습니다.");
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
        // 2초 뒤에 특정 오브젝트 생성 (기존 로직)
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
        // 보스 삭제 위치의 x좌표 + 3
        Vector2 spawnPosition = new Vector2(transform.position.x + 3f, transform.position.y);

        // 특정 오브젝트 생성
        if (spawnObjectOnDeath != null)
        {
            Instantiate(spawnObjectOnDeath, spawnPosition, Quaternion.identity);
        }

        // 보스 삭제
        Destroy(gameObject);
    }
}