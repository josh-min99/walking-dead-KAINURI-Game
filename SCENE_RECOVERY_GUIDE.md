# Unity Scene 복구 가이드 - SampleScene

## 1. Scene 기본 구조

### Scene 계층 구조
```
SampleScene
├── Main Camera
├── NetworkManager (비어있는 GameObject)
│   ├── Server (Script)
│   └── Client (Script)
├── Board (비어있는 GameObject)
│   └── board (Script)
├── GameUI (비어있는 GameObject)
│   └── GameUI (Script)
├── Canvas (UI Canvas)
│   ├── StartUI
│   ├── MainUI
│   ├── MarketUI
│   ├── ZMarketUI
│   ├── WaitingUI
│   ├── DisplayUI
│   ├── ConfirmUI
│   ├── ItemPageUI
│   ├── TeamInfoUI
│   ├── ZTeamInfoUI
│   ├── OppoInfoUI
│   ├── ZOppoInfoUI
│   ├── WaitBattleUI
│   ├── ArmyWinPage
│   ├── ZWinPage
│   ├── FinalWin
│   ├── FinalLose
│   └── NotionPage
└── MiniGame (미니게임 오브젝트)
```

---

## 2. Main Camera 설정

### 컴포넌트
- Transform
  - Position: (0, 0, -10)
  - Rotation: (0, 0, 0)
- Camera
  - Projection: Orthographic
  - Size: 적절한 값 설정 (게임 화면 크기에 따라)
  - Clipping Planes: Near: 0.3, Far: 1000
- AspectRatioFitter (스크립트)
- FixedAspect (스크립트)

---

## 3. NetworkManager 설정

### GameObject 생성
- 이름: NetworkManager
- Transform: Position (0, 0, 0)

### 컴포넌트 추가
1. **Server** 스크립트 추가
2. **Client** 스크립트 추가

---

## 4. Board 설정

### GameObject 생성
- 이름: Board
- Transform: Position (0, 0, 0)
- **중요**: 시작 시 비활성화 상태 (Inactive)

### board.cs 스크립트 필드 연결

#### [Header("Art stuff")]
- **Normal Color**: R:1, G:1, B:1, A:0.001
- **Hover Color**: Yellow
- **Active Color**: Green
- **Tile Sprite**: 타일용 스프라이트 (직접 생성 필요)

#### [Header("Prefabs & Materials")]
- **prefabs** (배열 크기: 10):
  - [0]: `Assets/Prefabs/1-신병_0.prefab`
  - [1]: `Assets/Prefabs/2-보병_0.prefab`
  - [2]: `Assets/Prefabs/3-특전사_0.prefab`
  - [3]: `Assets/Prefabs/4-사이보그_0.prefab`
  - [4]: `Assets/Prefabs/5-슈퍼솔저_0.prefab`
  - [5]: `Assets/Prefabs/1-아기좀비_0.prefab`
  - [6]: `Assets/Prefabs/2-좀비_0.prefab`
  - [7]: `Assets/Prefabs/3-근육좀비_0.prefab`
  - [8]: `Assets/Prefabs/4-특수좀비_0.prefab`
  - [9]: `Assets/Prefabs/5-좀비킹_0.prefab`

#### [Header("UI")]
- **timeText**: Canvas > MainUI 하위의 시간 표시 텍스트 (TMP_Text)
- **goldText**: Canvas > MainUI 하위의 골드 표시 텍스트 (TMP_Text)
- **moveText**: Canvas > MainUI 하위의 이동 횟수 표시 텍스트 (TMP_Text)
- **marketTrans**: Canvas > MarketUI의 RectTransform
- **teamInfoTrans**: Canvas > TeamInfoUI의 RectTransform
- **ItemPageTrans**: Canvas > ItemPageUI의 RectTransform
- **oppoInfoPage**: Canvas > OppoInfoUI의 RectTransform
- **mainUI**: Canvas > MainUI GameObject
- **waitBattleUI**: Canvas > WaitBattleUI GameObject
- **marketUI**: Canvas > MarketUI GameObject
- **gameUI**: GameUI GameObject의 GameUI 컴포넌트
- **display**: Canvas > DisplayUI GameObject
- **textcreater**: TextUICreater 스크립트가 붙은 GameObject 컴포넌트
- **armyWinPage**: Canvas > ArmyWinPage GameObject
- **ZWinPage**: Canvas > ZWinPage GameObject
- **finalWin**: Canvas > FinalWin GameObject
- **finalLose**: Canvas > FinalLose GameObject
- **notionPage**: Canvas > NotionPage GameObject

#### 아이템 프리팹 연결
- **armorPrefab**: `Assets/ArtWork/item/방탄복.prefab`
- **reversePrefab**: `Assets/ArtWork/item/인과역전.prefab`
- **aidKitPrefab**: `Assets/ArtWork/item/구급상자.prefab`
- **vaccinePrefab**: `Assets/ArtWork/item/백신.prefab`
- **gasBombPrefab**: `Assets/ArtWork/item/가스폭탄.prefab`
- **raiboPrefab**: `Assets/ArtWork/item/라이보.prefab`
- **minePrefab**: `Assets/ArtWork/item/지뢰.prefab`
- **gpsPrefab**: `Assets/ArtWork/item/GPS추적기.prefab`
- **bombPrefab**: `Assets/ArtWork/item/메가폭탄.prefab`
- **bombLightPrefab**: 메가폭탄 범위 표시용 프리팹 (생성 필요)

