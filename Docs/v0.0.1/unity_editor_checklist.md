# 0.1.0 Unity Editor 설정 가이드

## ✅ 전체 작업 체크리스트

- [ ] **Title 씬 생성** (Canvas, 버튼, TitleManager, CutsceneManager)
- [ ] **Game_Map_1 수정** (door_1 → Move 타입, frame에 FrameView 부착)
- [ ] **Game_Map_2 씬 생성** (player, door_2, DoorConfirmPopup)
- [ ] **Build Settings 업데이트** (Title → Game_Map_1 → Game_Map_2 → Ending)

---

## 1. Title 씬 생성

### 1-1. 씬 생성
- File → New Scene → Save As → `Assets/_Scenes/Title.unity`

### 1-2. Canvas_Title 생성
- Hierarchy 우클릭 → UI → Canvas → 이름: `Canvas_Title`
- 하위 생성:

| 요소 | 타입 | 설정 |
|------|------|------|
| Background | UI → Image | 전체화면, 배경 이미지 할당 |
| Text_Title | UI → Text-TMP | "Luna's Room", 가운데 상단 |
| Button_Start | UI → Button-TMP | "Start", 가운데 |
| Button_Quit | UI → Button-TMP | "Quit", Start 아래 |

### 1-3. 커트씬 Canvas (오프닝용)
- Hierarchy 우클릭 → UI → Canvas → 이름: `Canvas_Cutscene`
- Canvas 내 UI → Image 생성 → 이름: `CutsceneImage` (전체화면)
- `Canvas_Cutscene` 초기 **비활성화**

### 1-4. TitleManager 오브젝트
- Create Empty → 이름: `TitleManager`
- Add Component → `TitleManager` 스크립트
- `Cutscene Manager` 필드: 아래에서 만들 오브젝트 연결

### 1-5. CutsceneManager 오브젝트
- Create Empty → 이름: `CutsceneManager`
- Add Component → `CutsceneManager` 스크립트
- 설정:
  - **Cutscene Canvas**: `Canvas_Cutscene`
  - **Cutscene Image**: `CutsceneImage`
  - **Next Scene Name**: `Game_Map_1`
  - **Cutscene Duration**: 3 (초)
  - **Cutscene Animator**: 나중에 애니메이션 파일 준비 후 설정

### 1-6. 버튼 OnClick 연결
| 버튼 | OnClick | 함수 |
|------|---------|------|
| Button_Start | TitleManager | `OnStartClicked()` |
| Button_Quit | TitleManager | `OnQuitClicked()` |

---

## 2. Game_Map_1 수정

### 2-1. door_1 설정 변경
- door_1 선택 → Door 스크립트:
  - **Door Type**: `Move`
  - **Next Scene Name**: `Game_Map_2`
  - **Player Inventory**: player 연결
  - (confirmPopup은 비워둠 — Move 타입이므로 불필요)

### 2-2. frame에 FrameView 스크립트 부착
- `frame` 오브젝트 선택
- **Layer**: `Interactable`로 변경
- Add Component → **Box Collider 2D** → Is Trigger ✓
- Add Component → `FrameView` 스크립트

### 2-3. FrameViewPanel UI 생성
- Canvas_Game 우클릭 → UI → Panel → 이름: `FrameViewPanel`
- 하위 생성:
  - **Image_Hint**: UI → Image (힌트 이미지)
  - **Text_Hint**: UI → Text-TMP (힌트 설명 텍스트)
- `FrameViewPanel`에 Add Component → `FrameViewPanel` 스크립트
  - **Player Move**: player의 PlayerMove2D 연결
- `FrameViewPanel` 초기 **비활성화**
- frame의 `FrameView` 스크립트 → **View Panel**: `FrameViewPanel` 연결

---

## 3. Game_Map_2 씬 생성

### 3-1. 씬 생성
- File → New Scene → Save As → `Assets/_Scenes/Game_Map_2.unity`

### 3-2. 기본 오브젝트
- Main Camera (Orthographic)
- player 복사 (Game_Map_1의 player와 동일 구성)
  - Rigidbody2D, Capsule Collider 2D, Animator
  - PlayerMove2D, PlayerInventory, PlayerInteraction 스크립트
- map_2 (빈 오브젝트)
  - bg (배경 + Edge Collider 2D)
  - door_2 (Door 스크립트)

### 3-3. door_2 설정
- **Layer**: `Interactable`
- Add Component → Box Collider 2D → Is Trigger ✓
- Add Component → `Door` 스크립트
  - **Door Type**: `Ending`
  - **Correct Key**: `B`
  - **Player Inventory**: player 연결
  - **Confirm Popup**: `DoorConfirmPopup` (아래에서 생성)

### 3-4. Canvas_Game 생성
- UI → Canvas → 이름: `Canvas_Game`
- 하위 생성:
  - **Text_E** (TMP, 'E' 표시용)
  - **MessageText** (TMP, 메시지 표시용)
  - **DoorConfirmPopup** (Panel, 초기 비활성)
    - **Text_Message** (TMP, 확인 문구)
    - **Button_Yes** (Button-TMP, "Yes")
    - **Button_No** (Button-TMP, "No")

### 3-5. DoorConfirmPopup 설정
- `DoorConfirmPopup`에 Add Component → `DoorConfirmPopup` 스크립트
  - **Player Move**: player의 PlayerMove2D 연결
  - **Message Text**: `Text_Message` 연결
- 버튼 OnClick:
  - Button_Yes → DoorConfirmPopup → `OnYesClicked()`
  - Button_No → DoorConfirmPopup → `OnNoClicked()`

### 3-6. 싱글톤 UI 오브젝트
- Create Empty → `MessageUI` + MessageUI 스크립트 → Message Text 연결
- Create Empty → `InteractionPromptUI` + InteractionPromptUI 스크립트 → Text_E 연결

### 3-7. GameManager
- Game_Map_1에 이미 있고 DontDestroyOnLoad로 유지됨
- Game_Map_2에는 별도 배치 **불필요** (자동 유지)
- ⚠️ 단, Game_Map_2에서 직접 테스트 시작하려면 GameManager 임시 배치 필요

---

## 4. Build Settings

File → Build Settings:

| 순서 | 씬 |
|------|-----|
| 0 | Title |
| 1 | Game_Map_1 |
| 2 | Game_Map_2 |
| 3 | Ending |

---

## 🎮 전체 게임 흐름 테스트

```
Title (Start) → [오프닝] → Game_Map_1
  → frame에 E → 힌트 패널 (ESC 닫기)
  → drawer에 E → 열쇠 선택
  → door_1에 E → Game_Map_2로 이동 (열쇠 유지됨)
  → door_2에 E →
    ├ 열쇠 없음 → "Need a key"
    └ 열쇠 있음 → 확인 팝업
      ├ No → 돌아감
      ├ Yes + Key A → "The key doesn't fit."
      └ Yes + Key B → Ending 씬
```
