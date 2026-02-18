// ============================================================
// EndingManager.cs
// 엔딩 씬을 관리하는 스크립트
// 아무 키나 누르면 게임을 종료함 (키보드 + 마우스 클릭 모두 포함)
// ============================================================

using UnityEngine;
using UnityEngine.InputSystem;

public class EndingManager : MonoBehaviour
{
    // ============================================================
    // Unity 생명주기 - Update
    // 매 프레임마다 아무 키 입력을 감지
    // ============================================================
    private void Update()
    {
        // 키보드의 아무 키나 눌렸는지 확인
        bool keyPressed = Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame;
        
        // 마우스 버튼이 눌렸는지 확인
        bool mousePressed = Mouse.current != null && 
            (Mouse.current.leftButton.wasPressedThisFrame || 
             Mouse.current.rightButton.wasPressedThisFrame ||
             Mouse.current.middleButton.wasPressedThisFrame);

        // 키보드 또는 마우스 입력이 있으면 게임 종료
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
        
        // Unity Editor에서는 플레이 모드 종료
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // 빌드된 게임에서는 애플리케이션 종료
        Application.Quit();
        #endif
    }
}
