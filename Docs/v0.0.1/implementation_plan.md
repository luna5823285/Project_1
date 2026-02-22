# 0.1.0 버전 구현 제안서

> **목표**: 타이틀 씬, 오프닝/엔딩 애니메이션, Game_Map_2, frame 상호작용, door 기능 확장

---

## 📋 변경 범위 요약

### 씬 구성 변경
| 순서 | 씬 이름 | 0.0.1 | 0.1.0 | 비고 |
|------|---------|-------|-------|------|
| 0 | Title | ❌ | ✅ | 신규 |
| 1 | Game_Map_1 | ✅ 시작씬 | ✅ | door_1 기능 변경 |
| 2 | Game_Map_2 | ❌ | ✅ | 신규 |
| 3 | Ending | ✅ | ✅ | 유지 |

### 스크립트 변경 분류
| 분류 | 파일 |
|------|------|
| **신규 생성** | TitleManager.cs, CutsceneManager.cs, FrameView.cs, DoorConfirmPopup.cs |
| **수정** | Door.cs, PlayerInteraction.cs, GameTypes.cs |
| **유지** | GameManager.cs, PlayerMove2D.cs, PlayerInventory.cs, Drawer.cs, KeySelectPopup.cs, MessageUI.cs, InteractionPromptUI.cs, EndingManager.cs |

---

## 1. 오프닝/엔딩 애니메이션 관리 방식 제안

### 🏆 권장: 전용 커트씬 씬 사용 (씬 기반 관리)

#### 구조 제안
```
씬 흐름:
Title → [Opening 커트씬] → Game_Map_1 → Game_Map_2 → [Ending 커트씬] → Ending
```

#### 방법 A: 별도 씬으로 분리 ❌ (비추천)
- Opening, Ending 각각 별도 씬 생성
- **단점**: 씬이 너무 많아짐 (6개), 관리 복잡

#### 방법 B: 기존 씬 내 오버레이 Canvas ✅ (추천)
- Title 씬과 Ending 씬에 **전체화면 Canvas + Image**를 배치
- Animator로 스프라이트 시퀀스 재생
- **하나의 CutsceneManager.cs 스크립트**로 통합 관리

**추천 이유:**
1. **씬 수 최소화**: 추가 씬 없이 기존 씬 활용 (총 4개 유지)
2. **재사용성**: CutsceneManager를 프리팹화하면 어디서든 사용 가능
3. **단순한 흐름**: Title → Game_Map_1 → Game_Map_2 → Ending (깔끔한 4개 씬)
4. **초보자 친화적**: 별도 씬 전환 없이 같은 씬에서 애니메이션 재생

#### 파일 구조
```
Assets/
├── _Core/
│   └── CutsceneManager.cs          ← 커트씬 재생/관리 스크립트
├── Game/
│   ├── Animations/
│   │   ├── Opening/
│   │   │   ├── OpeningAnimator.controller
│   │   │   └── OpeningAnimation.anim
│   │   └── Ending/
│   │       ├── EndingAnimator.controller
│   │       └── EndingAnimation.anim
│   └── Sprites/
│       ├── Opening/                 ← 오프닝 스프라이트 시퀀스
│       └── Ending/                  ← 엔딩 스프라이트 시퀀스
```

#### CutsceneManager.cs 설계
```csharp
public class CutsceneManager : MonoBehaviour
{
    public Image cutsceneImage;       // 전체화면 Image
    public Animator animator;          // 스프라이트 애니메이션 제어
    public string nextSceneName;       // 재생 완료 후 이동할 씬
    public float autoPlayDuration;     // 자동 재생 시간

    public void PlayCutscene() { ... }
    private IEnumerator PlayAndTransition() { ... }
}
```

#### 동작 흐름
**오프닝:**
1. Title에서 Start 클릭
2. 전체화면 Canvas 활성화 + Opening 애니메이션 재생
3. 재생 완료 → Game_Map_1 씬으로 자동 전환

**엔딩:**
1. door_2에서 올바른 열쇠(B) 사용
2. Ending 씬 로드
3. 전체화면 엔딩 애니메이션 재생
4. 재생 완료 → 기존 엔딩 화면 표시 (Thank you for playing)

