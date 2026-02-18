// ============================================================
// CutsceneManager.cs
// 커트씬(오프닝/엔딩) 재생을 관리하는 스크립트
// 전체화면 Image에 스프라이트 애니메이션을 재생하고 완료 후 씬 전환
// ============================================================

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    // ============================================================
    // Inspector 설정 변수
    // ============================================================
    
    // 커트씬을 표시할 전체화면 Canvas
    public GameObject cutsceneCanvas;
    
    // 커트씬 이미지 (스프라이트 애니메이션이 재생될 Image)
    public Image cutsceneImage;
    
    // 커트씬 애니메이터 (스프라이트 시퀀스 제어)
    public Animator cutsceneAnimator;
    
    // 재생 완료 후 이동할 씬 이름
    public string nextSceneName;
    
    // 커트씬 재생 시간 (초) - 애니메이션이 없을 때 대기 시간으로 사용
    public float cutsceneDuration = 3f;

    // ============================================================
    // 커트씬 재생 시작
    // TitleManager 등에서 호출
    // ============================================================
    public void PlayCutscene()
    {
        // 커트씬 Canvas 활성화
        if (cutsceneCanvas != null)
        {
            cutsceneCanvas.SetActive(true);
        }
        
        // 애니메이션 재생 후 씬 전환 코루틴 시작
        StartCoroutine(PlayAndTransition());
    }

    // ============================================================
    // 커트씬 재생 및 씬 전환 코루틴
    // ============================================================
    private IEnumerator PlayAndTransition()
    {
        Debug.Log($"[Cutscene] 커트씬 재생 시작 ({cutsceneDuration}초)");
        
        // 애니메이터가 있으면 재생 트리거
        if (cutsceneAnimator != null)
        {
            cutsceneAnimator.SetTrigger("Play");
        }
        
        // 지정된 시간만큼 대기
        yield return new WaitForSeconds(cutsceneDuration);
        
        Debug.Log($"[Cutscene] 커트씬 완료 → {nextSceneName} 씬으로 이동");
        
        // 다음 씬으로 전환
        SceneManager.LoadScene(nextSceneName);
    }
}
