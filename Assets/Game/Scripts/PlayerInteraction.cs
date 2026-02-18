// ============================================================
// PlayerInteraction.cs
// 플레이어의 E키 상호작용 시스템을 담당하는 스크립트
// 주변의 상호작용 가능한 오브젝트를 감지하고 E키 입력 시 Interact() 호출
// 상호작용 가능한 오브젝트 근처에서 'E' 표시
// ============================================================

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    // ============================================================
    // Inspector 설정 변수
    // ============================================================
    
    // 상호작용 가능 범위 (플레이어 주변 반경)
    public float interactionRadius = 1.5f;
    
    // 상호작용 가능한 오브젝트의 레이어 (예: "Interactable" 레이어)
    public LayerMask interactableLayer;

    // ============================================================
    // 내부 변수
    // ============================================================
    
    // 현재 범위 내에 있는 상호작용 가능한 오브젝트
    private GameObject currentInteractable;

    // ============================================================
    // Unity 생명주기 - Update
    // 매 프레임마다 주변 상호작용 가능 오브젝트를 확인하고 E키 입력 처리
    // ============================================================
    private void Update()
    {
        // 1. 주변의 상호작용 가능한 오브젝트 감지
        DetectInteractable();

        // 2. E키 입력 확인 (Keyboard.current가 null인지 체크하여 안전하게 처리)
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            // 상호작용 가능한 오브젝트가 있으면 Interact() 호출
            if (currentInteractable != null)
            {
                TryInteract(currentInteractable);
            }
        }
    }

    // ============================================================
    // 상호작용 가능 오브젝트 감지
    // 플레이어 주변의 Collider2D를 검사하여 상호작용 가능 오브젝트 찾기
    // ============================================================
    private void DetectInteractable()
    {
        // OverlapCircle을 사용하여 플레이어 주변의 모든 Collider2D 검색
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactionRadius, interactableLayer);

        GameObject previousInteractable = currentInteractable;

        // 가장 가까운 오브젝트를 현재 상호작용 대상으로 설정
        if (hits.Length > 0)
        {
            currentInteractable = hits[0].gameObject;
        }
        else
        {
            currentInteractable = null;
        }

        // 상호작용 대상이 바뀌면 UI 업데이트
        if (previousInteractable != currentInteractable)
        {
            UpdateInteractionUI();
        }
    }

    // ============================================================
    // 상호작용 UI 업데이트
    // ============================================================
    private void UpdateInteractionUI()
    {
        if (InteractionPromptUI.Instance == null) return;

        // 현재 상호작용 가능한 오브젝트가 있으면 'E' 표시
        if (currentInteractable != null)
        {
            InteractionPromptUI.Instance.ShowAt(currentInteractable.transform);
        }
        else
        {
            InteractionPromptUI.Instance.Hide();
        }
    }

    // ============================================================
    // 상호작용 시도
    // 오브젝트의 Interact() 메서드를 호출 (Drawer, Door 등)
    // ============================================================
    private void TryInteract(GameObject interactable)
    {
        // Drawer 컴포넌트가 있는지 확인
        Drawer drawer = interactable.GetComponent<Drawer>();
        if (drawer != null)
        {
            drawer.Interact();
            return;
        }

        // Door 컴포넌트가 있는지 확인
        Door door = interactable.GetComponent<Door>();
        if (door != null)
        {
            door.Interact();
            return;
        }

        // FrameView 컴포넌트가 있는지 확인
        FrameView frame = interactable.GetComponent<FrameView>();
        if (frame != null)
        {
            frame.Interact();
            return;
        }

        // 그 외 상호작용 가능한 오브젝트 추가 가능
    }

    // ============================================================
    // 디버깅용 Gizmo 표시
    // Unity Editor에서 상호작용 범위를 시각적으로 확인
    // ============================================================
    private void OnDrawGizmosSelected()
    {
        // 노란색 원으로 상호작용 범위 표시
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
