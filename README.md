# IT Asset Portal

사내 IT 자산(노트북, 장비 등)을 등록하고 조회하기 위한 ASP.NET Core MVC 프로젝트입니다.  
ASP.NET 학습을 목적으로 진행하는 스터디 프로젝트입니다.  
현재는 **1단계(기초 기능 구축 단계)** 이며, 기능을 하나씩 순차적으로 추가하고 있습니다.

## 현재 단계 (Step 1)

구현 완료:
- 자산 목록 조회 (`Assets/Index`)
- 자산 등록 (`Assets/Create`)
- 기본 유효성 검사 (Data Annotations)
- EF Core + SQL Server 연동
- 앱 시작 시 DB 자동 생성 (`EnsureCreated`)

아직 진행 중:
- 수정/삭제 기능
- 검색/필터/정렬 고도화
- 인증/권한 처리
- 운영 배포 구성

## 프로젝트 구조

```text
Controllers/   # MVC 컨트롤러
Data/          # DbContext
Models/        # 도메인 모델
Views/         # Razor 뷰
wwwroot/       # 정적 파일(css/js/lib)
```

## 로드맵 (초안)

- [x] Step 1: 자산 등록/조회 기본 기능
- [ ] Step 2: 수정/삭제(CRUD 완성)
- [ ] Step 3: 검색/필터 및 UI 개선
- [ ] Step 4: 인증/권한 및 운영 준비
