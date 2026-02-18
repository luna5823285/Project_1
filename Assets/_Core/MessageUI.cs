// ============================================================
// MessageUI.cs
// 화면에 일시적으로 메시지를 표시하는 스크립트
// 싱글톤 패턴으로 어디서든 접근 가능
// ============================================================

using UnityEngine;
using TMPro;
using System.Collections;

public class MessageUI : MonoBehaviour
{
    // ============================================================
    // 싱글톤
    // ============================================================
    public static MessageUI Instance { get; private set; }

    // ============================================================
    // Inspector 설정 변수
    // ============================================================
    
    // 메시지를 표시할 TextMeshProUGUI
    public TextMeshProUGUI messageText;
    
    // 메시지 표시 시간 (초)
    public float displayDuration = 2f;

    // ============================================================
    // Unity 생명주기 - Awake
    // ============================================================
    private void Awake()
    {
        // 싱글톤 설정
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        // 초기에는 메시지 숨김
        if (messageText != null)
        {
            messageText.enabled = false;
        }
    }

    // ============================================================
    // 메시지 표시 메서드
    // ============================================================
    public void ShowMessage(string message)
    {
        // 이전 코루틴 중지
        StopAllCoroutines();
        
        // 메시지 표시 및 자동 숨김
        StartCoroutine(DisplayMessage(message));
    }

    // ============================================================
    // 메시지 표시 코루틴
    // ============================================================
    private IEnumerator DisplayMessage(string message)
    {
        // 메시지 설정 및 표시
        messageText.text = message;
        messageText.enabled = true;
        
        // 지정된 시간만큼 대기
        yield return new WaitForSeconds(displayDuration);
        
        // 메시지 숨김
        messageText.enabled = false;
    }
}