#### [Header("ITEM")] - 아이템 카운트 UI 텍스트
각 아이템마다 군인용/좀비용 2개씩 필요:
- **mineText**: 군인 지뢰 개수 (TMP_Text)
- **ZmineText**: 좀비 지뢰 개수 (TMP_Text)
- **illusionText**: 군인 분신술 개수 (TMP_Text)
- **ZillusionText**: 좀비 분신술 개수 (TMP_Text)
- **gpsText**: 군인 GPS 개수 (TMP_Text)
- **ZgpsText**: 좀비 GPS 개수 (TMP_Text)
- **bigbombText**: 군인 메가폭탄 개수 (TMP_Text)
- **ZbigbombText**: 좀비 메가폭탄 개수 (TMP_Text)
- **changePlaceText**: 군인 말체인지 개수 (TMP_Text)
- **ZchangePlaceText**: 좀비 말체인지 개수 (TMP_Text)
- **reverseText**: 군인 인과역전 개수 (TMP_Text)
- **ZreverseText**: 좀비 인과역전 개수 (TMP_Text)
- **teleportText**: 군인 텔레포트 개수 (TMP_Text)
- **ZteleportText**: 좀비 텔레포트 개수 (TMP_Text)
- **armorText**: 군인 방탄복 개수 (TMP_Text)
- **droneText**: 군인 정찰드론 개수 (TMP_Text)
- **carText**: 군인 군용차 개수 (TMP_Text)
- **aidKitText**: 구급상자 개수 (TMP_Text)

#### 기타 필드
- **pcPrefab**: 상대편 말 표시용 프리팹 (생성 필요)
- **miniGame**: MiniGame GameObject
- **server**: NetworkManager > Server 컴포넌트
- **client**: NetworkManager > Client 컴포넌트

#### 초기값 설정 (Inspector에서 설정)
- **timeRemaining**: 10
- **movePerTurn**: 10
- **cameraMoveAmount**: 15
- **duration**: 1

---

## 5. GameUI 설정

### GameObject 생성
- 이름: GameUI
- Transform: Position (0, 0, 0)

### GameUI.cs 스크립트 필드 연결

- **server**: NetworkManager > Server 컴포넌트
- **client**: NetworkManager > Client 컴포넌트
- **addressInput**: Canvas > StartUI > IP 입력 필드 (TMP_InputField)
- **startUI**: Canvas > StartUI GameObject
- **mainUI**: Canvas > MainUI GameObject
- **marketUI**: Canvas > MarketUI GameObject
- **ZmarketUI**: Canvas > ZMarketUI GameObject
- **ZmarketItemUI3**: Canvas > ZMarketUI 하위 아이템 페이지
- **marketItemUI3**: Canvas > MarketUI 하위 아이템 페이지
- **marketItemUI**: Canvas > MarketUI 하위 아이템 페이지
- **ZmarketItemUI**: Canvas > ZMarketUI 하위 아이템 페이지
- **itemUI**: Canvas > ItemPageUI GameObject
- **ZitemUI**: Canvas > ZItemPageUI GameObject
- **teamInfoUI**: Canvas > TeamInfoUI GameObject
- **ZteamInfoUI**: Canvas > ZTeamInfoUI GameObject
- **waitingUI**: Canvas > WaitingUI GameObject
- **ZoppoInfo**: Canvas > ZOppoInfoUI GameObject
- **oppoInfo**: Canvas > OppoInfoUI GameObject
- **Board**: Board GameObject
- **board**: Board GameObject의 board 컴포넌트
- **displayUI**: Canvas > DisplayUI GameObject
- **confirmImg**: Canvas > ConfirmUI > 군인 확인 이미지
- **ZconfirmImg**: Canvas > ConfirmUI > 좀비 확인 이미지
- **confirmUI**: Canvas > ConfirmUI GameObject
- **oppo**: OppoInfoUI의 OppoInfo 컴포넌트
- **Zoppo**: ZOppoInfoUI의 OppoInfo 컴포넌트

---

## 6. Canvas UI 구조

### Canvas 설정
- **Render Mode**: Screen Space - Overlay
- **Canvas Scaler**:
  - UI Scale Mode: Scale With Screen Size
  - Reference Resolution: 적절한 해상도 설정 (예: 1920x1080)
  - Screen Match Mode: Match Width Or Height
- **Graphic Raycaster**: 추가

### 주요 UI 페이지 구성

#### StartUI (시작 화면)
- 배경 이미지
- Host 버튼
- Connect 버튼
- IP 주소 입력 필드 (TMP_InputField)
- 게임 제목 등

#### WaitingUI (대기 화면)
- "상대 접속 대기 중..." 텍스트
- IP 주소 표시 영역

