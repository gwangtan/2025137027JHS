using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BossAttackAI : MonoBehaviour
{
    // --- 보스 이동 관련 설정 ---
    public float moveSpeed = 3f;
    public float moveInterval = 2f;

    [Header("이동 범위")]
    public float minX = -8f;
    public float maxX = 8f;
    public float minY = 0f;
    public float maxY = 5f;

    private Vector3 targetPosition;
    private float nextMoveTime;

    // --- 투사체 공격 설정 ---
    [Header("투사체 설정")]
    public GameObject projectilePrefab1;
    public GameObject projectilePrefab2;
    public GameObject projectilePrefab3;
    public GameObject projectilePrefab4;
    public GameObject projectilePrefab5;
    public GameObject projectilePrefab6;

    public float attackInterval = 3f;
    public float projectileSpeed = 8f;

    [Header("투사체 3 & 4 전용 설정")]
    public float orbitRadius = 0.4f;
    public float orbitSpeed = 180f;
    public float orbitDuration = 2f;

    // --- 투사체 6번 전용 설정 ---
    [Header("투사체 6번 전용 설정")]
    public float hexFormationRadius = 0.6f;
    public float hexOrbitSpeed = 120f;
    public float hexInitialDelay = 1f;
    public float hexShotInterval = 0.5f;
    public float hexCooldownAfterFire = 3f;

    private float nextAttackTime;
    private bool isHexAttackOnCooldown = false;
    private bool isHexFormedAndOrbiting = false;
    private List<GameObject> hexProjectiles = new List<GameObject>();
    private float currentHexOrbitAngle = 0f;

    // --- 씬 초기화 관련 설정 ---
    [Header("씬 초기화 설정")]
    public int maxPlayerHits = 3;
    private int currentPlayerHits = 0;

    void Start()
    {
        SetNewTargetPosition();
        nextMoveTime = Time.time + moveInterval;
        nextAttackTime = Time.time + attackInterval;

        StartCoroutine(FormHexProjectilesAndOrbit());
        currentPlayerHits = 0;
    }

    void Update()
    {
        // --- 이동 로직 ---
        if (Time.time >= nextMoveTime)
        {
            SetNewTargetPosition();
            nextMoveTime = Time.time + moveInterval;
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // --- 6번 투사체 회전 로직 ---
        if (isHexFormedAndOrbiting && hexProjectiles.Count > 0)
        {
            currentHexOrbitAngle += hexOrbitSpeed * Time.deltaTime;
            for (int i = 0; i < hexProjectiles.Count; i++)
            {
                GameObject hexP = hexProjectiles[i];
                if (hexP != null && hexP.transform.parent == this.transform)
                {
                    float angle = (i * 60f) + currentHexOrbitAngle;
                    Vector3 orbitOffset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * hexFormationRadius;
                    hexP.transform.localPosition = orbitOffset;
                }
            }
        }

        // --- 공격 로직 ---
        if (Time.time >= nextAttackTime)
        {
            ChooseAndExecuteAttackPattern();
            nextAttackTime = Time.time + attackInterval;
        }
    }

    void SetNewTargetPosition()
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        targetPosition = new Vector3(randomX, randomY, transform.position.z);
        Debug.Log("보스의 새로운 목표 지점: " + targetPosition);
    }

    void ChooseAndExecuteAttackPattern()
    {
        int attackChoice = (!isHexAttackOnCooldown && isHexFormedAndOrbiting && hexProjectiles.Count > 0) ? Random.Range(0, 4) : Random.Range(0, 3);

        switch (attackChoice)
        {
            case 0:
                StartCoroutine(AttackPattern1And2());
                break;
            case 1:
                StartCoroutine(AttackPattern3And4());
                break;
            case 2:
                if (projectilePrefab5 != null)
                {
                    GameObject p5 = Instantiate(projectilePrefab5, transform.position, Quaternion.identity);
                    Rigidbody2D rb5 = p5.GetComponent<Rigidbody2D>();
                    if (rb5 != null) rb5.velocity = Vector2.down * projectileSpeed;
                }
                break;
            case 3:
                StartCoroutine(ShootHexProjectiles());
                break;
        }
    }

    IEnumerator AttackPattern1And2()
    {
        if (projectilePrefab1 != null)
        {
            GameObject p1 = Instantiate(projectilePrefab1, transform.position, Quaternion.identity);
            Rigidbody2D rb1 = p1.GetComponent<Rigidbody2D>();
            if (rb1 != null) rb1.velocity = Vector2.down * projectileSpeed;
        }
        yield return new WaitForSeconds(1f);
        if (projectilePrefab2 != null)
        {
            GameObject p2_1 = Instantiate(projectilePrefab2, transform.position, Quaternion.identity);
            GameObject p2_2 = Instantiate(projectilePrefab2, transform.position + new Vector3(0.5f, 0, 0), Quaternion.identity);
            Rigidbody2D rb2_1 = p2_1.GetComponent<Rigidbody2D>();
            Rigidbody2D rb2_2 = p2_2.GetComponent<Rigidbody2D>();
            if (rb2_1 != null) rb2_1.velocity = Vector2.down * projectileSpeed;
            if (rb2_2 != null) rb2_2.velocity = Vector2.down * projectileSpeed;
        }
    }

    IEnumerator AttackPattern3And4()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) yield break;

        GameObject p3 = projectilePrefab3 != null ? Instantiate(projectilePrefab3, transform.position, Quaternion.identity) : null;
        GameObject p4 = projectilePrefab4 != null ? Instantiate(projectilePrefab4, transform.position, Quaternion.identity) : null;

        if (p3 != null) p3.transform.parent = this.transform;
        if (p4 != null) p4.transform.parent = this.transform;

        float currentOrbitAngle = 0f;
        float startTime = Time.time;

        while (Time.time < startTime + orbitDuration)
        {
            currentOrbitAngle += orbitSpeed * Time.deltaTime;
            Vector3 offset1 = new Vector3(Mathf.Cos(currentOrbitAngle * Mathf.Deg2Rad), Mathf.Sin(currentOrbitAngle * Mathf.Deg2Rad), 0) * orbitRadius;
            Vector3 offset2 = new Vector3(Mathf.Cos((currentOrbitAngle + 180) * Mathf.Deg2Rad), Mathf.Sin((currentOrbitAngle + 180) * Mathf.Deg2Rad), 0) * orbitRadius;

            if (p3 != null) p3.transform.localPosition = offset1;
            if (p4 != null) p4.transform.localPosition = offset2;

            yield return null;
        }

        GameObject finalPlayer = GameObject.FindGameObjectWithTag("Player");
        if (finalPlayer == null) { Destroy(p3); Destroy(p4); yield break; }

        if (p3 != null)
        {
            p3.transform.parent = null;
            Rigidbody2D rb3 = p3.GetComponent<Rigidbody2D>();
            if (rb3 != null) rb3.velocity = (finalPlayer.transform.position - p3.transform.position).normalized * projectileSpeed;
        }
        if (p4 != null)
        {
            p4.transform.parent = null;
            Rigidbody2D rb4 = p4.GetComponent<Rigidbody2D>();
            if (rb4 != null) rb4.velocity = (finalPlayer.transform.position - p4.transform.position).normalized * projectileSpeed;
        }
    }

    IEnumerator FormHexProjectilesAndOrbit()
    {
        foreach (GameObject p in hexProjectiles) if (p != null) Destroy(p);
        hexProjectiles.Clear();

        yield return new WaitForSeconds(hexInitialDelay);

        for (int i = 0; i < 6; i++)
        {
            float angle = i * 60f;
            Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * hexFormationRadius;
            Vector3 spawnPos = transform.position + offset;

            if (projectilePrefab6 != null)
            {
                GameObject hexP = Instantiate(projectilePrefab6, spawnPos, Quaternion.identity);
                hexP.transform.parent = this.transform;
                hexProjectiles.Add(hexP);
            }
        }

        isHexFormedAndOrbiting = true;
        isHexAttackOnCooldown = false;
    }

    IEnumerator ShootHexProjectiles()
    {
        isHexAttackOnCooldown = true;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) { isHexAttackOnCooldown = false; yield break; }

        List<GameObject> activeProjectiles = new List<GameObject>();
        foreach (GameObject p in hexProjectiles) if (p != null && p.transform.parent == this.transform) activeProjectiles.Add(p);

        if (activeProjectiles.Count == 0)
        {
            isHexAttackOnCooldown = false;
            StartCoroutine(FormHexProjectilesAndOrbit());
            yield break;
        }

        foreach (GameObject hexP in activeProjectiles)
        {
            if (hexP != null)
            {
                hexP.transform.parent = null;
                Rigidbody2D rb = hexP.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.bodyType = RigidbodyType2D.Dynamic;
                    rb.gravityScale = 0f;
                    rb.isKinematic = false;
                    rb.velocity = (player.transform.position - hexP.transform.position).normalized * projectileSpeed;
                }
            }
            yield return new WaitForSeconds(hexShotInterval);
        }

        hexProjectiles.RemoveAll(p => p == null || p.transform.parent == null);
        StartCoroutine(FormHexProjectilesAndOrbit());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            currentPlayerHits++;
            if (currentPlayerHits >= maxPlayerHits)
            {
                RestartCurrentScene();
            }
        }

        PlayerProjectile playerProjectile = other.GetComponent<PlayerProjectile>();
        if (playerProjectile != null)
        {
            BossHealth bossHealth = GetComponent<BossHealth>();
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(playerProjectile.damageAmount);
            }

            Destroy(other.gameObject);
        }
    }

    void RestartCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
