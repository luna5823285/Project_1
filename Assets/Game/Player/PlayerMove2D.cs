// ============================================================
// PlayerMove2D.cs
// 플레이어의 2D 횡방향 이동을 담당하는 스크립트
// A, D 키를 사용하여 좌우로 이동하고, 애니메이션과 스프라이트 방향 전환 처리
// ============================================================

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove2D : MonoBehaviour
{
    // ============================================================
    // Inspector 설정 변수
    // ============================================================
    
    // 플레이어 이동 속도 (기본값: 5)
    public float moveSpeed = 5f;

    // ============================================================
    // 내부 컴포넌트 참조
    // ============================================================
    
    private Rigidbody2D rb;         // 물리 이동을 위한 Rigidbody2D
    private Animator animator;       // 걷기 애니메이션 제어용 Animator

    // ============================================================
    // 입력 변수
    // ============================================================
    
    private float moveInput;         // 현재 프레임의 이동 입력 (-1, 0, 1)
    
    // 이동 가능 여부 (팝업이 열리면 false로 설정됨)
    public bool canMove = true;

    // ============================================================
    // Unity 생명주기 - Awake
    // 컴포넌트 참조 초기화
    // ============================================================
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // ============================================================
    // Unity 생명주기 - Update
    // 매 프레임마다 A, D 키 입력을 확인하고 애니메이션 및 방향 처리
    // ============================================================
    private void Update()
    {
        // 이동이 불가능한 상태면 입력을 받지 않음
        if (!canMove)
        {
            moveInput = 0f;
            animator.SetBool("isWalking", false);
            return;
        }

        // A, D 키 입력 감지 (-1: 왼쪽, 0: 정지, 1: 오른쪽)
        moveInput = 0f;

        if (Keyboard.current.aKey.isPressed)
            moveInput = -1f;  // A키: 왼쪽 이동
        else if (Keyboard.current.dKey.isPressed)
            moveInput = 1f;   // D키: 오른쪽 이동

        // 애니메이션 처리: 이동 중이면 isWalking을 true로 설정
        animator.SetBool("isWalking", moveInput != 0);

        // 캐릭터 방향 전환 (좌우 반전)
        if (moveInput != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Sign(moveInput) * Mathf.Abs(scale.x);  // moveInput의 부호에 따라 X축 방향 설정
            transform.localScale = scale;
        }
    }

    // ============================================================
    // Unity 생명주기 - FixedUpdate
    // 물리 연산을 사용하여 실제 이동 처리 (고정된 시간 간격으로 호출)
    // ============================================================
    private void FixedUpdate()
    {
        // Rigidbody2D의 linearVelocity를 사용하여 수평 이동
        // Y축 속도는 유지하여 중력 영향을 받도록 함
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }
}
