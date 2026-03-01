// ============================================================
// FramePuzzlePanel.cs
// 액자(frame) 상호작용 시 표시되는 퍼즐 패널
// 나사 3개 클릭 → 커버 제거 → 열쇠 C 획득
// 상태(나사 제거 여부, 열쇠 C 획득)는 GameManager에 영구 저장
// 0.2.0: 신규 추가
// ============================================================

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class FramePuzzlePanel : MonoBehaviour
{
    // ============================================================
    // Inspector 설정 변수
    // ============================================================

    // 플레이어 이동 스크립트 참조
    public PlayerMove2D playerMove;

    // 플레이어 인벤토리 참조 (열쇠 C 획득 처리용)
    public PlayerInventory playerInventory;

    // 나사 오브젝트 배열 (Inspector에서 순서대로 Nail_0, Nail_1, Nail_2 등록)
    // 각 오브젝트는 반드시 Animator 컴포넌트를 가지고 있어야 함
    public GameObject[] nails;

    // 나사가 모두 제거되면 비활성화될 커버 오브젝트
    public GameObject frameCover;

    // 커버 제거 후 클릭 가능한 열쇠 C 아이콘 오브젝트
    public GameObject keyCIcon;

    // nail_off 애니메이션 클립 길이 (초 단위, Inspector에서 조정 가능)
    public float nailAnimDuration = 0.8f;

    // ============================================================
    // 내부 변수
    // ============================================================

    // 나사 제거 여부 (로컬 복사본, GameManager와 동기화)
    private bool[] nailRemoved = new bool[3];

    // 열쇠 C 획득 여부
    private bool keyCObtained;

    // 애니메이션 재생 중 여부 (재생 중 입력 차단)
    private bool isAnimating = false;

    // ============================================================
    // Unity 생명주기 - OnEnable
    // 팝업이 열릴 때마다 GameManager에서 상태를 복원
    // ============================================================
    private void OnEnable()
    {
        if (playerMove != null) playerMove.canMove = false;
        if (InteractionPromptUI.Instance != null) InteractionPromptUI.Instance.Hide();

        isAnimating = false;

        // GameManager에서 퍼즐 상태 복원
        LoadState();

        // 저장된 상태에 맞게 오브젝트 활성화 상태 적용
        UpdateVisuals();
    }

    // ============================================================
    // GameManager에서 퍼즐 상태 불러오기
    // ============================================================
    private void LoadState()
    {
        if (GameManager.Instance != null)
        {
            System.Array.Copy(GameManager.Instance.savedNailRemoved, nailRemoved, 3);
            keyCObtained = GameManager.Instance.savedKeyCObtained;
        }
        else
        {
            // GameManager 없을 때 기본값(모두 false)
            nailRemoved = new bool[3] { false, false, false };
            keyCObtained = false;
        }
    }

    // ============================================================
    // GameManager에 퍼즐 상태 저장
    // ============================================================
    private void SaveState()
    {
        if (GameManager.Instance != null)
        {
            System.Array.Copy(nailRemoved, GameManager.Instance.savedNailRemoved, 3);
            GameManager.Instance.savedKeyCObtained = keyCObtained;
        }
    }

    // ============================================================
    // 저장된 상태에 따라 UI 오브젝트 활성화 적용
    // ============================================================
    private void UpdateVisuals()
    {
        bool allRemoved = nailRemoved[0] && nailRemoved[1] && nailRemoved[2];

        for (int i = 0; i < nails.Length; i++)
        {
            if (nails[i] != null)
            {
                // 제거된 나사는 비활성화
                nails[i].SetActive(!nailRemoved[i]);
            }
        }

        // 나사 전부 제거 시 커버 비활성화
        if (frameCover != null) frameCover.SetActive(!allRemoved);

        // 나사 전부 제거 + 열쇠 C 미획득 → 열쇠 C 아이콘 활성화
        if (keyCIcon != null) keyCIcon.SetActive(allRemoved && !keyCObtained);
    }

    // ============================================================
    // Unity 생명주기 - Update
    // 애니메이션 재생 중이 아닐 때만 ESC 허용
    // ============================================================
    private void Update()
    {
        if (isAnimating) return;

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
    // 나사 클릭 처리
    // 각 나사 오브젝트(버튼)의 OnClick에서 호출
    // nailIndex: 0, 1, 2
    // ============================================================
    public void OnNailClicked(int nailIndex)
    {
        // 애니메이션 재생 중이거나 이미 제거된 나사라면 무시
        if (isAnimating || nailRemoved[nailIndex]) return;

        StartCoroutine(RemoveNailCoroutine(nailIndex));
    }

    // ============================================================
    // 나사 제거 코루틴
    // nail_off 애니메이션 재생 → 나사 비활성화 → 상태 저장 → 비주얼 갱신
    // ============================================================
    private IEnumerator RemoveNailCoroutine(int nailIndex)
    {
        isAnimating = true;

        // nail_off 애니메이션 재생
        Animator anim = nails[nailIndex].GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("nail_off");
        }

        // 애니메이션 길이만큼 대기
        yield return new WaitForSeconds(nailAnimDuration);

        // 나사 상태 업데이트 및 저장
        nailRemoved[nailIndex] = true;
        SaveState();
        UpdateVisuals();

        isAnimating = false;
    }

    // ============================================================
    // 열쇠 C 클릭 처리
    // keyCIcon 오브젝트(버튼)의 OnClick에서 호출
    // ============================================================
    public void OnKeyCClicked()
    {
        if (isAnimating || keyCObtained) return;

        // 인벤토리에 열쇠 C 추가 (AddKey 내부에서 ItemPopup도 표시됨)
        if (playerInventory != null)
        {
            playerInventory.AddKey(KeyType.C);
        }

        // 퍼즐 완료 처리
        keyCObtained = true;
        SaveState();
        UpdateVisuals(); // keyCIcon 비활성화

        // ItemPopup이 꺼지면 keyCIcon이 이미 비활성화된 상태이므로
        // 팝업을 닫을 때 추가 처리 불필요

        // 팝업 닫기 (ItemPopup 표시 후 사용자가 직접 닫음)
        gameObject.SetActive(false);
    }
}
