// ============================================================
// EndingManager.cs
// 엔딩 씬을 관리하는 스크립트
// Ending_A 씬과 Ending_B 씬 양쪽에 동일하게 사용 가능
// 0.1.x: 아무 키/클릭 → 게임 종료
// 0.2.0: Ending_B 씬 지원 (동일 스크립트, 씬별 텍스트 오브젝트만 다름)
// ============================================================

using UnityEngine;
using UnityEngine.InputSystem;

public class EndingManager : MonoBehaviour
{
    // ============================================================
    // 씬 시작 직후 한 프레임 동안 입력을 무시하는 플래그
    // (씬 전환 직후 키 입력이 잔류하여 즉시 종료되는 현상 방지)
    // ============================================================
    private bool inputReady = false;

    // ============================================================
    // Unity 생명주기 - Start
    // ============================================================
    private void Start()
    {
        // 씬 시작 후 다음 프레임부터 입력 허용
        StartCoroutine(EnableInputNextFrame());
    }

    private System.Collections.IEnumerator EnableInputNextFrame()
    {
        yield return null;
        inputReady = true;
    }

    // ============================================================
    // Unity 생명주기 - Update
    // 아무 키 또는 마우스 클릭 시 게임 종료
    // ============================================================
    private void Update()
    {
        if (!inputReady) return;

        bool keyPressed = Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame;
        bool mousePressed = Mouse.current != null &&
            (Mouse.current.leftButton.wasPressedThisFrame ||
             Mouse.current.rightButton.wasPressedThisFrame ||
             Mouse.current.middleButton.wasPressedThisFrame);

        if (keyPressed || mousePressed)
        {
            QuitGame();
        }
    }

    // ============================================================
    // 게임 종료 메서드
    // ============================================================
    private void QuitGame()
    {
        Debug.Log("[Ending] 게임을 종료합니다");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
