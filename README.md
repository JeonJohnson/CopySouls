# CopySouls

 YoungSan_22_2_TeamProject
---------------------------------------------------------------------
2022 - 2학기 영산대학교 캡스톤 팀프로젝트 - CopySouls

----------------------------코딩규칙--------------------------------
변수
ㄴ 첫글자 소문자, 자료형 형식은 뒤에 붙이기
EX) playerHp, playerTr

함수
ㄴ 데이터 참조 필요하면 함수로 끌어올수 있도록
ㄴ 첫글자 대문자, 함수명은 동사-명사순으로
Ex) CalcPrice, AlignmentInventory

구조체, 열거형, static변수들, const 변수들은 따로 스크립트 빼기
ㄴ 라이브러리 클래스파일 하나 만들예정

★다른사람 코드 수정할일 있으면 먼저 연락하고 수정하기★
----------------------------------------------------------------------

----근희할일----
~09.27 적 애니메이션 구하고 FSM 적용시켜보기
~10.04 애니메이션 행동트리와 연동후 적 관련 오브젝트 풀링/유닛매니저 제작
~10.18 아처스테이트 완성하기
~10.25 플레이어 발꾸락말고 대구빡이나 몸통 을 타겟으로 잡기 에너미도 발꾸락 기준말고 뮨상기준으로 dir계산, fov계산하기. Bow -> Weapon 상속받기
~11.15 근접몬스터 제작 완료 후, 보스몬스터 제작 들어가기

----대원할일----
~10.04 카메라 캐릭터 팔로우기능, 벽 충돌기능 제작
~10.25 카메라 디테일 잡기
~11.15 카메라 쉐이크 끝내고, UI/인벤토리/퀵슬롯 작업(feat 용석, 정민)

----용석할일----
~09.27 적 애니메이션 구하고 FSM 적용시켜보기
~10.04 적 관련 오브젝트 풀링/유닛매니저 제작
~10.25 레그돌, hit,damaged함수 적용, att패턴 만들기
~11.15  근접몬스터 완성 후 플레이어와 상호작용 기능 끝내기, , UI/인벤토리/퀵슬롯 작업(feat 정민)

----정민할일----
~09.27 플레이어 이동시스템 완성시키기 , 기본공격/플레이어 피격 함수 만들기
~10.18 공격패턴(강공, 약공, 대쉬, 앞잡, 뒤잡) 완성, 애니메이션 이벤트 적용시키기
ㄴ 카메라 관련 연동효과(이동속도에 따라 카메라 관성효과, 카메라가 캐릭터 약간 쳐다보게)
~10.25 플레이어 hit 함수 행님 방식대로 바꾸기 플레이어 무기 weapon 상속받게 스크립트 바꾸기 남은 공격함수 작업
~11.15 적과 상호작용 기능(데미지 주고받기, 잡기공격) 끝내고, UI/인벤토리/퀵슬롯 작업(feat 용석)

근희&용석 
Enemy 스크립트 정리하기
+ UnitManager에서 적 생성하고 지우고 리스트 관리하는거 까지
-------------------------------------일정표--------------------------------------

10.11 ~ 10.18
각자 작업물 디테일 제외하고 작동만 되게 다 만들어서 합칠준비되게 하기

플레이어는 카메라 연동하고 남은 공격함수 기능들 전부 만들기

카메라 쪽은
플레이어 속도와 카메라 관성효과 연동

10.18 ~ 10.25
18일날 다 모여서 한 씬에 합치기
20일날 부터 시험기간



10.25 ~ 11.01
시험기간 끝나고 회의해서
인벤토리, 퀵슬롯, 상점, 화톳불(저장) , 스탯찍기 , 스킬, 몬스터 루팅 중 구현할거 고르기

