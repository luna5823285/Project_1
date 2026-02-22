// ============================================================
// PlayerInventory.cs
// 플레이어의 인벤토리를 관리하는 스크립트
// 열쇠 보유 여부와 현재 열쇠 타입을 저장
// 0.1.0: 씬 전환 시 GameManager에서 열쇠 상태 복원
// ============================================================

using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // ============================================================
    // Inspector 설정 변수 (디버깅용, 게임 중 변경됨)
    // ============================================================
    
    // 열쇠 보유 여부 (true: 열쇠 있음, false: 열쇠 없음)
    public bool hasKey = false;
    
    // 현재 보유 중인 열쇠 타입 (A 또는 B)
    public KeyType currentKey;

    // ============================================================
    // Unity 생명주기 - Start
    // 씬 시작 시 GameManager에서 열쇠 상태 복원
    // ============================================================
    private void Start()
    {
        // GameManager에 저장된 열쇠 상태가 있으면 복원
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadKeyState(this);
        }
    }

    // ============================================================
    // 열쇠 획득 메서드
    // KeySelectPopup에서 플레이어가 열쇠를 선택하면 호출됨
    // ============================================================
    public void SetKey(KeyType key)
    {
        hasKey = true;
        currentKey = key;
        
        // 키 획득 즉시 GameManager에 저장 (씬 전환 시 door 연결 여부와 무관하게 보존)
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SaveKeyState(this);
        }
        
        Debug.Log($"[Inventory] 열쇠 획득: {key}");
        
        string keyName = key == KeyType.A ? "A" : "B";
        MessageUI.Instance?.ShowMessage($"Got Key {keyName}");
    }
}
