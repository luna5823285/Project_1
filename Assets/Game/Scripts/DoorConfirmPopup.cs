// ============================================================
// DoorConfirmPopup.cs
// door_2 상호작용 시 열쇠 선택 팝업 관리
// 0.1.x: "Would you like to open the door?" + Yes/No
// 0.2.0: "Which key do you want to use?" + 보유 열쇠별 버튼 표시
//        열쇠 C → Ending_B, 열쇠 A/B → Ending_A 씬 분기
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

    // 플레이어 이동 스크립트 참조
    public PlayerMove2D playerMove;

    // 안내 문구 텍스트
    public TextMeshProUGUI messageText;

    // 열쇠별 버튼 오브젝트 (미리 배치, 인벤토리 상태에 따라 활성화)
    public GameObject keyAButton;  // 열쇠 A 버튼
    public GameObject keyBButton;  // 열쇠 B 버튼
    public GameObject keyCButton;  // 열쇠 C 버튼

    // ============================================================
    // 내부 변수
    // ============================================================

    private PlayerInventory playerInventory;

    // ============================================================
    // 팝업 표시 메서드
    // Door.cs에서 호출
    // ============================================================
    public void Show(PlayerInventory inventory)
    {
        playerInventory = inventory;
        messageText.text = "Which key do you want to use?";

        // 보유 중인 열쇠에 해당하는 버튼만 활성화
        if (keyAButton != null) keyAButton.SetActive(inventory.HasKey(KeyType.A));
        if (keyBButton != null) keyBButton.SetActive(inventory.HasKey(KeyType.B));
        if (keyCButton != null) keyCButton.SetActive(inventory.HasKey(KeyType.C));

        gameObject.SetActive(true);
    }

    // ============================================================
    // Unity 생명주기 - OnEnable
    // ============================================================
    private void OnEnable()
    {
        if (playerMove != null) playerMove.canMove = false;
        if (InteractionPromptUI.Instance != null) InteractionPromptUI.Instance.Hide();
    }

    // ============================================================
    // Unity 생명주기 - Update
    // ESC로 팝업 닫기
    // ============================================================
    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            gameObject.SetActive(false);
        }
    }

    // ============================================================
    // Unity 생명주기 - OnDisable
    // ============================================================
    private void OnDisable()
    {
        if (playerMove != null) playerMove.canMove = true;
    }

    // ============================================================
    // 열쇠 선택 메서드
    // 각 버튼의 OnClick에서 호출
    // keyIndex: 0 = A, 1 = B, 2 = C
    // ============================================================
    public void SelectKey(int keyIndex)
    {
        KeyType selectedKey = (KeyType)keyIndex;
        Debug.Log($"[DoorConfirmPopup] 선택한 열쇠: {selectedKey}");

        // 열쇠 C → Ending_B, 열쇠 A / B → Ending_A
        if (selectedKey == KeyType.C)
        {
            SceneManager.LoadScene("Ending_B");
        }
        else
        {
            SceneManager.LoadScene("Ending_A");
        }
    }

    // ============================================================
    // 취소 버튼 / ESC 역할 (팝업 닫기)
    // ============================================================
    public void OnCancelClicked()
    {
        gameObject.SetActive(false);
    }
}