#### MainUI (메인 게임 UI)
- 시간 표시 (timeText)
- 골드 표시 (goldText)
- 이동 횟수 표시 (moveText)
- 아이템 버튼들
- 상점 버튼
- 아군 정보 버튼
- 적군 정보 버튼
- 턴 종료 버튼

#### MarketUI / ZMarketUI (상점 UI)
- 군인용 / 좀비용 각각 생성
- 유닛 구매 버튼
- 아이템 구매 버튼
- 레벨업 버튼
- 뒤로가기 버튼
- 배경 이미지: `Assets/ArtWork/screen/` 폴더의 이미지 사용

#### TeamInfoUI / ZTeamInfoUI (아군 정보 UI)
- 아군 말들의 정보 표시
- TeamInfo 스크립트 연결

#### OppoInfoUI / ZOppoInfoUI (적군 정보 UI)
- 적군 말들의 정보 표시
- OppoInfo 스크립트 연결

#### ItemPageUI (아이템 페이지)
- 보유 아이템 목록
- 아이템 사용 버튼

#### ConfirmUI (구매 확인 UI)
- 구매 확인 이미지 (군인용/좀비용)
- 확인/취소 버튼

#### DisplayUI (게임 진행 표시 UI)
- 현재 라운드 표시
- 게임 상태 표시

#### WaitBattleUI (전투 대기 UI)
- 전투 대기 중 표시

#### 승리/패배 화면
- **ArmyWinPage**: 군인 승리 화면
- **ZWinPage**: 좀비 승리 화면
- **FinalWin**: 최종 승리 화면 (`Assets/ArtWork/screen/승리.png`)
- **FinalLose**: 최종 패배 화면 (`Assets/ArtWork/screen/패배.png`)

#### NotionPage (알림창)
- 알림 메시지 표시
- 배경 이미지: `Assets/ArtWork/screen/알림창.png`

---

## 7. MiniGame 설정

### GameObject 생성
- 이름: MiniGame
- 시작 시 비활성화 상태

### MiniGame.cs 스크립트 연결
- Win 버튼
- Lose 버튼
- 미니게임 UI 요소들

---

## 8. Layer 및 Tag 설정

### Layers (Edit > Project Settings > Tags and Layers)
- **unTile**: 타일이 아닌 오브젝트용 레이어

### Tags
- 필요한 태그들 추가 (코드 분석 필요)

---

## 9. Input System 설정

### InputManager 설정 (Edit > Project Settings > Input Manager)
- 키보드 입력 설정
- 마우스 입력 설정

### 주요 입력
- P 키: 일시정지
- 마우스 클릭: 말 이동 및 선택

---

## 10. 복구 순서 (권장)

1. **Canvas 및 기본 UI 구조 생성**
   - Canvas 생성 및 설정
   - 주요 UI 페이지 GameObject 생성

2. **NetworkManager 생성**
   - Server, Client 스크립트 추가

3. **Board 생성**
   - board 스크립트 추가

4. **GameUI 생성**
   - GameUI 스크립트 추가

5. **UI 요소 상세 구성**
   - 각 페이지별 하위 요소 생성
   - 텍스트, 버튼, 이미지 추가

6. **스크립트 필드 연결**
   - board.cs의 모든 필드 연결
   - GameUI.cs의 모든 필드 연결

7. **Prefab 연결**
   - 유닛 프리팹 배열 연결
   - 아이템 프리팹 연결

8. **테스트**
   - Host/Connect 기능 테스트
   - UI 전환 테스트

---

## 11. 추가 생성 필요 항목

### 1. bombLightPrefab (메가폭탄 범위 표시)
- 반투명 원형 스프라이트
- 반경 3칸 범위 표시용

### 2. pcPrefab (상대편 말 표시)
- 상대편 말의 위치를 표시하는 프리팹
- 스프라이트: `Assets/ArtWork/piece/상대말.png`

### 3. TextUICreater 오브젝트
- TextUICreater 스크립트가 붙은 GameObject
- 텍스트 생성 기능 담당

### 4. eventNotion 리스트
- 이벤트 알림용 GameObject 리스트
- 건물 방문 알림 등에 사용

---

## 12. 주의사항

1. **Board GameObject는 시작 시 비활성화 상태여야 함**
   - 게임 시작 시 GameUI에서 활성화

2. **팀별 UI 분리**
   - 군인팀(Army): 일반 UI 사용
   - 좀비팀(Zombi): Z 접두사 UI 사용

3. **네트워크 동기화**
   - Server는 호스트(군인팀)에서만 동작
   - Client는 양쪽 모두 동작

4. **카메라 위치**
   - 군인팀: (0, 0, -10)
   - 좀비팀: (30, 0, -10)
   - GameUI에서 자동 조정

5. **타일 생성**
   - board.cs의 Start()에서 147개 타일 자동 생성
   - customPositions 리스트에 위치 정의됨

---

## 다음 단계

Scene 복구 후 다음 작업 필요:
1. Prefab 설정 복구 (유닛 프리팹들)
2. 아이템 프리팹 설정
3. UI 이미지 및 스프라이트 연결
4. 빌드 설정
5. 테스트 및 디버깅
