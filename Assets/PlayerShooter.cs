using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    // �ν����Ϳ� �Ҵ��� ����ü ������ (�̸� ������ ����ü ������Ʈ)
    public GameObject projectilePrefab;
    // �߻� ���� (��)
    public float fireRate = 0.5f;
    // ���� �߻���� ���� �ð�
    private float nextFireTime;

    // ���ο� ���: �ٹ��� �߻� Ȱ��ȭ ����
    public bool isMultiDirectionalShootingActive = false;
    // �ٹ��� �߻� �� ����ü�� ������ ���� (��: 30��)
    public float projectileSpreadAngle = 30f;

    void Update()
    {
        // ���� ���� �ð��� ���� �߻� ���� �ð����� ũ�� ����ü �߻�
        if (Time.time >= nextFireTime)
        {
            // ���� �߻� �ð� ����
            nextFireTime = Time.time + fireRate;

            if (isMultiDirectionalShootingActive)
            {
                // �������� ����ü �߻�
                ShootSingleProjectile(Quaternion.identity);
                // ���� �������� ����ü �߻� (Z�� �������� projectileSpreadAngle��ŭ ȸ��)
                ShootSingleProjectile(Quaternion.Euler(0, 0, projectileSpreadAngle));
                // ������ �������� ����ü �߻� (Z�� �������� -projectileSpreadAngle��ŭ ȸ��)
                ShootSingleProjectile(Quaternion.Euler(0, 0, -projectileSpreadAngle));
                Debug.Log("PlayerShooter (Fire): �ٹ��� ����ü �߻�!");
            }
            else
            {
                // �⺻ ���� �߻�
                ShootSingleProjectile(Quaternion.identity);


            }
        }
    }

    // ����ü �ϳ��� Ư�� ȸ������ �߻��ϴ� ���� �޼���
    // rotationOffset�� �÷��̾��� ���� ȸ������ �߰������� ������ ȸ���Դϴ�.
    private void ShootSingleProjectile(Quaternion rotationOffset)
    {
        // �÷��̾��� ��ġ�� �÷��̾��� ���� ȸ���� rotationOffset�� ���� ȸ������ ����ü ����
        // Mover ��ũ��Ʈ�� transform.forward�� ����Ѵٸ� �� ȸ������ ���� ����ü ���� ������ �����˴ϴ�.
        GameObject newProjectileGO = Instantiate(projectilePrefab, transform.position, transform.rotation * rotationOffset);

        // ������ ����ü���� Mover ������Ʈ�� �����ɴϴ�.
        Mover newProjectileMover = newProjectileGO.GetComponent<Mover>();

        // Mover ������Ʈ�� �ִٸ� PlayerStatsManager���� ���� ������ ���� ������ �����մϴ�.
        if (newProjectileMover != null)
        {
            if (PlayerStatsManager.Instance != null)
            {
                newProjectileMover.damage = PlayerStatsManager.Instance.currentProjectileDamage;
            }
            else
            {
                Debug.LogWarning("PlayerShooter (ShootSingleProjectile): PlayerStatsManager �ν��Ͻ��� ã�� �� �����ϴ�! �⺻ ����ü �������� ����մϴ�.");
            }
        }
    }

    // �߻縦 ���ߴ� ���� �޼��� (�ܺο��� ȣ�� ����)
    public void StopShooting()
    {
        // �� ��ũ��Ʈ�� ��Ȱ��ȭ�Ͽ� �߻縦 ����ϴ�.
        enabled = false;
        Debug.Log("PlayerShooter (StopShooting): �߻� ������.");
    }

    // �߻縦 �ٽ� �����ϴ� ���� �޼��� (�ܺο��� ȣ�� ����)
    public void StartShooting()
    {
        enabled = true; // ��ũ��Ʈ Ȱ��ȭ
        nextFireTime = Time.time; // ��� �߻� �����ϵ��� �ʱ�ȭ
        Debug.Log("PlayerShooter (StartShooting): �߻� �簳��.");
    }

    // �ٹ��� �߻� ����� Ȱ��ȭ�ϴ� ���� �޼��� (BossHealth���� ȣ��)
    public void ActivateMultiDirectionalShooting()
    {
        isMultiDirectionalShootingActive = true;
        Debug.Log("PlayerShooter (ActivateMultiDirectionalShooting): �ٹ��� �߻� Ȱ��ȭ��.");
    }

    // �ٹ��� �߻� ����� ��Ȱ��ȭ�ϴ� ���� �޼��� (�ʿ�� ���)

}
