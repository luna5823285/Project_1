// ============================================================
// Door.cs
// 문 상호작용을 담당하는 스크립트
// E키를 눌러 상호작용하면 열쇠를 확인하고 엔딩 씬으로 이동
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
    // 엔딩 문을 열기 위한 올바른 열쇠 타입 (0.0.1에서는 B)
    public KeyType correctKey;

    [Header("Move Door Only")]
    // Move 타입 문일 때 이동할 씬 이름 (0.0.1에서는 미사용)
    public string nextSceneName;

    // 플레이어의 인벤토리 참조 (열쇠 보유 여부 확인)
    public PlayerInventory playerInventory;

    // ============================================================
    // 상호작용 메서드
    // PlayerInteraction.cs에서 E키 입력 시 호출됨
    // ============================================================
    public void Interact()
    {
        // Move 타입 문: 즉시 다음 씬으로 이동 (0.0.1에서는 사용 안 함)
        if (doorType == DoorType.Move)
        {
            SceneManager.LoadScene(nextSceneName);
            return;
        }

        // Ending 타입 문: 열쇠 확인 후 엔딩 씬으로 이동
        // 1. 열쇠를 보유하지 않은 경우
        if (!playerInventory.hasKey)
        {
            Debug.Log("[Door] Need a key");
            MessageUI.Instance?.ShowMessage("Need a key");
            return;
        }

        // 2. 올바른 열쇠를 가지고 있는 경우
        if (playerInventory.currentKey == correctKey)
        {
            Debug.Log("[Door] 성공! 엔딩 씬으로 이동합니다");
            SceneManager.LoadScene("Ending"); // 엔딩 씬 전환
        }
        // 3. 잘못된 열쇠를 가지고 있는 경우
        else
        {
            Debug.Log("[Door] Wrong key");
            MessageUI.Instance?.ShowMessage("Wrong key");
        }
    }
}
