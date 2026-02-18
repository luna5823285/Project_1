// ============================================================
// FrameViewPanel.cs
// 액자 상세보기 패널의 UI 관리 스크립트
// 패널 활성화 시 이동 금지, ESC로 닫기 가능
// KeySelectPopup과 동일한 패턴 사용
// ============================================================

using UnityEngine;
using UnityEngine.InputSystem;

public class FrameViewPanel : MonoBehaviour
{
    // ============================================================
    // Inspector 설정 변수
    // ============================================================
    
    // 플레이어의 이동 스크립트 참조 (이동 제어용)
    public PlayerMove2D playerMove;

    // ============================================================
    // Unity 생명주기 - OnEnable
    // 패널이 활성화될 때 호출됨
    // ============================================================
    private void OnEnable()
    {
        // 패널이 열리면 플레이어 이동 비활성화
        if (playerMove != null)
        {
            playerMove.canMove = false;
        }
        
        // 'E' 표시 숨김
        if (InteractionPromptUI.Instance != null)
        {
            InteractionPromptUI.Instance.Hide();
        }
    }

    // ============================================================
    // Unity 생명주기 - Update
    // ESC 키 입력 감지
    // ============================================================
    private void Update()
    {
        // ESC 키를 누르면 패널 닫기
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            gameObject.SetActive(false);
        }
    }

    // ============================================================
    // Unity 생명주기 - OnDisable
    // 패널이 비활성화될 때 호출됨
    // ============================================================
    private void OnDisable()
    {
        // 패널이 닫히면 플레이어 이동 활성화
        if (playerMove != null)
        {
            playerMove.canMove = true;
        }
    }
}
