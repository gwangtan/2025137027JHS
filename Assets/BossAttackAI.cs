using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class BossAttackAI : MonoBehaviour
{
    // --- ���� �̵� ���� ���� ---
    public float moveSpeed = 3f;
    public float moveInterval = 2f;

    [Header("�̵� ����")]
    public float minX = -8f;
    public float maxX = 8f;
    public float minY = 0f;
    public float maxY = 5f;

    private Vector3 targetPosition;
    private float nextMoveTime;

    // --- ����ü ���� ���� ---
    [Header("����ü ����")]
    public GameObject projectilePrefab1;
    public GameObject projectilePrefab2;
    public GameObject projectilePrefab3;
    public GameObject projectilePrefab4;
    public GameObject projectilePrefab5;
    public GameObject projectilePrefab6;

    public float attackInterval = 3f;
    public float projectileSpeed = 3f;

    // --- ����ü 1 & 2 ���� ������ �߻� ���� ---
    [Header("����ü 1 & 2 ���� ������ �߻� ����")]
    public float hexSpreadRadius12 = 0.5f; // ������ �߽����κ��� ����ü������ �Ÿ�
    [Tooltip("������(6) �Ǵ� ĥ����(7) �� ���ϴ� ����ü ������ �����ϼ���.")]
    public int numberOfProjectiles12 = 7; // �������� ������ ����ü ���� (6���� ����)

    [Header("����ü 3 & 4 ���� ����")]
    public float orbitRadius = 0.4f;
    public float orbitSpeed = 180f;
    public float orbitDuration = 2f;

    // --- ����ü 6�� ���� ���� ---
    [Header("����ü 6�� ���� ����")]
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

    // --- �� �ʱ�ȭ ���� ���� ---
    [Header("�� �ʱ�ȭ ����")]
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
        // --- �̵� ���� ---
        if (Time.time >= nextMoveTime)
        {
            SetNewTargetPosition();
            nextMoveTime = Time.time + moveInterval;
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // --- 6�� ����ü ȸ�� ���� ---
        if (isHexFormedAndOrbiting && hexProjectiles.Count > 0)
        {
            currentHexOrbitAngle += hexOrbitSpeed * Time.deltaTime;
            for (int i = 0; i < hexProjectiles.Count; i++)
            {
                GameObject hexP = hexProjectiles[i];
                if (hexP != null && hexP.transform.parent == this.transform)
                {
                    float angle = (i * (360f / hexProjectiles.Count)) + currentHexOrbitAngle; // Dynamically adjust angle based on current hex projectiles count
                    Vector3 orbitOffset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0) * hexFormationRadius;
                    hexP.transform.localPosition = orbitOffset;
                }
            }
        }

        // --- ���� ���� ---
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
        Debug.Log("������ ���ο� ��ǥ ����: " + targetPosition);
    }

    void ChooseAndExecuteAttackPattern()
    {
        int attackChoice = (!isHexAttackOnCooldown && isHexFormedAndOrbiting && hexProjectiles.Count > 0) ? Random.Range(0, 4) : Random.Range(0, 3);

        switch (attackChoice)
        {
            case 0:
                StartCoroutine(AttackPattern1And2Hexagonal());
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

    // ����ü 1 & 2�� ���� �������� ������(�Ǵ� ������ n-����) ������� ������ �߻�
    IEnumerator AttackPattern1And2Hexagonal()
    {
        // ����ü 1 �߻�
        if (projectilePrefab1 != null)
        {
            for (int i = 0; i < numberOfProjectiles12; i++)
            {
                // �� ����ü�� ������ �յ��ϰ� �й� (��: 7���� 360/7 = �� 51.4�� ����)
                float angle = i * (360f / numberOfProjectiles12);
                // ������ �߾ӿ������� ������ �̿��� ����ü�� ����� �ʱ� ��ġ�� ���
                Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
                Vector3 spawnPos = transform.position + (Vector3)direction * hexSpreadRadius12; // ���� �ֺ��� ����

                GameObject p1 = Instantiate(projectilePrefab1, spawnPos, Quaternion.identity);
                Rigidbody2D rb1 = p1.GetComponent<Rigidbody2D>();
                if (rb1 != null) rb1.velocity = direction * projectileSpeed; // ���� �������� �߻�
            }
        }
        yield return new WaitForSeconds(1f); // ù ��° �߻� �� ��� �ð�

        // ����ü 2 �߻�
        if (projectilePrefab2 != null)
        {
            for (int i = 0; i < numberOfProjectiles12; i++)
            {
                float angle = i * (360f / numberOfProjectiles12);
                Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
                Vector3 spawnPos = transform.position + (Vector3)direction * hexSpreadRadius12;

                GameObject p2 = Instantiate(projectilePrefab2, spawnPos, Quaternion.identity);
                Rigidbody2D rb2 = p2.GetComponent<Rigidbody2D>();
                if (rb2 != null) rb2.velocity = direction * projectileSpeed;
            }
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