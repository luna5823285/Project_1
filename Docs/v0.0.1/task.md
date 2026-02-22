# 0.1.0 버전 구현 체크리스트

## Phase 1: 코드 작업

### 신규 스크립트 생성
- [x] `CutsceneManager.cs` - 커트씬 재생/전환 관리
- [x] `TitleManager.cs` - 타이틀 씬 UI 관리
- [x] `FrameView.cs` - frame 상호작용 (이미지+텍스트 패널)
- [x] `FrameViewPanel.cs` - 액자 패널 UI 관리 (ESC 닫기, 이동 제어)
- [x] `DoorConfirmPopup.cs` - door_2 확인 팝업 (Yes/No)

### 기존 스크립트 수정
- [x] `Door.cs` - Ending 타입에 confirmPopup 연동 + Move 전 열쇠 저장
- [x] `PlayerInteraction.cs` - FrameView 감지 추가
- [x] `GameManager.cs` - 열쇠 상태 Save/Load 기능 추가
- [x] `PlayerInventory.cs` - Start()에서 열쇠 상태 복원
- [x] `GameTypes.cs` - 주석 업데이트

## Phase 2: Unity 설정 (Editor)

### Title 씬 생성
- [ ] Title 씬 생성
- [ ] Canvas_Title + 배경, 제목, Start/Quit 버튼
- [ ] TitleManager 오브젝트 생성 및 연결
- [ ] CutsceneManager 오브젝트 생성 (오프닝용)

### Game_Map_1 수정
- [ ] door_1 → Move 타입, nextSceneName = "Game_Map_2"
- [ ] frame에 FrameView 스크립트 부착 + Interactable 레이어
- [ ] FrameViewPanel UI 생성 (Canvas_Game)

### Game_Map_2 씬 생성
- [ ] 씬 생성 + bg, door_2 배치
- [ ] Canvas_Game 복제 (Text_E, MessageText, DoorConfirmPopup)
- [ ] door_2에 Door 스크립트 설정 (Ending 타입)
- [ ] DoorConfirmPopup UI 생성

### Build Settings
- [ ] Title(0), Game_Map_1(1), Game_Map_2(2), Ending(3)

## Phase 3: 검증
- [ ] 전체 흐름 테스트
- [ ] 빌드 테스트
