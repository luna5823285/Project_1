// ============================================================
// DoorConfirmPopup.cs
// door_2 상호작용 시 열쇠 사용 확인 팝업 관리
// "Would you like to open the door with the key?" + Yes/No 버튼
// ============================================================

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class DoorConfirmPopup : MonoBehaviour
{
    // ============================================================
    // Inspector 설정 변수
    // ============================================================
    
    // 플레이어의 이동 스크립트 참조 (이동 제어용)
    public PlayerMove2D playerMove;
    
    // 메시지 텍스트
    public TextMeshProUGUI messageText;

    // ============================================================
    // 내부 변수
    // ============================================================
    
    // 플레이어가 가진 열쇠 타입
    private KeyType playerKey;
    
    // 문을 열기 위한 올바른 열쇠 타입
    private KeyType correctKey;

    // ============================================================
    // 팝업 표시 메서드
    // Door.cs에서 호출
    // ============================================================
    public void Show(KeyType playerKey, KeyType correctKey)
    {
        this.playerKey = playerKey;
        this.correctKey = correctKey;
        messageText.text = "Would you like to open the door with the key?";
        gameObject.SetActive(true);
    }

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
    // Yes 버튼 클릭
    // Unity UI Button의 OnClick 이벤트에서 호출됨
    // ============================================================
    public void OnYesClicked()
    {
        if (playerKey == correctKey)
        {
            // 올바른 열쇠: 엔딩 씬으로 이동
            Debug.Log("[Door] 성공! 엔딩 씬으로 이동합니다");
            SceneManager.LoadScene("Ending");
        }
        else
        {
            // 잘못된 열쇠: 메시지 표시 후 팝업 닫기
            Debug.Log("[Door] 열쇠가 맞지 않습니다");
            MessageUI.Instance?.ShowMessage("The key doesn't fit.");
            gameObject.SetActive(false);
        }
    }

    // ============================================================
    // No 버튼 클릭
    // Unity UI Button의 OnClick 이벤트에서 호출됨
    // ============================================================
    public void OnNoClicked()
    {
        // 팝업 닫기 → 게임으로 복귀
        gameObject.SetActive(false);
    }
}
