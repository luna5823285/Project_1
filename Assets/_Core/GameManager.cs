// ============================================================
// GameManager.cs
// 게임의 전반적인 흐름을 관리하는 싱글톤 매니저
// DontDestroyOnLoad로 씬 전환 시에도 유지
// 0.1.0: 씬 전환 시 열쇠 상태 유지 기능 추가
// ============================================================

using UnityEngine;

public class GameManager : MonoBehaviour
{
    // ============================================================
    // 싱글톤 인스턴스
    // ============================================================
    public static GameManager Instance { get; private set; }

    // ============================================================
    // 씬 간 공유 데이터 (열쇠 상태)
    // ============================================================
    
    // 열쇠 보유 여부
    public bool savedHasKey = false;
    
    // 보유 중인 열쇠 타입
    public KeyType savedCurrentKey;

    // ============================================================
    // Unity 생명주기 - Awake
    // 싱글톤 초기화 및 DontDestroyOnLoad 설정
    // ============================================================
    private void Awake()
    {
        // 싱글톤 패턴: 이미 인스턴스가 존재하면 현재 오브젝트 파괴
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시에도 파괴되지 않음
        }
        else
        {
            Destroy(gameObject); // 중복 인스턴스 제거
        }
    }

    // ============================================================
    // 열쇠 상태 저장 (씬 전환 전 호출)
    // ============================================================
    public void SaveKeyState(PlayerInventory inventory)
    {
        savedHasKey = inventory.hasKey;
        savedCurrentKey = inventory.currentKey;
        Debug.Log($"[GameManager] 열쇠 상태 저장: hasKey={savedHasKey}, key={savedCurrentKey}");
    }

    // ============================================================
    // 열쇠 상태 복원 (씬 전환 후 호출)
    // ============================================================
    public void LoadKeyState(PlayerInventory inventory)
    {
        inventory.hasKey = savedHasKey;
        inventory.currentKey = savedCurrentKey;
        Debug.Log($"[GameManager] 열쇠 상태 복원: hasKey={savedHasKey}, key={savedCurrentKey}");
    }
}