> [!NOTE]
> 스프라이트 애니메이션 파일은 사용자가 준비해야 합니다. 스크립트와 Animator 구조만 미리 구현합니다.

---

## 2. 타이틀 씬

### 구조
```
Title (씬)
├── Canvas_Title
│   ├── Background (전체화면 Image - 배경 이미지)
│   ├── Text_Title ("Luna's Room")
│   ├── Button_Start ("Start")
│   └── Button_Quit ("Quit")
├── Canvas_Cutscene (초기 비활성)
│   └── CutsceneImage (전체화면 Image + Animator)
├── EventSystem
└── TitleManager (빈 오브젝트)
```

### TitleManager.cs 설계
```csharp
public class TitleManager : MonoBehaviour
{
    public CutsceneManager cutsceneManager;

    public void OnStartClicked()
    {
        // 오프닝 커트씬 재생 → 완료 후 Game_Map_1으로 이동
        cutsceneManager.PlayCutscene();
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }
}
```

---

## 3. frame 오브젝트 상호작용

### 설계
- frame에 새 스크립트 `FrameView.cs` 부착
- 상호작용 시 패널 표시 (이미지 + 설명 텍스트)
- ESC로 닫기 가능

### FrameView.cs 설계
```csharp
public class FrameView : MonoBehaviour
{
    public GameObject viewPanel;       // 표시할 패널
    // 패널 내부에 Image, Text(TMP)가 이미 배치되어 있음

    public void Interact()
    {
        viewPanel.SetActive(true);
    }
}
```

### PlayerInteraction.cs 수정
- `TryInteract()`에 `FrameView` 처리 추가

```diff
 private void TryInteract(GameObject interactable)
 {
     Drawer drawer = interactable.GetComponent<Drawer>();
     if (drawer != null) { drawer.Interact(); return; }

     Door door = interactable.GetComponent<Door>();
     if (door != null) { door.Interact(); return; }

+    FrameView frame = interactable.GetComponent<FrameView>();
+    if (frame != null) { frame.Interact(); return; }
 }
```

### Unity 설정
- frame에 `FrameView.cs` 부착 + Interactable 레이어 + Box Collider 2D (Trigger)
- Canvas_Game에 FrameViewPanel 생성 (Image + Text)
- 패널도 `KeySelectPopup`처럼 `OnEnable`에서 이동 금지 / `OnDisable`에서 이동 허용 처리

---

## 4. door_1 기능 변경 (Ending → Move)

### 변경 사항
- door_1의 `doorType`을 **Move**로 변경
- `nextSceneName`을 **"Game_Map_2"**로 설정
- **코드 수정 불필요** — Door.cs의 Move 타입 로직이 이미 구현되어 있음

```csharp
// 기존 Door.cs 코드 (이미 작동함)
if (doorType == DoorType.Move)
{
    SceneManager.LoadScene(nextSceneName); // "Game_Map_2"
    return;
}
```

### Unity Inspector 변경만 필요
- door_1 선택 → Door Type: **Move** / Next Scene Name: **"Game_Map_2"**

---

## 5. Game_Map_2 씬 + door_2

### 씬 구조
```
Game_Map_2 (씬)
├── Main Camera
├── player (프리팹 또는 복제)
├── map_2
│   ├── bg (Edge Collider 2D)
│   └── door_2 (Door Script + Interactable + Collider)
├── Canvas_Game
│   ├── Text_E
│   ├── MessageText
│   ├── KeySelectPopup (비활성)
│   └── DoorConfirmPopup (비활성) ← 신규
├── EventSystem
├── MessageUI
└── InteractionPromptUI
```

### door_2 상호작용 흐름

```
door_2에 E키 → 열쇠 확인 →
  ├── 열쇠 없음 → "Need a key" 메시지
  └── 열쇠 있음 → DoorConfirmPopup 표시
       ├── Yes 클릭 →
       │   ├── Key A → "The key doesn't fit." 메시지
       │   └── Key B → Ending 씬으로 이동
       └── No 클릭 → 패널 닫기 + 게임으로 복귀
```

### Door.cs 수정

