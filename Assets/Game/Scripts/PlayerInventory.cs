// ============================================================
// PlayerInventory.cs
// 플레이어의 인벤토리를 관리하는 스크립트
// 열쇠 보유 여부와 현재 열쇠 타입을 저장
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
    // 열쇠 획득 메서드
    // KeySelectPopup에서 플레이어가 열쇠를 선택하면 호출됨
    // ============================================================
    public void SetKey(KeyType key)
    {
        hasKey = true;           // 열쇠 보유 상태로 변경
        currentKey = key;        // 선택한 열쇠 타입 저장
        
        Debug.Log($"[Inventory] 열쇠 획득: {key}");
        
        // 화면에 메시지 표시
        string keyName = key == KeyType.A ? "A" : "B";
        MessageUI.Instance?.ShowMessage($"Got Key {keyName}");
    }
}
