# Ending 씬 구성 가이드

## 🎬 엔딩 씬 Unity Editor 설정

### 1단계: Ending 씬 열기
- Project 창에서 `Assets/_Scenes/Ending.unity` 더블클릭

---

### 2단계: Canvas 생성
- [ ] Hierarchy 우클릭 → **UI → Canvas**
- [ ] 이름: `Canvas_Ending`
- [ ] Render Mode: Screen Space - Overlay

---

### 3단계: 배경 Panel 생성
- [ ] `Canvas_Ending` 우클릭 → **UI → Panel**
- [ ] 이름: `Background`
- [ ] Inspector → Image 컴포넌트:
  - **Color**: 완전 검은색 (R:0, G:0, B:0, A:255) ← Alpha 255로 완전 불투명

---

### 4단계: "Thank you for playing" 텍스트 생성
- [ ] `Canvas_Ending` 우클릭 → **UI → Text - TextMeshPro**
- [ ] 이름: `Text_ThankYou`
- [ ] Inspector → TextMeshPro 설정:
  - **Text**: `Thank you for playing`
  - **Font Size**: 60
  - **Color**: 흰색 (R:255, G:255, B:255)
  - **Alignment**: 가운데 정렬 (수평/수직)
- [ ] Rect Transform:
  - **Pos Y**: 50 (중앙보다 약간 위)

---

### 5단계: "Press any key to quit" 텍스트 생성
- [ ] `Canvas_Ending` 우클릭 → **UI → Text - TextMeshPro**
- [ ] 이름: `Text_PressKey`
- [ ] Inspector → TextMeshPro 설정:
  - **Text**: `Press any key to quit the game`
  - **Font Size**: 30
  - **Color**: 흰색 (R:255, G:255, B:255)
  - **Alignment**: 가운데 정렬 (수평/수직)
- [ ] Rect Transform:
  - **Anchor Presets**: 하단 중앙 (Alt + Shift + 클릭)
  - **Pos Y**: 100 (하단에서 위로 100픽셀)

---

### 6단계: EndingManager 스크립트 추가
- [ ] Hierarchy에서 빈 GameObject 생성 (우클릭 → Create Empty)
- [ ] 이름: `EndingManager`
- [ ] Inspector → **Add Component** → `EndingManager` 스크립트

---

### 7단계: 씬 저장
- [ ] `Ctrl + S` 또는 File → Save

---

## ✅ 완료 확인

### 최종 하이어라키 구조:
```
Ending (씬)
├── Canvas_Ending
│   ├── Background (검은색 Panel)
│   ├── Text_ThankYou ("Thank you for playing")
│   └── Text_PressKey ("Press any key to quit")
├── EventSystem
└── EndingManager (빈 오브젝트 + EndingManager 스크립트)
```

### 테스트:
1. Ending 씬에서 Play 버튼 클릭
2. 검은 배경에 흰색 텍스트 2개 표시 확인
3. 아무 키나 누르면 플레이 모드 종료 확인

---

## 🎮 전체 게임 흐름 테스트

1. **Game_Map_1** 씬 열기
2. Play 버튼 클릭
3. 서랍에서 열쇠 B 선택 (팝업 열릴 때 이동 불가 확인)
4. 문에서 E 키 → Ending 씬 전환
5. 검은 화면 + 텍스트 확인
6. 아무 키나 누르면 종료
