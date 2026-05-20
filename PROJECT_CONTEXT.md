# ItAssetPortal — 프로젝트 컨텍스트 (다른 채팅에 붙여넣기용)

> 새 Cursor 대화를 열 때 **이 파일 전체** 또는 아래 "짧은 버전"을 첫 메시지에 붙여넣으면 이어서 진행 가능합니다.

## 목표
- 포트폴리오: 사내 **IT 자산 + 헬프데스크** (ASP.NET Core 8 MVC + SQL Server Express)
- 채용 타겟: 경영기획/IT (ERP·그룹웨어 느낌, .NET 웹, MS SQL)

## 진행 방식 (3번 로드맵)
| 단계 | 내용 | 상태 |
|------|------|------|
| 1 | MVC + DB + 자산 1테이블 목록/등록 | **진행 중** |
| 2 | 자산 CRUD 확장 | 대기 |
| 3 | Identity 로그인·역할 | 대기 |
| 4 | 티켓·배정 이력 | 대기 |
| 5 | SQL 대시보드 + README/ERD | 대기 |

## 학습 협업
- 코드는 AI가 ~80% 작성, 사용자는 실행·이해 확인 ~20%
- 단계마다 설명 + 확인 2문항 후 다음 단계

## 환경
- Visual Studio 2022, .NET 8
- SQL Server: `localhost\SQLEXPRESS` (Express 2025)
- SSMS 22, 연결 시 **서버 인증서 신뢰** 체크
- 연결 문자열: `TrustServerCertificate=True`

## repo
- 경로: `C:\GitHub\ItAssetPortal`
- UI: 한글
- **학습 노트 (버튼 따라가기·개인용):** `STUDY_GUIDE.md` 맨 위 「따라가기」

---

## 짧은 버전 (새 채팅용 복붙)

```
ItAssetPortal 포트폴리오 (ASP.NET Core 8 MVC + SQL Express).
경로: C:\GitHub\ItAssetPortal
로드맵 3번 단계식. 현재 1단계: 자산 테이블 + 목록/등록.
DB: localhost\SQLEXPRESS, TrustServerCertificate=True.
진행/이해 질문은 PROJECT_CONTEXT.md 와 최신 커밋 기준으로 이어서 도와줘.
```

---

## 진행 로그 (단계 완료 시 여기 업데이트)

### 1단계
- [x] 프로젝트 생성
- [x] EF + Assets 테이블
- [x] 목록 / 등록 화면
- [ ] 사용자 F5 실행 + SSMS 확인
- [ ] 이해 확인 2문항
