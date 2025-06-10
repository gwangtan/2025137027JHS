using UnityEngine;

public class PopupDebug : MonoBehaviour
{
    void OnEnable()
    {
        // 오브젝트가 활성화될 때 콘솔에 로그를 출력합니다.
        Debug.Log($"{gameObject.name} 이(가) 활성화되었습니다!", this);
    }

    void OnDisable()
    {
        // 오브젝트가 비활성화될 때 콘솔에 로그를 출력합니다.
        Debug.Log($"{gameObject.name} 이(가) 비활성화되었습니다!", this);

        // 해당 오브젝트의 모든 컴포넌트를 가져옵니다.
        Component[] allComponents = GetComponents<Component>();

        foreach (Component component in allComponents)
        {
            // Transform 컴포넌트는 비활성화할 수 없으므로 건너뜁니다.
            // 또한, 이 스크립트 자신(PopupDebug)도 비활성화하면 현재 로직이 중단될 수 있으므로 건너뜁니다.
            if (component == null || component is Transform || component == this)
            {
                continue;
            }

            // 컴포넌트가 'enabled' 속성을 가지는 'Behaviour' 타입인지 확인합니다.
            // MonoBehaviour, Renderer, Collider 등 대부분의 활성화/비활성화 가능한 컴포넌트가 Behaviour를 상속합니다.
            Behaviour behaviourComponent = component as Behaviour;
            if (behaviourComponent != null)
            {
                // 해당 컴포넌트가 현재 활성화되어 있다면 비활성화합니다.
                if (behaviourComponent.enabled)
                {
                    behaviourComponent.enabled = false;
                    Debug.Log($"  - {component.GetType().Name} 컴포넌트 비활성화됨.");
                }
            }
            // Rigidbody와 같이 'enabled' 속성이 없는 일부 컴포넌트는 이 방식으로 직접 비활성화할 수 없습니다.
            // 그런 컴포넌트들은 isKinematic, useGravity 등 해당 컴포넌트의 특정 속성을 통해 제어해야 합니다.
        }
    }

    void OnDestroy()
    {
        // 오브젝트가 파괴될 때 콘솔에 로그를 출력합니다.
        Debug.Log($"{gameObject.name} 이(가) 파괴되었습니다!", this);
    }
}
