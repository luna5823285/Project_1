// ============================================================
// ItemPopup.cs
// 열쇠 획득 시 표시되는 아이템 팝업 UI
// 구성: 검은 배경(알파 200) + 열쇠 아이콘 + "Got a key." 텍스트
// ESC 또는 마우스 클릭으로 닫기
// 0.2.0: 신규 추가
// ============================================================

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class ItemPopup : MonoBehaviour
{
    // ============================================================
    // 싱글톤 인스턴스
    // ============================================================
    public static ItemPopup Instance { get; private set; }

    // ============================================================
    // Inspector 설정 변수
    // ============================================================

    // 플레이어 이동 스크립트 참조 (팝업 중 이동 비활성화)
    public PlayerMove2D playerMove;

    // 아이콘 이미지 (획득한 열쇠 스프라이트를 동적으로 설정)
    public Image keyIcon;

    // 열쇠별 스프라이트 배열 (Inspector에서 순서대로: A, B, C)
    public Sprite[] keySprites;

    // ============================================================
    // Unity 생명주기 - Awake
    // ============================================================
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ============================================================
    // 팝업 표시
    // PlayerInventory.AddKey()에서 호출
    // ============================================================
    public void Show(KeyType key)
    {
        // 열쇠 타입에 맞는 스프라이트 설정
        int index = (int)key;
        if (keySprites != null && index < keySprites.Length && keySprites[index] != null)
        {
            keyIcon.sprite = keySprites[index];
        }

        gameObject.SetActive(true);
        Debug.Log($"[ItemPopup] 팝업 표시: 열쇠 {key}");
    }

    // ============================================================
    // Unity 생명주기 - OnEnable
    // ============================================================
    private void OnEnable()
    {
        if (playerMove != null) playerMove.canMove = false;
        if (InteractionPromptUI.Instance != null) InteractionPromptUI.Instance.Hide();
    }

    // ============================================================
    // Unity 생명주기 - Update
    // ESC 또는 마우스 클릭으로 팝업 닫기
    // ============================================================
    private void Update()
    {
        bool escPressed = Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame;
        bool mouseClicked = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;

        if (escPressed || mouseClicked)
        {
            gameObject.SetActive(false);
        }
    }

    // ============================================================
    // Unity 생명주기 - OnDisable
    // ============================================================
    private void OnDisable()
    {
        if (playerMove != null) playerMove.canMove = true;
    }
}
