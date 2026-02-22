// ============================================================
// MapExitTrigger.cs
// 맵 경계 트리거 스크립트
// 플레이어가 트리거에 접촉 시 지정된 씬으로 이동
// Game_Map_2 좌측에 배치 → Game_Map_1으로 복귀
// ============================================================

using UnityEngine;
using UnityEngine.SceneManagement;

public class MapExitTrigger : MonoBehaviour
{
    // ============================================================
    // Inspector 설정 변수
    // ============================================================
    
    // 이동할 씬 이름
    public string targetSceneName = "Game_Map_1";
    
    // 플레이어의 인벤토리 참조 (씬 전환 전 키 상태 저장용)
    public PlayerInventory playerInventory;

    // ============================================================
    // 트리거 접촉 감지
    // 플레이어가 트리거에 들어오면 씬 전환
    // ============================================================
    private void OnTriggerEnter2D(Collider2D other)
    {
        // "Player" 태그를 가진 오브젝트만 처리
        if (!other.CompareTag("Player")) return;

        Debug.Log($"[MapExitTrigger] {targetSceneName} 씬으로 이동");

        // 씬 전환 전 키 상태 저장
        if (GameManager.Instance != null && playerInventory != null)
        {
            GameManager.Instance.SaveKeyState(playerInventory);
        }

        SceneManager.LoadScene(targetSceneName);
    }

    // ============================================================
    // 디버깅용 Gizmo 표시
    // ============================================================
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
        
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        if (box != null)
        {
            Gizmos.DrawCube(transform.position + (Vector3)box.offset, box.size);
        }
    }
}
