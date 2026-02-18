// ============================================================
// Door.cs
// 문 상호작용을 담당하는 스크립트
// Move 타입: 다음 씬으로 이동
// Ending 타입: 열쇠 확인 → 확인 팝업 → 엔딩 씬 이동
// ============================================================

using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    // ============================================================
    // Inspector 설정 변수
    // ============================================================
    
    // 문의 타입 (Move: 씬 이동, Ending: 엔딩 처리)
    public DoorType doorType;

    [Header("Ending Door Only")]
    // 엔딩 문을 열기 위한 올바른 열쇠 타입
    public KeyType correctKey;
    
    // 엔딩 문의 확인 팝업 (DoorConfirmPopup)
    public DoorConfirmPopup confirmPopup;

    [Header("Move Door Only")]
    // Move 타입 문일 때 이동할 씬 이름
    public string nextSceneName;

    // 플레이어의 인벤토리 참조 (열쇠 보유 여부 확인)
    public PlayerInventory playerInventory;

    // ============================================================
    // 상호작용 메서드
    // PlayerInteraction.cs에서 E키 입력 시 호출됨
    // ============================================================
    public void Interact()
    {
        // Move 타입 문: 열쇠 상태 저장 후 다음 씬으로 이동
        if (doorType == DoorType.Move)
        {
            Debug.Log($"[Door] {nextSceneName} 씬으로 이동");
            
            // 씬 전환 전 열쇠 상태 저장
            if (GameManager.Instance != null && playerInventory != null)
            {
                GameManager.Instance.SaveKeyState(playerInventory);
            }
            
            SceneManager.LoadScene(nextSceneName);
            return;
        }

        // Ending 타입 문: 열쇠 확인 후 확인 팝업 표시
        // 1. 열쇠를 보유하지 않은 경우
        if (!playerInventory.hasKey)
        {
            Debug.Log("[Door] Need a key");
            MessageUI.Instance?.ShowMessage("Need a key");
            return;
        }

        // 2. 열쇠를 가지고 있으면 확인 팝업 표시
        if (confirmPopup != null)
        {
            confirmPopup.Show(playerInventory.currentKey, correctKey);
        }
    }
}
