// ============================================================
// KeySelectPopup.cs
// 열쇠 선택 팝업 UI를 관리하는 스크립트
// 버튼 클릭 시 플레이어 인벤토리에 열쇠를 추가하고 팝업 닫기
// 팝업이 열려있는 동안 플레이어 이동 비활성화
// ESC 키로 팝업 닫기 가능
// ============================================================

using UnityEngine;
using UnityEngine.InputSystem;

public class KeySelectPopup : MonoBehaviour
{
    // ============================================================
    // Inspector 설정 변수
    // ============================================================
    
    // 플레이어의 인벤토리 참조 (열쇠를 추가하기 위해 필요)
    public PlayerInventory playerInventory;
    
    // 플레이어의 이동 스크립트 참조 (이동 제어용)
    public PlayerMove2D playerMove;

    // ============================================================
    // Unity 생명주기 - OnEnable
    // 팝업이 활성화될 때 호출됨
    // ============================================================
    private void OnEnable()
    {
        // 팝업이 열리면 플레이어 이동 비활성화
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
        // ESC 키를 누르면 팝업 닫기
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            gameObject.SetActive(false);
        }
    }

    // ============================================================
    // Unity 생명주기 - OnDisable
    // 팝업이 비활성화될 때 호출됨
    // ============================================================
    private void OnDisable()
    {
        // 팝업이 닫히면 플레이어 이동 활성화
        if (playerMove != null)
        {
            playerMove.canMove = true;
        }
    }

    // ============================================================
    // 열쇠 선택 메서드
    // Unity UI 버튼의 OnClick 이벤트에서 호출됨
    // ============================================================
    // keyIndex: 0 = KeyType.A, 1 = KeyType.B
    public void SelectKey(int keyIndex)
    {
        // 선택한 keyIndex를 KeyType enum으로 변환하여 인벤토리에 추가
        playerInventory.SetKey((KeyType)keyIndex);
        
        // 팝업 닫기 (OnDisable에서 자동으로 이동 활성화됨)
        gameObject.SetActive(false);
    }
}
