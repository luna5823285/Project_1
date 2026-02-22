# 'E' 표시 간소화 설정 가이드

## 🔧 Unity 설정 (간소화 버전)

### 1단계: Text_E 생성 (Canvas_Game에)

- [ ] `Canvas_Game` 우클릭 → **UI → Text - TextMeshPro**
- [ ] 이름: `Text_E`
- [ ] TextMeshPro 설정:
  - **Text**: `E`
  - **Font Size**: 50
  - **Color**: 노란색 (R:255, G:255, B:0) 또는 흰색
  - **Alignment**: 가운데 정렬
  - **Font Style**: Bold

---

### 2단계: InteractionPromptUI 오브젝트 생성

- [ ] Hierarchy에서 빈 GameObject 생성 (우클릭 → Create Empty)
- [ ] 이름: `InteractionPromptUI`
- [ ] Inspector → **Add Component** → `InteractionPromptUI` 스크립트
- [ ] InteractionPromptUI 스크립트 설정:
  - **Prompt Text**: `Text_E` 드래그 앤 드롭
  - **Main Camera**: `Main Camera` 드래그 앤 드롭 (또는 자동)
  - **Y Offset**: 1.5 (오브젝트 위쪽 거리)

---

## ✅ 최종 하이어라키 구조

```
Game_Map_1
├── Canvas_Game
│   ├── Text_E (새로 추가)
│   ├── MessageText
│   └── KeySelectPopup
├── InteractionPromptUI (빈 오브젝트, 새로 추가)
├── MessageUI
└── player
```

---

## 🎮 테스트

- [ ] drawer 근처로 이동 → drawer 위에 'E' 표시
- [ ] drawer에서 멀어짐 → 'E' 숨김
- [ ] door_1 근처로 이동 → door_1 위에 'E' 표시

**이제 월드 오브젝트마다 Canvas를 만들 필요 없이, 하나의 'E' 텍스트가 오브젝트 위치를 따라다닙니다!**