```diff
 public class Door : MonoBehaviour
 {
     public DoorType doorType;
     public KeyType correctKey;
     public string nextSceneName;
     public PlayerInventory playerInventory;
+    public DoorConfirmPopup confirmPopup;  // Ending 타입 문의 확인 팝업

     public void Interact()
     {
         if (doorType == DoorType.Move)
         {
             SceneManager.LoadScene(nextSceneName);
             return;
         }

         // Ending 타입: 열쇠 확인
         if (!playerInventory.hasKey)
         {
             MessageUI.Instance?.ShowMessage("Need a key");
             return;
         }

-        if (playerInventory.currentKey == correctKey)
-        {
-            SceneManager.LoadScene("Ending");
-        }
-        else
-        {
-            MessageUI.Instance?.ShowMessage("Wrong key");
-        }
+        // 열쇠를 가지고 있으면 확인 팝업 표시
+        confirmPopup.Show(playerInventory.currentKey, correctKey);
     }
 }
```

### DoorConfirmPopup.cs (신규)
```csharp
public class DoorConfirmPopup : MonoBehaviour
{
    public PlayerMove2D playerMove;
    public TextMeshProUGUI messageText;

    private KeyType playerKey;
    private KeyType correctKey;

    public void Show(KeyType playerKey, KeyType correctKey)
    {
        this.playerKey = playerKey;
        this.correctKey = correctKey;
        messageText.text = "Would you like to open the door with the key?";
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        if (playerMove != null) playerMove.canMove = false;
        InteractionPromptUI.Instance?.Hide();
    }

    private void OnDisable()
    {
        if (playerMove != null) playerMove.canMove = true;
    }

    private void Update()
    {
        // ESC로 닫기
        if (Keyboard.current?.escapeKey.wasPressedThisFrame == true)
            gameObject.SetActive(false);
    }

    public void OnYesClicked()
    {
        if (playerKey == correctKey)
        {
            SceneManager.LoadScene("Ending");
        }
        else
        {
            MessageUI.Instance?.ShowMessage("The key doesn't fit.");
            gameObject.SetActive(false);
        }
    }

    public void OnNoClicked()
    {
        gameObject.SetActive(false);
    }
}
```

---

## 6. 영문 문구 검토

| 원문 | 문법 | 판정 |
|------|------|------|
| "Would you like to open the door with the key?" | ✅ 자연스러움 | 그대로 사용 |
| "The key doesn't fit." | ✅ 자연스러움 | 그대로 사용 |

> 두 문장 모두 문법적으로 정확하고 자연스럽습니다.

---

## 7. 작업 순서 제안

### Phase 1: 코드 작업
1. `CutsceneManager.cs` 생성
2. `TitleManager.cs` 생성
3. `FrameView.cs` 생성
4. `DoorConfirmPopup.cs` 생성
5. `Door.cs` 수정 (Ending 타입에 confirmPopup 연동)
6. `PlayerInteraction.cs` 수정 (FrameView 추가)
7. `GameTypes.cs` 주석 업데이트

### Phase 2: Unity 설정
1. Title 씬 생성 (Canvas, 버튼, TitleManager)
2. Game_Map_1 수정 (door_1 → Move 타입, frame에 FrameView 부착)
3. Game_Map_2 씬 생성 (player, door_2, Canvas_Game, DoorConfirmPopup)
4. Build Settings 업데이트 (Title, Game_Map_1, Game_Map_2, Ending)

### Phase 3: 검증
1. 전체 흐름 테스트
2. 빌드 테스트

---

## 8. 주의 사항

> [!IMPORTANT]
> **Game_Map_2의 player 오브젝트**: Game_Map_1에서 Game_Map_2로 이동할 때 player의 인벤토리(열쇠) 상태가 유지되어야 합니다. 두 가지 방법이 있습니다:
> 1. **DontDestroyOnLoad 사용** (권장): player를 씬 전환 시 파괴하지 않고 유지
> 2. **GameManager에 데이터 저장**: 씬 전환 전 열쇠 정보를 GameManager(싱글톤)에 저장 후 복원

> [!WARNING]
> **커트씬 스프라이트**: 스프라이트 파일은 사용자가 직접 준비해야 합니다. 코드에서는 Animator와 관리 구조만 구현합니다.

> [!NOTE]
> **FrameViewPanel**: Drawer의 KeySelectPopup과 유사한 UI 구조이므로, 동일한 패턴(OnEnable/OnDisable에서 이동 제어)을 따릅니다.
