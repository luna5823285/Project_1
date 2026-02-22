# 버그 수정 계획서

## 버그 원인 분석 요약

---

## Bug 1 & 2: frame E키 미표시 + 상호작용 불가

### 원인
`PlayerInteraction.cs`는 `Physics2D.OverlapCircleAll()`로 주변 오브젝트를 감지하는데,  
**`interactableLayer`** 에 지정된 레이어만 탐지합니다.

`frame` 오브젝트의 **Layer가 `Interactable`로 설정되어 있지 않거나**,  
`PlayerInteraction`의 **Interactable Layer 슬롯이 비어 있으면** 감지 자체가 안 됩니다.

### 수정 방법
**코드 수정 없음 — Unity Inspector 설정만 변경**

1. `frame` 오브젝트 선택 → Inspector 상단 **Layer → `Interactable`** 로 변경
   - `Interactable` 레이어가 없으면 Add Layer로 새로 생성
2. `player` 또는 `player_1` 오브젝트의 `PlayerInteraction` 컴포넌트 →  
   **Interactable Layer** 슬롯에 `Interactable` 레이어 체크

> [!NOTE]
> `FrameView` 컴포넌트 처리는 `PlayerInteraction.cs`의 `TryInteract()`에 이미 구현되어 있음 (119번째 줄). Inspector 연결만 하면 됩니다.

---

## Bug 3: Key B 보유 시 Door_2 상호작용해도 "Need a key" 표시

### 원인
`Door.cs`의 `Interact()` 메서드 (57번째 줄):
```csharp
if (!playerInventory.hasKey)
```
`playerInventory`가 `null`이거나, **씬 전환 후 `GameManager`에서 키 상태가 복원되지 않아** `hasKey = false`인 상태로 남아있을 가능성이 높습니다.

> [!IMPORTANT]
> `door_2`의 Inspector에서 **Player Inventory** 슬롯이 연결되어 있는지 먼저 확인 필요.  
> 또한 `GameManager`의 `LoadKeyState`가 정상 동작하는지 확인 필요.

### 수정 방법
**Inspector 확인 우선, 코드 수정은 필요시**

1. `door_2` 오브젝트 선택 → `Door` 컴포넌트 → **Player Inventory** 슬롯에 `player`의 `PlayerInventory` 연결 확인
2. `door_2`의 **Door Type → `Ending`** 으로 설정되어 있는지 확인
3. `door_2`의 **Correct Key → `B`** 로 설정되어 있는지 확인
4. `door_2`의 **Confirm Popup** 슬롯에 `DoorConfirmPopup` 오브젝트 연결 확인

---

## Bug 4: Game_Map_2 → Game_Map_1 복귀 불가

### 원인
Map_2에 복귀 기능 자체가 없음. 새 스크립트 추가 필요.

### 수정 방법
**[NEW] `MapExitTrigger.cs` 스크립트 생성**

- 화면 가장 좌측에 Collider2D (Is Trigger = true) 오브젝트 배치
- 플레이어가 접촉 시(`OnTriggerEnter2D`) 지정된 씬으로 이동
- 씬 이동 전 `GameManager.SaveKeyState()` 호출하여 키 상태 유지

#### [NEW] `MapExitTrigger.cs`
```csharp
public class MapExitTrigger : MonoBehaviour
{
    public string targetSceneName;       // 이동할 씬 이름 ("Game_Map_1")
    public PlayerInventory playerInventory; // 키 상태 저장용
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance != null && playerInventory != null)
                GameManager.Instance.SaveKeyState(playerInventory);
            SceneManager.LoadScene(targetSceneName);
        }
    }
}
```

#### Unity 설정
- `Game_Map_2` 씬 하이어라키에 빈 GameObject → 이름: `LeftExitTrigger`
- 컴포넌트: `BoxCollider2D` (Is Trigger ✅) + `MapExitTrigger` 스크립트
- 위치: 화면 가장 좌측 끝 (X축 최솟값 근처)
- **Target Scene Name**: `"Game_Map_1"`
- **Player Inventory**: `player`의 `PlayerInventory` 연결

---

## 수정 파일 목록

| 작업 | 파일 | 비고 |
|------|------|------|
| Inspector 설정 | `frame` Layer → Interactable | 코드 X |
| Inspector 설정 | `PlayerInteraction` Interactable Layer 슬롯 | 코드 X |
| Inspector 확인 | `door_2` Door 컴포넌트 슬롯들 | 코드 X |
| [NEW] | `MapExitTrigger.cs` | 신규 스크립트 |
| Unity 설정 | `Game_Map_2`에 `LeftExitTrigger` 오브젝트 배치 | |

---

## 검증 계획

| 버그 | 검증 방법 |
|------|-----------|
| Bug 1&2 | `Game_Map_1` 플레이 → `frame` 근처 접근 시 `[E]` 표시 확인, E키 입력 시 `FrameViewPanel` 표시 확인 |
| Bug 3 | Key B 획득 후 `door_2` 접근 → E키 → `DoorConfirmPopup` 표시 확인 |
| Bug 4 | `Game_Map_2` 이동 후 화면 좌측 끝까지 이동 → `Game_Map_1`으로 복귀 확인, 키 상태 유지 확인 |
