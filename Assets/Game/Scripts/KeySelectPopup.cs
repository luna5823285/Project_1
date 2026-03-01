// ============================================================
// KeySelectPopup.cs
// Drawer 상호작용 시 표시되는 서랍 내부 팝업
// 0.1.x: 버튼 텍스트 방식
// 0.2.0: 이미지 기반 리디자인 — 서랍 내부 일러스트 + 열쇠 이미지(버튼)
//        인벤토리 상태에 따라 열쇠 이미지 오브젝트 활성화/비활성화
// ============================================================

using UnityEngine;
using UnityEngine.InputSystem;

public class KeySelectPopup : MonoBehaviour
{
    // ============================================================
    // Inspector 설정 변수
    // ============================================================

    // 플레이어의 인벤토리 참조
    public PlayerInventory playerInventory;

    // 플레이어의 이동 스크립트 참조
    public PlayerMove2D playerMove;

    // 서랍 내부 열쇠 이미지 오브젝트
    // 미획득 상태: 활성화 / 획득 완료: 비활성화
    public GameObject keyAObject;  // 열쇠 A 이미지 (클릭 가능한 버튼)
    public GameObject keyBObject;  // 열쇠 B 이미지 (클릭 가능한 버튼)

    // ============================================================
    // Unity 생명주기 - OnEnable
    // ============================================================
    private void OnEnable()
    {
        // 이동 비활성화, 'E' 표시 숨김
        if (playerMove != null) playerMove.canMove = false;
        if (InteractionPromptUI.Instance != null) InteractionPromptUI.Instance.Hide();

        // 인벤토리 상태에 따라 열쇠 이미지 갱신
        RefreshKeyImages();
    }

    // ============================================================
    // 열쇠 이미지 활성화 상태 갱신
    // 이미 획득한 열쇠는 서랍에서 사라진 것으로 표현
    // ============================================================
    private void RefreshKeyImages()
    {
        if (playerInventory == null) return;

        // 이미 가지고 있는 열쇠는 이미지 비활성화
        if (keyAObject != null) keyAObject.SetActive(!playerInventory.HasKey(KeyType.A));
        if (keyBObject != null) keyBObject.SetActive(!playerInventory.HasKey(KeyType.B));
    }

    // ============================================================
    // Unity 생명주기 - Update
    // ESC로 팝업 닫기
    // ============================================================
    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
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

    // ============================================================
    // 열쇠 선택 메서드
    // 각 열쇠 이미지 오브젝트(버튼)의 OnClick에서 호출
    // keyIndex: 0 = KeyType.A, 1 = KeyType.B
    // ============================================================
    public void SelectKey(int keyIndex)
    {
        KeyType key = (KeyType)keyIndex;

        // 이미 보유 중인 열쇠는 무시
        if (playerInventory.HasKey(key))
        {
            Debug.Log($"[KeySelectPopup] 이미 보유한 열쇠: {key}");
            return;
        }

        playerInventory.AddKey(key);

        // 팝업 닫기 (ItemPopup이 표시되므로 서랍 팝업은 닫음)
        gameObject.SetActive(false);
    }
}
