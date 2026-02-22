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
    // 자동 생성 - 어느 씬에서 시작해도 GameManager 존재 보장
    // ============================================================
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void AutoCreate()
    {
        if (Instance == null)
        {
            GameObject go = new GameObject("GameManager");
            go.AddComponent<GameManager>();
        }
    }

    // ============================================================
    // Unity 생명주기 - Awake
    // 싱글톤 초기화 및 DontDestroyOnLoad 설정
    // ============================================================
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("[GameManager] 싱글톤 초기화 완료");
        }
        else
        {
            Destroy(gameObject);
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
