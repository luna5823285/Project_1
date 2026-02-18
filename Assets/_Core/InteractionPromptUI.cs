// ============================================================
// InteractionPromptUI.cs
// 화면에 'E' 표시를 관리하는 스크립트 (싱글톤)
// 상호작용 가능한 오브젝트 위에 'E'를 표시
// ============================================================

using UnityEngine;
using TMPro;

public class InteractionPromptUI : MonoBehaviour
{
    // ============================================================
    // 싱글톤
    // ============================================================
    public static InteractionPromptUI Instance { get; private set; }

    // ============================================================
    // Inspector 설정 변수
    // ============================================================
    
    // 'E' 표시 TextMeshProUGUI
    public TextMeshProUGUI promptText;
    
    // 카메라 참조
    public Camera mainCamera;
    
    // 오브젝트 위 오프셋 (Y축)
    public float yOffset = 1.5f;

    // ============================================================
    // 내부 변수
    // ============================================================
    
    // 현재 추적 중인 오브젝트
    private Transform targetObject;

    // ============================================================
    // Unity 생명주기 - Awake
    // ============================================================
    private void Awake()
    {
        // 싱글톤 설정
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        // 카메라 자동 찾기
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        
        // 초기에는 숨김
        Hide();
    }

    // ============================================================
    // Unity 생명주기 - LateUpdate
    // 오브젝트 위치 추적
    // ============================================================
    private void LateUpdate()
    {
        // 타겟이 있으면 화면 위치로 이동
        if (targetObject != null && promptText.enabled)
        {
            UpdatePosition();
        }
    }

    // ============================================================
    // 특정 오브젝트 위에 'E' 표시
    // ============================================================
    public void ShowAt(Transform target)
    {
        targetObject = target;
        promptText.enabled = true;
        UpdatePosition();
    }

    // ============================================================
    // 'E' 숨김
    // ============================================================
    public void Hide()
    {
        targetObject = null;
        promptText.enabled = false;
    }

    // ============================================================
    // 화면 위치 업데이트
    // ============================================================
    private void UpdatePosition()
    {
        if (targetObject == null || mainCamera == null) return;

        // 오브젝트의 월드 좌표 (위쪽 오프셋 추가)
        Vector3 worldPos = targetObject.position + Vector3.up * yOffset;
        
        // 월드 좌표를 스크린 좌표로 변환
        Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPos);
        
        // 화면 밖이면 숨김
        if (screenPos.z < 0)
        {
            promptText.enabled = false;
            return;
        }
        
        // 텍스트 위치 설정
        promptText.enabled = true;
        promptText.transform.position = screenPos;
    }
}
