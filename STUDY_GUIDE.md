# ItAssetPortal — 내 학습 노트

## 한 줄로 이 프로젝트

**ASP.NET 웹으로 만든 IT 자산 목록·등록 시스템**  
(ASP.NET Core 8 MVC + SQL Server Express)

---

> **나만 보는 문서.** 설계 보고서 아님.  
> VS에서 이 파일 열고 **미리 보기** 켜 두고, 막히면 **「따라가기」**만 찾으면 됨.

**자주 보는 곳**

| 하고 싶은 것 | 절 |
|--------------|-----|
| F5 / 목록 / 등록 버튼 눌렀을 때 | **[따라가기](#따라가기-내가-누르면-어디로-가나)** |
| SSMS에서 데이터 확인 | [SSMS에서 보기](#ssms에서-한번-확인) |
| WinForms랑 헷갈릴 때 | [20절](#20-winforms-vs-웹--질문하고-이해한-것) |
| 폴더·파일이 뭐 하는지 (참고) | [부록](#부록-참고-백과) |

---

## 따라가기 (내가 누르면 어디로 가나)

> 형식: **내 동작** → **브라우저 주소** → **C# 어디** → **DB** → **다시 어떤 화면**

---

### ① F5 (실행) — 처음 목록이 뜰 때

```
나: Visual Studio에서 F5
  → Program.cs 가 서버 켬 (app.Run)
  → 기본 주소가 Assets/Index 로 잡혀 있음 (Program.cs MapControllerRoute)

브라우저: GET /Assets/Index  (또는 /Assets)

  → Controllers/AssetsController.cs
       Index()  함수  (17~23줄)
         _db.Assets ... ToListAsync()   ← DB에서 읽기
         return View(assets);

  → Views/Assets/Index.cshtml  가 HTML 표로 그림
  → Views/_ViewStart.cshtml → _Layout.cshtml 이 감쌈 (메뉴 등)

DB: SELECT ... FROM Assets  (EF가 알아서 SQL 생성)
```

**이때 보는 파일 순서 (추천):**  
`Program.cs` → `AssetsController.cs` Index → `Index.cshtml`

---

### ② 「자산 등록」 버튼 — 등록 화면만 열 때

**화면:** 목록 (`Index.cshtml`) 오른쪽 파란 버튼 **자산 등록**

```
나: 「자산 등록」 클릭

  → Views/Assets/Index.cshtml  8줄
       <a asp-action="Create">자산 등록</a>
       (링크 URL이 /Assets/Create 로 만들어짐)

브라우저: GET /Assets/Create

  → AssetsController.cs
       Create()  함수  (26~28줄)   ← 이름 같지만 GET용 (인자 없음)
         return View();

  → Views/Assets/Create.cshtml  (빈 입력 폼)

DB: 아직 안 건드림
```

---

### ③ 「저장」 버튼 — 진짜 DB에 넣을 때

**화면:** 등록 폼 (`Create.cshtml`)

```
나: 이름 입력 후 「저장」 클릭

  → Views/Assets/Create.cshtml  8줄, 22줄
       <form asp-action="Create" method="post">
       <button type="submit">저장</button>
       (Name, SerialNumber 값이 POST로 전송)

브라우저: POST /Assets/Create  (폼 데이터 포함)

  → AssetsController.cs
       Create(Asset asset)  함수  (33~44줄)   ← POST용
         ModelState.IsValid  (이름 비었으면 다시 폼)
         asset.CreatedAt = ...
         _db.Assets.Add(asset);
         await _db.SaveChangesAsync();   ← 여기서 INSERT
         return RedirectToAction(nameof(Index));

브라우저: 자동으로 다시 GET /Assets/Index  (목록으로)

  → Index() 다시 실행 → 방금 넣은 줄 포함 표시

DB: ItAssetPortalDb.dbo.Assets 테이블에 행 1개 추가
```

**WinForms 감각:** 저장 버튼 Click 이벤트 = `Create(Asset asset)` POST 함수.

**이때 보는 파일 순서:**  
`Create.cshtml` → `AssetsController.cs` Create POST → `Models/Asset.cs` → `Data/AppDbContext.cs`

---

### ④ 껐다 켜도 데이터 있나?

```
F5 / 브라우저 / SSMS 끔  →  SQL Server Express 안 데이터는 그대로
다시 F5  →  Index() 가 다시 SELECT  →  목록에 그대로 보임
```

---

### ⑤ 한 장 요약 (등록만)

```
[자산 등록] 클릭
  Index.cshtml → GET → AssetsController.Create() → Create.cshtml

[저장] 클릭
  Create.cshtml → POST → AssetsController.Create(Asset)
    → AppDbContext → SQL INSERT → Redirect → Index 목록
```

---

## SSMS에서 한번 확인

등록 직후, 창고에 들어갔는지 보려면:

```sql
SELECT * FROM ItAssetPortalDb.dbo.Assets;
```

SSMS 안 켜도 웹은 됨. **확인용**으로만 열면 됨.  
자세한 클릭 순서: [15절](#15-sql-server-express-vs-ssms)

---

## 내가 헷갈렸던 것 (짧게)

- **화면** = 브라우저 HTML (`Views/*.cshtml`)
- **저장 처리** = 서버 `AssetsController` + `_db`
- **Create()가 두 개** = GET(폼 보여주기) / POST(저장) 이름만 같음
- WinForms 자세히: [20절](#20-winforms-vs-웹--질문하고-이해한-것)

---

## 메모 (내가 적는 칸)

- 

---

# 부록 (참고 백과)

> 아래는 **폴더 설명·줄별 해설** 등. 평소엔 위 「따라가기」만 봐도 됨.

---

## 0. 한 줄 요약 & F5 타임라인

### 이 프로젝트가 뭔지

**브라우저로 IT 자산 목록을 보고, 새 자산을 등록하는 사내용 웹**  
기술: **ASP.NET Core 8 MVC + SQL Server Express**

### F5 누르면 (시간 순서)

```
① Visual Studio가 Program.cs 실행
② 웹 서버 켜짐 (예: https://localhost:7xxx)
③ 브라우저가 주소 자동 오픈
④ 서버가 URL 해석 → AssetsController.Index()
⑤ AppDbContext → SQL Server에서 Assets SELECT
⑥ Index.cshtml + _Layout → HTML 생성
⑦ 브라우저에 「IT 자산 목록」 표시
```

**WinForms 비유:** `Main` → 폼 표시 → DB `SELECT` → Grid 채우기 (웹 버전).

### 꼭 외울 4가지 (나머지는 나중에)

| 이름 | 역할 한 줄 |
|------|------------|
| **Program.cs** | 전원 + 설정 |
| **Controller** | 요청 받아 처리 |
| **Model + DbContext** | 데이터 모양 + DB 통역 |
| **View** | HTML 화면 |

---

## 1. 프로젝트 생성 (어떤 템플릿?)

### Visual Studio에서

1. `새 프로젝트 만들기`
2. **ASP.NET Core 웹앱(Model-View-Controller)** 선택
3. 프레임워크: **.NET 8**
4. 인증: **없음** (1단계; 3단계에서 로그인 추가 예정)

### 터미널 (참고)

```bash
dotnet new mvc -n ItAssetPortal -f net8.0
```

### 용어

| 용어 | 뜻 |
|------|-----|
| ASP.NET Core | Microsoft **C# 웹** 프레임워크 |
| MVC | Model / View / Controller 로 화면·로직·데이터 분리 |
| WinForms 템플릿 | **아님** — 폼 디자이너 대신 브라우저 + HTML |

---

## 2. MVC + DB 큰 그림

```
[사용자 · 브라우저]
        ↕  HTTP (주소 입력, 링크 클릭, 폼 전송)
[Controller]     ← C# : "무슨 일 할지" 결정
        ↕
[Model]            ← C# 클래스 : 데이터 모양
        ↕
[AppDbContext]     ← EF Core : C# ↔ SQL 연결
        ↕
[SQL Server]       ← 실제 저장 (Express)
        ↕
[View .cshtml]     ← HTML : 사용자에게 보이는 것
```

### WinForms 비교표

| MVC | WinForms |
|-----|----------|
| View | Form, DataGridView, TextBox |
| Controller | 버튼 Click 이벤트 안의 코드 |
| Model | 데이터 클래스, DB 행 |
| Program.cs | `static void Main()` + 앱 전역 설정 |
| appsettings.json | App.config 비슷한 설정 파일 |

→ **질문하며 이해한 내용** (안 쓰는 이유, HTML vs 서버 역할 등): **[20절](#20-winforms-vs-웹--질문하고-이해한-것)**

### ItAssetPortal에서 브라우저 vs 서버

**이 프로젝트만** 보면 이렇게 나뉜다.

| 역할 | ItAssetPortal에서 뭐냐 |
|------|------------------------|
| **클라이언트** | **브라우저** — HTML 보여 주기, 「저장」 누르면 값만 전송 |
| **서버** | **ASP.NET (F5)** — `AssetsController`, EF, SQL INSERT/SELECT |
| **DB** | **SQL Server Express** — `ItAssetPortalDb.dbo.Assets` |

**MVC는 서버 안** 폴더 이름 그대로:

- `Views` = View · `Controllers` = Controller · `Models` + `Data` = Model/DB

WinForms랑 차이(화면 exe 안 vs 브라우저+서버): **[20절](#20-winforms-vs-웹--질문하고-이해한-것)**

### MySQL 수업이랑

- `SELECT`, `INSERT`, `WHERE` 개념 **동일**
- DB 프로그램만 **MySQL → SQL Server**
- 쿼리 창 도구: **Workbench/phpMyAdmin → SSMS**

---

## 3. A층 — 앱 켜기 & 설정

**역할:** 주방에 가스·전기 켜기. 아직 “요리(화면)”는 안 나옴.

### Program.cs

- 웹앱 **진입점** (예전 `Main` 역할)
- “MVC 써라”, “DB 이렇게 연결해라”, “기본 URL은 /Assets” 등록
- `app.Run()` 이후 서버가 **계속 켜져 있음** (F5 중지할 때까지)

→ 자세한 줄 설명: [11절](#11-programcs-줄별-설명)

### appsettings.json

- 앱 **설정 파일** (JSON)
- **ConnectionStrings:DefaultConnection** = SQL Server 주소
  - `localhost\SQLEXPRESS` = 인스턴스
  - `Database=ItAssetPortalDb` = DB 이름
  - `TrustServerCertificate=True` = 로컬 SSL 인증서 이슈 방지

### ItAssetPortal.csproj

- 프로젝트 정의: **.NET 8**, NuGet 패키지 목록
- 1단계에서 추가한 것: **Entity Framework Core SqlServer** 등

### Properties/launchSettings.json

- F5 시 **어떤 URL·포트**로 열지 (템플릿 기본)
- `applicationUrl` 에 `https://localhost:xxxx` 형태

### wwwroot/

- **정적 파일**: css, js, 이미지
- 브라우저가 **그대로 다운로드** (Controller 안 거침)
- Bootstrap CSS/JS 여기 또는 `lib` 하위

---

## 4. B층 — 데이터 (Model + DB)

**역할:** “자산 한 줄”이 어떤 칸을 갖는지 정의하고, SQL Server에 저장.

### Models/Asset.cs

- **테이블 한 행**을 C# 클래스로 표현
- `Id` : 번호 (자동 증가, PK)
- `Name` : 자산 이름 (필수)
- `SerialNumber` : 시리얼 (선택)
- `CreatedAt` : 등록 시각
- `[Required]`, `[Display]` : 검증·화면 라벨 (한글)

→ 자세히: [13절](#13-assetcs--appdbcontext)

### Data/AppDbContext.cs

- **DbContext** = “이 앱이 쓰는 DB 세션”
- `DbSet<Asset> Assets` = `Assets` 테이블에 해당
- Controller는 `_db.Assets.Add(...)` 처럼 **C#으로** DB 조작
- 내부적으로 EF가 **SQL 생성** (`INSERT`, `SELECT` …)

### SQL Server Express (별도 설치)

- 데이터가 **실제로 쌓이는** 프로그램 (백그라운드 서비스)
- 인스턴스 이름: **SQLEXPRESS**
- 1단계: `Program.cs`의 `EnsureCreated()` 가 DB·테이블 **없으면 생성**

### SSMS (별도 설치)

- DB **눈으로 보기**, SQL **직접 실행**
- Express = 창고, SSMS = 창고 관리 프로그램
- 연결: `localhost\SQLEXPRESS`, **서버 인증서 신뢰** 체크

---

## 5. C층 — 일 처리 (Controller)

**역할:** 브라우저 **요청(URL)** 을 받아, DB·View에 일 시키는 **접수 창구**.

### Controllers/AssetsController.cs (1단계 핵심)

| 메서드 | HTTP | 하는 일 |
|--------|------|---------|
| `Index` | GET | 목록 조회 → Index View |
| `Create()` | GET | 빈 등록 폼 → Create View |
| `Create(Asset asset)` | POST | 폼 데이터 저장 → 목록으로 Redirect |

- 생성자 `AssetsController(AppDbContext db)` : DB 연결 **주입** (직접 `new` 안 함)
- `return View(...)` : “이 View 파일 써”
- `RedirectToAction(nameof(Index))` : 저장 후 목록으로

→ 줄별: [12절](#12-assetscontroller-줄별-설명)

### Controllers/HomeController.cs (템플릿 잔여)

- 프로젝트 만들 때 **자동 생성**된 샘플
- Home, Privacy, Error 페이지
- 1단계 **핵심 아님** (기본 URL을 Assets로 바꿔서 거의 안 씀)

### URL이 Controller를 찾는 법

`Program.cs` 에 등록된 패턴:

```
{controller}/{action}/{id?}
```

예: `/Assets/Create` → Controller=`Assets`, Action=`Create`

→ 표: [9절](#9-url-규칙-표)

---

## 6. D층 — 화면 (View)

**역할:** 사용자에게 보이는 **HTML**. C# 변수를 끼워 넣을 수 있음.

### Views/Assets/Index.cshtml

- `@model IEnumerable<Asset>` : “목록 데이터 받는다”
- `@foreach` 로 표 `<tr>` 생성
- **자산 등록** 링크 → `/Assets/Create`

### Views/Assets/Create.cshtml

- `@model Asset` : “한 건 폼”
- `<form method="post">` : 저장 시 POST
- `asp-for="Name"` : Model 속성과 input **자동 연결** (Tag Helper)

### Views/Shared/_Layout.cshtml

- **모든 페이지 공통 틀** (메뉴, `<head>`, Bootstrap, footer)
- `<meta charset="utf-8" />` : 브라우저 한글
- `@RenderBody()` : 각 페이지 내용이 들어가는 구멍

### Views/_ViewStart.cshtml

```cshtml
@{
    Layout = "_Layout";
}
```

- 모든 View 열 때 **자동 실행**
- “Layout 씌워라” 한 줄 설정

### Views/_ViewImports.cshtml

```cshtml
@using ItAssetPortal.Models
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
```

- View마다 `using` 안 써도 `Asset` 이름 사용
- `asp-for`, `asp-action` 같은 **Tag Helper** 활성화

### .cshtml 이란?

- **Razor** = HTML + `@` 로 C# 삽입
- WinForms **폼을 텍스트로 작성**하는 느낌 (드래그 디자이너 없음)

→ View 주변 파일: [14절](#14-view-파일들-_layout-_viewstart)

---

## 7. 목록 보기 — 처음부터 끝까지

```
[1] 사용자: F5 또는 /Assets/Index 접속
[2] HTTP GET /Assets/Index
[3] 라우팅 → AssetsController.Index()
[4] _db.Assets.OrderByDescending(...).ToListAsync()
[5] EF Core → SQL Server: SELECT ... FROM Assets
[6] return View(assets)
[7] Razor: Index.cshtml 실행
[8] _ViewStart → _Layout 감싸기
[9] HTML 응답 → 브라우저 표 렌더링
```

**확인:** SSMS → `ItAssetPortalDb` → `dbo.Assets` → 행 개수

---

## 8. 자산 등록 — 처음부터 끝까지

### (1) 등록 화면 열기

```
[1] 「자산 등록」 클릭
[2] GET /Assets/Create
[3] AssetsController.Create()  (인자 없음)
[4] return View() → Create.cshtml (빈 폼)
```

### (2) 저장

```
[1] 이름 입력 후 「저장」
[2] HTTP POST /Assets/Create  (폼 필드 Name, SerialNumber 전송)
[3] AssetsController.Create(Asset asset)
[4] ModelState 검증 (이름 필수 등)
[5] asset.CreatedAt = UtcNow
[6] _db.Assets.Add(asset); SaveChangesAsync()
[7] EF → INSERT INTO Assets ...
[8] RedirectToAction(Index)  → 브라우저가 GET /Assets/Index 다시 요청
[9] 목록에 새 행 표시
```

**ValidateAntiForgeryToken** : 위조 POST 방지 (폼에 숨은 토큰).

---

## 9. URL 규칙 표

| 브라우저 주소 | Controller | Action | 설명 |
|---------------|------------|--------|------|
| `/` 또는 `/Assets` | Assets | Index | 목록 (기본) |
| `/Assets/Index` | Assets | Index | 목록 |
| `/Assets/Create` (GET) | Assets | Create | 등록 폼 |
| `/Assets/Create` (POST) | Assets | Create | 저장 처리 |
| `/Home/Index` | Home | Index | 템플릿 샘플 (비핵심) |

**규칙:** `/컨트롤러이름/메서드이름/선택id`

---

## 10. 템플릿 vs 우리가 만든 것

### 한눈에

| 구분 | 의미 |
|------|------|
| **템플릿** | `dotnet new mvc` / VS 생성 시 **자동**으로 생긴 파일 |
| **우리 추가** | 1단계 IT 자산 기능용으로 **새로 만든** 파일 |
| **우리 수정** | 템플릿 파일 중 **내용만 바꾼** 것 |

### 1단계 핵심 8개

| 파일 | 출처 |
|------|------|
| Program.cs | 수정 |
| appsettings.json | 수정 |
| Models/Asset.cs | 추가 |
| Data/AppDbContext.cs | 추가 |
| Controllers/AssetsController.cs | 추가 |
| Views/Assets/Index.cshtml | 추가 |
| Views/Assets/Create.cshtml | 추가 |
| Views/Shared/_Layout.cshtml | 수정 |

### 템플릿만 (지금 거의 안 씀)

- HomeController, Views/Home/*
- ErrorViewModel, Views/Shared/Error.cshtml
- wwwroot 기본 css/js, Bootstrap lib

### 프로젝트 문서 (실행과 무관)

- `STUDY_GUIDE.md` (이 파일)
- `PROJECT_CONTEXT.md`, `CHAT_GUIDE.md`

**12개 파일을 한꺼번에 외울 필요 없음** → [3~6절](#3-a층--앱-켜기--설정) 네 층만 이해.

---

## 11. Program.cs 줄별 설명

파일: `Program.cs`

| 줄 | 코드 | 설명 |
|----|------|------|
| 4 | `WebApplication.CreateBuilder` | 웹앱 빌더 생성 |
| 6 | `AddControllersWithViews` | MVC 모드 활성화 |
| 8-9 | `AddDbContext<AppDbContext>` | DB 연결 등록 (appsettings 문자열 사용) |
| 11 | `Build()` | 앱 객체 완성 |
| 13-17 | `EnsureCreated()` | DB/테이블 없으면 생성 (**1단계용; 나중에 Migration으로 전환 가능**) |
| 25-27 | `UseStaticFiles` 등 | css/js 제공, 라우팅, 권한(지금은 비어 있음) |
| 30-32 | `MapControllerRoute` | URL 규칙; 기본 `Assets/Index` |
| 34 | `Run()` | 서버 시작·대기 |

---

## 12. AssetsController 줄별 설명

파일: `Controllers/AssetsController.cs`

| 부분 | 설명 |
|------|------|
| `: Controller` | “이 클래스는 MVC Controller다” |
| `_db` | AppDbContext 인스턴스 (생성자로 받음) |
| `Index()` | DB에서 전체 읽기 → View에 목록 전달 |
| `Create()` GET | 등록 화면만 보여 줌 |
| `Create(Asset asset)` POST | 폼 → Asset 객체 바인딩 → DB 저장 → Redirect |
| `ModelState.IsValid` | Required 등 검증 실패 시 폼 다시 |
| `ValidateAntiForgeryToken` | POST 위조 방지 |

---

## 13. Asset.cs & AppDbContext

### Asset.cs (Model)

- DB 테이블 **Assets** 와 맞춤
- `Id` : EF가 PK·IDENTITY로 처리
- 속성 하나 = 컬럼 하나 (개념상)

### AppDbContext

- `DbContextOptions<AppDbContext>` : 연결 정보는 Program에서 주입
- `DbSet<Asset> Assets` : “Assets 테이블”에 대한 핸들

**EF Core 한 줄:** C#에서 `_db.Assets.Add(x)` → SQL `INSERT` 생성.

---

## 14. View 파일들 (_Layout, _ViewStart…)

| 파일 | 필수? | 역할 |
|------|-------|------|
| `_ViewStart.cshtml` | MVC 관례 | Layout 지정 |
| `_ViewImports.cshtml` | MVC 관례 | using, Tag Helper |
| `_Layout.cshtml` | 관례 | 공통 HTML 틀 |
| `_ValidationScriptsPartial.cshtml` | Create에서 사용 | jQuery 검증 스크립트 |

**Tag Helper 예**

- `asp-action="Create"` → URL 자동 생성
- `asp-for="Name"` → `Asset.Name` 과 input 연결

---

## 15. SQL Server Express vs SSMS

### Express vs SSMS (역할)

| | Express | SSMS |
|--|---------|------|
| 역할 | 데이터 **저장** (엔진) | **관리·조회** UI |
| 비유 | 창고 (월세 = 백그라운드 **서비스**) | 창고 들어가는 **열쇠·관리 앱** |
| 켜놔야 하나? | 웹/DB 쓸 때 **서비스 실행** | **필수 아님** — 볼 때만 실행 |
| 포트폴리오 | “SQL Server 사용” | SSMS 스크린샷 |

**ItAssetPortal(F5)** 은 Express에 **직접** INSERT/SELECT 함.  
**SSMS를 켜 두지 않아도** 웹은 동작함.

---

### SSMS에서 뭘 조작하나?

**매일 필수 아님.** 이럴 때만 열면 됨.

| 할 일 | 방법 |
|--------|------|
| 웹에서 등록한 게 DB에 들어갔나 | 아래 **쿼리** 또는 GUI |
| SQL 연습 / 포폴 | `SELECT` 스크린샷 |
| 문제 해결 | DB·테이블 존재 여부 확인 |

**개체 탐색기**에서 자주 가는 곳:

```
localhost\SQLEXPRESS
  └─ 데이터베이스
       └─ ItAssetPortalDb
            └─ 테이블
                 └─ dbo.Assets
```

---

### 방법 A — GUI (쿼리 안 쳐도 됨)

1. `dbo.Assets` **우클릭**
2. **상위 1000개 행 선택**
3. 아래 결과 창에 표 데이터 표시

→ 이때 `SELECT TOP 1000 ...` 이 **자동 생성**됨 (참고용).

---

### 방법 B — 쿼리로 데이터 보기 (SELECT)

#### 1) 새 쿼리 창 열기

1. SSMS에서 서버 연결 (`localhost\SQLEXPRESS`, **서버 인증서 신뢰**)
2. 상단 **새 쿼리** (또는 `Ctrl+N`)

#### 2) 복사해서 실행할 SQL

```sql
USE ItAssetPortalDb;
GO

SELECT * FROM Assets;
```

한 줄로 쓰려면:

```sql
SELECT * FROM ItAssetPortalDb.dbo.Assets;
```

#### 3) 실행

- **실행** 버튼(▶) 또는 **F5**
- 아래 **결과** 창에 행이 나오면 성공

#### 4) `ㅁㅁㅁ` 자리에 뭐가 들어가나

| SQL 조각 | 이 프로젝트에서 |
|----------|----------------|
| 데이터베이스 | `ItAssetPortalDb` (`appsettings.json`의 Database=) |
| 테이블 | `Assets` (`Asset.cs` / `DbSet<Asset> Assets`) |
| `dbo.` | 기본 스키마 (보통 `dbo.Assets`) |

MySQL 때: `SELECT * FROM students`  
지금: `SELECT * FROM Assets` (DB는 `USE`로 먼저 선택)

#### 5) 결과에 보이는 컬럼

| 컬럼 | 의미 |
|------|------|
| Id | 번호 (자동 증가) |
| Name | 자산 이름 |
| SerialNumber | 시리얼 (NULL 가능) |
| CreatedAt | 등록 시각 |

#### 6) 개수만 볼 때

```sql
SELECT COUNT(*) FROM ItAssetPortalDb.dbo.Assets;
```

---

### 자주 하는 실수

| 잘못된 예 | 이유 |
|-----------|------|
| `SELECT * FROM ItAssetPortal` | **프로젝트 폴더명** ≠ DB 이름 |
| `SELECT * FROM Asset` | 테이블명은 **`Assets`** (s 포함) |
| SSMS 안 켰더니 데이터 사라짐? | **아님** — Express에 저장됨, SSMS는 보기만 |

---

### 데이터는 꺼도 남나?

**남음.**

| 끈 것 | 데이터 |
|--------|--------|
| 브라우저 | 유지 |
| Visual Studio F5 (웹 서버) | 유지 |
| SSMS | 유지 |

웹에서 자산 2개 넣고 **다 꺼도**, Express 디스크에 저장된 건 그대로.  
다시 **F5** 하거나 위 `SELECT` 하면 2행 보임.

**사라지는 경우:** DB/테이블 삭제, `DELETE` 실행, DB 파일 삭제 등 **직접 지울 때만**.

---

### 웹 vs SSMS 확인 (같은 창고)

```
[웹 F5]  AssetsController → EF → INSERT/SELECT
[SSMS]   사람이 SELECT * FROM Assets 로 직접 확인
```

둘 다 **같은 `ItAssetPortalDb.dbo.Assets`** 를 봄.

---

## 16. 한글 깨짐 (UTF-8)

**증상:** `?맸귓`, `??` 같은 글자 (에디터만)

**원인:** 파일 UTF-8인데 Visual Studio가 **CP949**로 열 때

**해결**

1. 파일 닫기 → 다시 열기
2. **파일 → 다른 이름으로 저장** → **UTF-8 서명 있음 (65001)**
3. 프로젝트 `.editorconfig` : `charset = utf-8-bom`

**브라우저**는 `_Layout`의 `<meta charset="utf-8" />` 로 보통 정상.

---

## 17. 직접 만든다면 순서 (요리 레시피)

| 순서 | 할 일 | 확인 |
|------|--------|------|
| 1 | MVC 프로젝트 생성 | F5 빈 사이트 |
| 2 | appsettings 연결 문자열 | — |
| 3 | Asset 모델 | — |
| 4 | AppDbContext + Program 등록 | — |
| 5 | F5, SSMS에 DB/테이블 | EnsureCreated |
| 6 | AssetsController Index + View | 목록 |
| 7 | Create GET/POST + View | 등록 |

**한 번에 많은 파일 X** — 이 순서가 “요리 과정”.

---

## 18. 로드맵 & 학습 방식

### 로드맵

| 단계 | 내용 |
|------|------|
| 1 | MVC + DB + 자산 목록/등록 ← **지금** |
| 2 | 수정·삭제·검색 |
| 3 | 로그인·역할 (Identity) |
| 4 | 티켓·배정 이력 |
| 5 | SQL 대시보드 + README/ERD |

### 학습 방식 (합의)

- 코드 작성 ~80% / 이해·F5·SSMS ~20%
- **전체 흐름 먼저** → 그다음 파일 조금씩
- 단계 끝: 실행 확인 + 질문 2개
- 긴 설명 = **이 파일(STUDY_GUIDE)에 축적**

### 1단계 완료 체크

- [ ] F5 → 자산 목록
- [ ] 등록 → 목록에 반영
- [ ] SSMS `Assets` 테이블에 데이터
- [ ] 네 층(A/B/C/D) 설명 가능

---

## 19. 새 Cursor 채팅용 문장

```
ItAssetPortal. 경로 C:\GitHub\ItAssetPortal
@STUDY_GUIDE.md @PROJECT_CONTEXT.md 참고.
현재 1단계. 설명은 STUDY_GUIDE 백과사전 형식으로 천천히, 전체 흐름 우선.
```

---

## 20. WinForms vs 웹 — 질문하고 이해한 것

> 스스로 물어보고 맞게 이해한 내용을 백과사전에 고정해 둔 절입니다.

### Q1. 1단계라서 WinForms를 안 쓰는 거야? 나중에 쓸 예정이야?

**아니요.**

- **1단계라서 빠진 게 아님**
- **ItAssetPortal은 처음부터 웹(MVC)만** 가는 포폴
- **이 프로젝트 안에 WinForms를 넣을 계획 없음**

| | WinForms | ItAssetPortal (웹) |
|--|----------|---------------------|
| 화면 | Windows **데스크톱 창** | **브라우저** |
| 포폴 맥락 | .NET **유틸** 쪽에 가까울 수 있음 | .NET **웹** (공고와 맞춤) |

**WinForms 경험은 버리지 않음** — C#, SQL, 클래스, 이벤트 “무엇을 할지”는 같고, **UI만 다름**. 설명할 때 비유로만 씀.

---

### Q2. WinForms UI를 지금 HTML로 만든 거야?

**역할은 같고, 재료만 다름.** ✅

| 하는 일 | WinForms | 지금 웹 |
|---------|----------|---------|
| 목록 | DataGridView | HTML `<table>` (`Index.cshtml`) |
| 입력 | TextBox | HTML `<input>` (`Create.cshtml`) |
| 버튼 | Button | `<button>` / 링크 |
| 공통 틀 | Form + 메뉴 | `_Layout.cshtml` |

- 예전: Form **디자이너**로 배치  
- 지금: **HTML + Razor** (`.cshtml`) 로 작성  

**“무엇을 보여 줄지”** 는 같고, **그리는 도구**가 바뀐 것.

---

### Q3. WinForms는 exe 창 안에서만 하는데, 웹은 브라우저에서 하는 거야?

**맞음.** ✅

#### WinForms

```
.exe 실행 (F5)
  → 내 PC에 창 하나
  → 그 창 안에서만 버튼·그리드
  → 보통 그 PC 사용자만
```

#### 지금 웹

```
F5 → 웹 서버(ASP.NET) 켜짐
  → 브라우저가 주소(localhost) 접속
  → HTML 페이지가 브라우저에 표시
  → Chrome/Edge 안에서 목록·등록
  → (배포 후) 다른 PC도 같은 주소로 접속 가능
```

```
WinForms:  [ 내 PC ] → [ exe 창 ] → UI가 창에 박혀 있음

웹:        [ 서버 ] → HTML 생성·DB 처리
                ↑
           [ 브라우저 ] ← 여기서 UI가 보임
```

**개발 중(F5)** 은 서버 + 브라우저가 **둘 다 내 PC**에서 돌아감.  
**배포 후** 직원은 브라우저만 켜면 됨 (exe 설치 X).

---

### Q4. 웹에 보이는 건 HTML이고, 등록·리스트 반영은 서버 역할이야?

**맞음.** ✅

#### 브라우저 (보이는 것)

- 서버가 보낸 **HTML**을 그림
- CSS로 꾸밈
- `Create.cshtml` / `Index.cshtml` → 결국 **HTML**

#### 서버 (하는 일)

- **저장·DB 읽기·검증·리다이렉트**
- `AssetsController` + `AppDbContext` + SQL Server

```
[브라우저]                         [서버]
  등록 폼 표시 (HTML)        ←──   HTML 만들어 전송
  「저장」 클릭
  Name, Serial 전송         ──→   Create(Asset) → INSERT
  목록 페이지               ←──   Redirect → Index HTML
  표에 한 줄 보임           ←──   (DB에서 읽은 데이터)
```

| 일 | 담당 |
|----|------|
| 화면 그리기 (표, 폼) | **브라우저** (HTML) |
| 저장 버튼 **처리** | **서버** (Controller) |
| **DB** 저장·조회 | **서버** → SQL Server |

브라우저는 **값을 보내기만** 하고, **목록에 넣는 일**은 서버 + DB.

**확인 방법:** 「저장」 후 SSMS `Assets` 테이블에 행이 생기면 → 서버+DB가 한 일이 맞음.

---

### Q5. WinForms 때랑 “누가 일하나” 차이

| | WinForms | 웹 |
|--|----------|-----|
| UI | exe **창 안** | **브라우저** (HTML) |
| 저장 로직 | 보통 **같은 exe 안** C# | **서버** C# (Controller) |
| DB 연결 | PC에서 직접 | **서버**가 연결 |

예전: **창 + 로직이 한 프로그램**  
지금: **화면(브라우저)** 과 **일 처리(서버)** 가 **나뉨**

---

### 한 줄로 외우기

1. **이 포폴 = WinForms 안 씀** (웹만).  
2. **UI 역할** = WinForms 컨트롤 ≈ **HTML**.  
3. **실행** = exe 창 **vs** 브라우저.  
4. **보이는 것** = HTML / **등록·DB** = 서버.

---

## 메모 (본인이 적는 칸)

<!-- F5 결과, 에러, 이해한 점, 헷갈리는 층(A/B/C/D) -->

- 
