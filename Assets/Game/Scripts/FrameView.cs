// ============================================================
// FrameView.cs
// 액자(frame) 상호작용을 담당하는 스크립트
// 0.1.x: 단순 이미지 패널 표시
// 0.2.0: FramePuzzlePanel(퍼즐)을 표시하도록 변경
// ============================================================

using UnityEngine;

public class FrameView : MonoBehaviour
{
    // ============================================================
    // Inspector 설정 변수
    // ============================================================

    // 액자 퍼즐 패널 (FramePuzzlePanel 컴포넌트가 있는 GameObject)
    public GameObject puzzlePanel;

    // ============================================================
    // 상호작용 메서드
    // PlayerInteraction.cs에서 E키 입력 시 호출됨
    // ============================================================
    public void Interact()
    {
        if (puzzlePanel != null)
        {
            puzzlePanel.SetActive(true);
        }
    }
}
