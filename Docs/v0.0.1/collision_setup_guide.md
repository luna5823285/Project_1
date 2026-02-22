# 캐릭터 이동 범위 제한 문제 해결

## ✅ 현재 상태
- bg에 Edge Collider 2D 설정 완료
- Is Trigger 체크 해제됨 (올바름)

## 🔍 확인 사항

### 1단계: player 오브젝트에 Collider2D 확인

**중요**: player에 Collider가 없으면 물리적 충돌이 발생하지 않습니다!

1. Hierarchy에서 `player` 오브젝트 선택
2. Inspector에서 다음 컴포넌트 확인:
   - **Capsule Collider 2D** 또는 **Circle Collider 2D** 또는 **Box Collider 2D**
   
#### ❌ Collider가 없는 경우:
- Inspector → **Add Component** 클릭
- **Capsule Collider 2D** 선택 (캐릭터에 가장 적합)
- Size 조정: 캐릭터 스프라이트 크기에 맞게

---

### 2단계: Rigidbody2D 설정 확인

1. player의 **Rigidbody2D** 컴포넌트 확인
2. 다음 설정이 올바른지 확인:
   - **Body Type**: Dynamic (Kinematic이면 충돌 무시됨!)
   - **Simulated**: ✓ 체크됨
   - **Gravity Scale**: 0 (2D 횡스크롤이므로)
   - **Collision Detection**: Continuous (권장)
   - **Freeze Rotation Z**: ✓ 체크됨 (캐릭터 회전 방지)

#### ❌ Body Type이 Kinematic인 경우:
- **Dynamic**으로 변경

---

### 3단계: Layer 충돌 매트릭스 확인 (드물게 문제)

1. Unity 메뉴 → **Edit → Project Settings**
2. **Physics 2D** 선택
3. 하단의 **Layer Collision Matrix** 확인
4. player가 속한 레이어와 bg가 속한 레이어가 서로 충돌하도록 체크되어 있는지 확인

---

## 🎯 가장 흔한 원인

### 1. player에 Collider2D가 없음 ⭐⭐⭐
→ **Capsule Collider 2D** 추가

### 2. Rigidbody2D Body Type이 Kinematic ⭐⭐
→ **Dynamic**으로 변경

### 3. Collider Size가 너무 작음 ⭐
→ Collider 크기 조정

---

## 📋 최종 player 설정

player 오브젝트에 다음이 모두 있어야 함:
- ✅ **Rigidbody2D** (Body Type: Dynamic)
- ✅ **Capsule Collider 2D** (또는 Circle/Box)
- ✅ **PlayerMove2D** 스크립트
- ✅ **PlayerInventory** 스크립트
- ✅ **PlayerInteraction** 스크립트

설정 후 테스트해보세요!
