# Kinect와 Unity AR을 활용한 운동 자세 교정 시스템


## 프로젝트 목표

* Kinect로부터 Body Tracking된 데이터를 제공받아 Unity AR캐릭터로 출력하는 동시에 Joint 각도를 계산하여 실시간으로 운동자세에 대한 피드백을 제공한다.

## 지도 교수님

이승규 교수님

## 팀원
고다경 2017103948
김다솜 2017103960
이상윤 2014104125

## 적용 기술 요약
### 각도에 대한 판별
+ 운동에 대한 기초 조사를 진행한 후, Kinect에서 Tracking을 잘할 수 있을 것으로 예상되는 운동 3개를 선정. 
+ 운동 자세 판단은 각 운동 별 주요 관절의 각도를 보고 판단함.
	+ 스쿼트 :  스쿼트 운동을 잘 하고 있는지 판단하는 데 기준이 된 관절은 오른쪽 무릎, 왼쪽 무릎, 척추관절, 골반, 양 쪽 발 끝
	+ 사이드하이킥 : 사이드하이킥 운동을 잘 하고 있는지 판단하는 데 기준이 된 관절은 양쪽 골반, 척추 관절.
	+ 런지: 런지 운동을 잘 하고 있는지 판단하는 데 기준이 된 관절은 오른쪽 무릎, 왼쪽 무릎, 척추관절.
+ 각 관절의 각도는 Joint들의 position을 이용하여 Vector3D 구조체를 만든 후, 벡터의 내적을 통해 계산함. 
+ 운동을 잘 하고 있는지 판단할 기준이 될 각도를 정하여 Rule-based 방식을 통해 사용자가 운동을 잘 하고 있는지 아닌지 즉각적으로 직관적인 Text를 표시함

### 주요 클래스 및 함수 설명 
+ Angle Class - Vector3D 구조체 생성 및 정규화, 벡터 내적을 통한 각도 계산
+ DetectJoints Class - Kinect SDK가 전송하는 joint별 position을 받아와 계산 후, GameObject position에 할당.
+ BodySourceManager - Kinect와 Unity를 연결
	+ 키넥트에서 Joint position 받아와 아바타에 넣어주는 역할을 수행함
+ Move - AR을 움직이며 Update를 계속 수행
	+ Move 클래스에 트레이너와 트레이니 오브젝트 변수 선언, CharacterSkeleton 선언
	+ start()함수에서 게임오브젝트의 스켈레톤 생성
	+ update()문을 돌면서 tracking된 joint를 계속 update
    + CreateBodyObject 함수 - body object 생성
    + RefreshBodyObject 함수 - AR의 Joint를 계속 Refresh함.
	    + 트레이니 운동 자세 각도 판단
    + Trainer_Run 함수 - Trainer를 담당하는 함수
	    + 운동 선택에서 넘겨진 운동에 대한 file stream 수행
	    + txt파일 하나마다 한 동작, count를 적용
	    + txt파일에 정리된 Kinect 값은 데이터 전처리를 통해 적당하게 움직이도록 미리 제작
    + Trainee_Count 함수 - 운동 별 트레이니의 운동 횟수 카운트

### 코드 활용방법
1. Unity를 이용하여 Assets파일 위치에서 Unity Project Open
2. Script폴더에 있는 c#파일들을 Code editor를 이용해 Open
3. Kinect는 Kinect SDK설치 후 USB3.0을 이용하여 연결 (Intel CPU 7세대에서 오류 발생, 8세대 이상 권장)
4. Unity 실행 시 항상 Kinect studio 또는 Configuration 실행 권장