11.01 ~ 11.08
근희 : 03, 04일날 교수님 부탁 프로젝트 끝나면 작업할거 정해서 카톡방에 보고
용석 : 플레이어랑 상호작용 끝나고나면 몬스터 디테일
대원 : 플레이어랑 연동되는 디테일 부분 작업 + 카메라 쉐이크
정민 : 플레이어 스테미나, 차지어택, 가드,  패링 (별 문제 없으면 하루만에)
		금방 끝나면 
			=>1. 무기 양손잡/한손잡 (예정)
			=>2. 패링 됐을 때 적 병123신 되는거 (요청)

----2022-11-01 회의록----
**보고사항**
전근희 : 도킷도킷 교수님과의 노예계약 중.
용석 : (쉐이더이슈) 리제로 
대원 : 시험 + 휴가 
정민 : 플레이어 피격 함수 쪽 수정 (Player_HitBox 레이어에다가 충돌 판정 나도록)

**계획** (1101~1108)
근희 : 11월 3~4일 까지 노예계약 유지 예정. 그 이후에 내 코드보고 수정할거, 추가할거 카톡으로 얘기 할 예정.
용석 : 플레이어랑 상호작용 (줘패고 줘 터지는거)
			=> 몬스터 디테일
대원 : 머장님 오더 수행 (player & 카메라 콜라보레이숀)
정민 : 플레이어 스테미나, 차지어택, 가드,  패링 (별 문제 없으면 하루만에)
		금방 끝나면 
			=>1. 무기 양손잡/한손잡 (예정)
			=>2. 패링 됐을 때 적 병123신 되는거 (요청)

**전반적인 시스템 우선 순위**
1. 카메라 쉐이크 : 쉐이크 컴포넌트(or매니저) 제작 하면, 각자 쉐이크 넣을 사람이 쉐이크이벤트 컴포넌트의 변수를 각자 조절해서 실행 시키기 - 完
		=> ㅇㅈ? 
2. 인벤토리, 퀵슬롯, 몬스터 루팅
3. 화톳불(저장)
4. 스킬
5. 스탯 (후순위)
6. 맵 깔고 게임플로우 세팅


** 시스템 구현할거 회의 **
인벤토리, 퀵슬롯, 상점, 화톳불(저장) , 스탯찍기 , 스킬, 몬스터 루팅 중 구현할거 고르기

1. 인벤토리, 퀵슬롯 (대충일단 Image 흰색 깔아두고 ui 루다가) V
	=> 근희 : rpg인디 ㅋㅋ 허쉴ㅋㅋ
	=> 정민 : ㄹㅇㅋㅋ
	=> 용석 : ㅋㅋ 만든거 있음 ㅋㅋ ㄱㄱ
	=> 대원 : ㄹㅇㅋㅋ 

2. 상점 (NPC, 상점 UI, 논 하드 벗 베리 귀찮) X 
	=> 정민 : 후 순위일듯
	=> 근희 : ㄹㅇㅋㅋ 
	=> 용석 : 하기 싫음
	=> 대원 : 후 순위

3. 화톳불(체크포인트, 휴식) 
	=> 근희 : 저장 기능은 제외하고 (체크포인트 = 맵에 적 리셋 + 기본 물약 재 충전 + 플레이어 스텟 리셋) V 
	=> 정민 : ㄹㅇ ㅋㅋ
	=> 용석 : ㄹㅇㅋㅋ
	=> 대원 : ㄹㅇ ㅋㅋ

4. 플레이어 스탯(육성) X
	=> 근희 : 후순위(귀차낭)
	=> 정민 : ㅇㅈ 
	=> 대원 : ㅇㅈ
	=> 용석 : ㅇㅈ 

5. 스킬 (무기마다 특수기 + ㄹㅇ스킬트리 시스템 + 마법 시스템) : 마법시스템V
	=> 근희 : 마법 시스템은 찬성(몬스터랑 리소스 애니메이션, fx 공유 되도록)
	=> 정민 : 무기마다 특수기 이미 있음 ㅋㅋ + 마법시스템 ㅇㅈ
	=> 대원 : 스킬트리 제외하고 ㄱㄱ
	=> 용석 : 스킬트리 제외하고 ㄱㄱ

