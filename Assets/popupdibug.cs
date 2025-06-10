using UnityEngine;

public class PopupDebug : MonoBehaviour
{
    void OnEnable()
    {
        // ������Ʈ�� Ȱ��ȭ�� �� �ֿܼ� �α׸� ����մϴ�.
        Debug.Log($"{gameObject.name} ��(��) Ȱ��ȭ�Ǿ����ϴ�!", this);
    }

    void OnDisable()
    {
        // ������Ʈ�� ��Ȱ��ȭ�� �� �ֿܼ� �α׸� ����մϴ�.
        Debug.Log($"{gameObject.name} ��(��) ��Ȱ��ȭ�Ǿ����ϴ�!", this);

        // �ش� ������Ʈ�� ��� ������Ʈ�� �����ɴϴ�.
        Component[] allComponents = GetComponents<Component>();

        foreach (Component component in allComponents)
        {
            // Transform ������Ʈ�� ��Ȱ��ȭ�� �� �����Ƿ� �ǳʶݴϴ�.
            // ����, �� ��ũ��Ʈ �ڽ�(PopupDebug)�� ��Ȱ��ȭ�ϸ� ���� ������ �ߴܵ� �� �����Ƿ� �ǳʶݴϴ�.
            if (component == null || component is Transform || component == this)
            {
                continue;
            }

            // ������Ʈ�� 'enabled' �Ӽ��� ������ 'Behaviour' Ÿ������ Ȯ���մϴ�.
            // MonoBehaviour, Renderer, Collider �� ��κ��� Ȱ��ȭ/��Ȱ��ȭ ������ ������Ʈ�� Behaviour�� ����մϴ�.
            Behaviour behaviourComponent = component as Behaviour;
            if (behaviourComponent != null)
            {
                // �ش� ������Ʈ�� ���� Ȱ��ȭ�Ǿ� �ִٸ� ��Ȱ��ȭ�մϴ�.
                if (behaviourComponent.enabled)
                {
                    behaviourComponent.enabled = false;
                    Debug.Log($"  - {component.GetType().Name} ������Ʈ ��Ȱ��ȭ��.");
                }
            }
            // Rigidbody�� ���� 'enabled' �Ӽ��� ���� �Ϻ� ������Ʈ�� �� ������� ���� ��Ȱ��ȭ�� �� �����ϴ�.
            // �׷� ������Ʈ���� isKinematic, useGravity �� �ش� ������Ʈ�� Ư�� �Ӽ��� ���� �����ؾ� �մϴ�.
        }
    }

    void OnDestroy()
    {
        // ������Ʈ�� �ı��� �� �ֿܼ� �α׸� ����մϴ�.
        Debug.Log($"{gameObject.name} ��(��) �ı��Ǿ����ϴ�!", this);
    }
}
