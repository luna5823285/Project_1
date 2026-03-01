// ============================================================
// PlayerInventory.cs
// 플레이어의 인벤토리를 관리하는 스크립트
// 0.1.0: 씬 전환 시 GameManager에서 열쇠 상태 복원
// 0.2.0: 열쇠 최대 3개 보유 가능한 다중 인벤토리로 전환
// ============================================================

using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // ============================================================
    // 열쇠 목록 (최대 3개)
    // ============================================================
    public List<KeyType> keys = new List<KeyType>();

    // ============================================================
    // Unity 생명주기 - Start
    // 씬 시작 시 GameManager에서 열쇠 상태 복원
    // ============================================================
    private void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadKeyState(this);
        }
    }

    // ============================================================
    // 열쇠 보유 여부 확인
    // ============================================================
    public bool HasKey(KeyType key)
    {
        return keys.Contains(key);
    }

    // ============================================================
    // 열쇠 하나라도 보유 중인지 확인
    // ============================================================
    public bool HasAnyKey()
    {
        return keys.Count > 0;
    }

    // ============================================================
    // 열쇠 획득
    // DrawerPopup(KeySelectPopup), FramePuzzlePanel 등에서 호출
    // ============================================================
    public void AddKey(KeyType key)
    {
        if (keys.Contains(key))
        {
            Debug.LogWarning($"[Inventory] 이미 보유 중인 열쇠: {key}");
            return;
        }

        keys.Add(key);

        // GameManager에 즉시 저장 (씬 전환 시 보존)
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SaveKeyState(this);
        }

        Debug.Log($"[Inventory] 열쇠 획득: {key} | 보유 목록: [{string.Join(", ", keys)}]");

        // ItemPopup 표시
        ItemPopup.Instance?.Show(key);
    }

    // ============================================================
    // 열쇠 제거 (사용 후 소모하는 경우를 위해 예비)
    // ============================================================
    public void RemoveKey(KeyType key)
    {
        if (keys.Remove(key))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SaveKeyState(this);
            }
            Debug.Log($"[Inventory] 열쇠 제거: {key}");
        }
    }
}
