// ============================================================
// TitleManager.cs
// 타이틀 씬을 관리하는 스크립트
// Start 버튼: 오프닝 커트씬 재생 후 게임 시작
// Quit 버튼: 게임 종료
// ============================================================

using UnityEngine;

public class TitleManager : MonoBehaviour
{
    // ============================================================
    // Inspector 설정 변수
    // ============================================================
    
    // 커트씬 매니저 참조 (오프닝 재생용)
    public CutsceneManager cutsceneManager;

    // ============================================================
    // Start 버튼 클릭 → 오프닝 재생 후 게임 시작
    // Unity UI Button의 OnClick 이벤트에서 호출됨
    // ============================================================
    public void OnStartClicked()
    {
        Debug.Log("[Title] Start 클릭 → 오프닝 재생");
        
        if (cutsceneManager != null)
        {
            cutsceneManager.PlayCutscene();
        }
    }

    // ============================================================
    // Quit 버튼 클릭 → 게임 종료
    // Unity UI Button의 OnClick 이벤트에서 호출됨
    // ============================================================
    public void OnQuitClicked()
    {
        Debug.Log("[Title] Quit 클릭 → 게임 종료");
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