6. 몬스터 루팅 (선 작업으루다가 인벤토리 완성  1. 루팅 안내 Ui , 2.아이템 이펙트 혹은 드랍 아이템 모델링 3. 인벤토리 상에서 나올 아이템 UI) V
	=> 근희 : 찬성
	=> 용석 : 아이템 자체가 준비가 안되있으니까 일단 후순위
	=> 대원 : 인벤토리 만들어지면 바로 ㄱㄱ ㅇㅈ?
	=> 정민 : ㅇㅈ ㅋㅋ

7. 컷씬()
	=> 근희 : 11/02일에 게임 완성되면 찬성
	=> 용석 : 후 순위
	=> 정민 : 반대
	=> 대원 : 칵 퉷







///////////
11.01 ~ 11.08
필드 만들기, 씬트리 제작 작업하면서 골라잡은 구현내용 연동시키기
UI는 기본 스프라이트 끼워만 두기


11.08 ~ 11.15
각자 파트 끝내기

11.15 ~ 11.22
디테일 작업시작
UI 에셋 연동, 

-----------------------------------------------
******데드라인 11월 말******
1. 인벤토리, 퀵슬롯, 몬스터 루팅 : 용석, 정민(2주)
2. 맵 깔고 게임플로우 세팅 + 화톳불(씬 리셋) : 대원
3. 보스 : 근희 ()
-> 인벤,퀵,루팅 완료시 일손 필요한 곳 붙기

***이후 여유 남으면***
1. 이펙트, 사운드 -> 인벤, 퀵, 루팅 완료시 한명 붙기 

2. 스킬 + 스탯 (후순위)
-----------------------------------------------
******데드라인 11월 말******
현재 발견되는 버그 : 존나 쉬발 크리티컬한거 아니면 패스하는데 존나 불가항력적이거나 자주 나오는거면 담당 제작자한테 말하고 견적 짜보고 견적 안나오면 패스패스패스ㅐ프새프ㅐ스패ㅡ햇그
-----------------------------------------------


11.22 ~ 11.29
사운드 넣고 테스트


---------------------------------------------
11:06 2022-11-29 1차 발표 후
**버그목록**
UI
애들 재 생성되고나서 체력바 위치 오류 있음. -> 근희
다시 살아났을 때 유다이 캔버스 리셋해야함. -> 근희
두번 죽을때 youDiedUI씹창나는거 -> 근희
다시 타이틀씬 갔을때 마우스 락 풀기 -> 정민

흐름
씬초기화 되면 필드 아이템들 지워주기 -> 용석 (인벤토리안에 만들어 놓을께 ) -> 근희 인게임매니저에서  처리

몬스터
골렘 히트 부분 버그 -> 공격중에 맞았을 때 가끔 멈춤 -> 씨발 근희
스피릿 무기 떨림 -> 용석

아이템,인벤토리
포션 여러개 한꺼번에 버렸을때 먹으면 1개만 먹어짐 -> 정민
인벤토리랑, 플레이어 스텟창 열렸을때 바로 마우스풀리고 카메라 락, 플레이어 락 -> 정민


**추가 작업 내역
라이트
필요없는 라이팅 제외하고 베이크로 바꾼뒤에 굽기(정민이가) -> 정민

카메라 쉐이크
잘 -> 용석, 대원

카메라 연출
어디 좀 큰 공간 들어갔을때 카메라 줌 min값 늘리기 (공간감 극대화) -> 대원
플레이어 달리거나 차징공격, 구르기 공격 처럼 팍 움직이는 부분에서 카메라 팔로우 속도 줄여서 가속 너낌 나오게 -> 대원

사운드
사운드 매니저 -> 근희

이펙트
각자 위시리스트 만든거 사서 -> 정민

---스케쥴---
12월 06일 : 마무리 
	- 캡스톤 수업에 다 모여서 릴리즈 뽑고 버그 확인
		=> 크리티컬한 버그 아니면 패스하고 그냥 시연할때 하지 않기
		=> 이후로 개인 작업은 깃에 올리지 말고 개인적으로 하기
	- 저녁에 디코에 모여서 영상 찍기 
12월 20일 : 영상 발표

