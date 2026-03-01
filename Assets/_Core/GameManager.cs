// ============================================================
// GameManager.cs
// 게임의 전반적인 흐름을 관리하는 싱글톤 매니저
// DontDestroyOnLoad로 씬 전환 시에도 유지
// 0.1.0: 씬 전환 시 열쇠 상태 유지 기능 추가
// 0.2.0: 다중 열쇠 목록, 퍼즐 나사 상태, 열쇠 C 획득 여부 저장 추가
// ============================================================

using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // ============================================================
    // 싱글톤 인스턴스
    // ============================================================
    public static GameManager Instance { get; private set; }

    // ============================================================
    // 씬 간 공유 데이터 — 열쇠 목록 (최대 3개)
    // ============================================================
    public List<KeyType> savedKeys = new List<KeyType>();

    // ============================================================
    // 씬 간 공유 데이터 — 액자 퍼즐 상태
    // savedNailRemoved[0~2]: 각 나사의 제거 여부
    // savedKeyCObtained: 열쇠 C 획득 완료 여부
    // ============================================================
    public bool[] savedNailRemoved = new bool[3] { false, false, false };
    public bool savedKeyCObtained = false;

    // ============================================================
    // 자동 생성 — 어느 씬에서 시작해도 GameManager 존재 보장
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
        savedKeys = new List<KeyType>(inventory.keys);
        Debug.Log($"[GameManager] 열쇠 상태 저장: [{string.Join(", ", savedKeys)}]");
    }

    // ============================================================
    // 열쇠 상태 복원 (씬 전환 후 호출)
    // ============================================================
    public void LoadKeyState(PlayerInventory inventory)
    {
        inventory.keys = new List<KeyType>(savedKeys);
        Debug.Log($"[GameManager] 열쇠 상태 복원: [{string.Join(", ", inventory.keys)}]");
    }

    // ============================================================
    // 퍼즐 상태 전체 초기화 (새 게임 시 호출)
    // ============================================================
    public void ResetAll()
    {
        savedKeys.Clear();
        savedNailRemoved = new bool[3] { false, false, false };
        savedKeyCObtained = false;
        Debug.Log("[GameManager] 전체 상태 초기화");
    }
}
