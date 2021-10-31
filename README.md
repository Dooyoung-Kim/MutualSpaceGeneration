# MutualSpaceGeneration

목표

서로 다르지만 유사한 구조를 지닌 AR 호스트의 공간(>20m^2)과 VR 클라이언트의 공간으로부터 테이블 기반 원격 협업에 적합한 공유 공간 생성
공유 공간 생성을 위한 호스트 공간 및 클라이언트 공간 정합 방법 및 최적화를 위한 공간 변형 수치 계산 알고리즘 제안

코드 실행 방법

Unity Project를 2020.3.19f 버전에서 염
‘MutualSpace’ Scene을 열면 AR Host Space (초록색 벽)과 VR Client Space(하얀색 벽)이 있음
‘Game’ Tab을 하나 더 추가하여 열고 ‘Display 4’로 설정함
플레이 버튼을 누르면 VR Table Anchor의 위치로 AR Host Space가 이동함. 그리고 Display 4에서 AR Host Space와 VR Client Space가 기본 기하 정보를 기반으로 label 된 것을 확인할 수 있음
공간 정합은 아래 키를 클릭하여 할 수 있음
키보드 Spacebar : 공간 정합률이 최대가 되는 AR Rotation 값으로 AR Host Space가 돌아가고, Scaling 값으로 VR Client Space 공간의 스케일이 변형됨. 최대가 되는 AR Host 공간의 Rotation Quaternion 값, VR Client 공간의 Relative Translation gain을 적용한 공간 변형률 (alpha, beta), 그리고 최종 공간정합률 maxMatchRate를 계산하여 Console 창에 보여줌.
키보드 Z : 현재 공간의 정합률을 계산함
키보드 X : Rotation 값만을 변형하여 정합률이 최대가 되는 Rotation 값을 찾음 (Scale 변형 없음)
키보드 C : VR Client 공간의 Scale 값만을 변형하여 정합률이 최대가 되는 (x,z) scale 값 alpha, beta를 찾음.
Spacebar 키를 활용해 찾은 AR Host 공간의 Rotation Quaternion 값, VR Client 공간의 Relative Translation gain을 적용한 공간 변형률 (alpha, beta)를 활용해 두 공간의 위치와 스케일을 변형하고 가장 가까운 벽면을 기준으로 새로운 벽면과 겹쳐진 가구들을 놓으면 최종 공유 공간 layout을 만들 수 있음.
위에서 결정된 Layout을 활용해 AR Host Space를 기반으로 공유 공간을 생성함.
(최종 생성된 공유 공간은 Prefab > MutualSpace 폴더 안에 추가해두었음.)


기본 전제

AR Host와 VR Client의 3D CAD 형태의 Digital Twin (DT) 존재
DT에 있는 객체와 공간 레이아웃에는 Basic Geometry Label이 존재함 (Floor, Table, Furniture, Wall)
AR DT와 VR DT에는 커다란 테이블이 놓여져있으며, 이 테이블을 중심으로 협업이 진행됨
VR DT에 있는 사용자의 공간에 AR DT를 정합하여 공유 공간을 생성함


정합 알고리즘 작동 원리

Semantic Segmentation
AR 호스트 디지털 트윈과 VR 클라이언트 디지털 트윈을 기본 기하 정보(Floor, Wall, Table, Furniture)에 따라 Semantic Segmentation을 함. 
ARSegmentedCam과 VRSegmentedCam을 통해 Label에 따라 서로 다른 색으로 Semantic Segmentation을 수행하고 각각의 카메라로 이를 Observe 함.
(‘MutualSpace’ Scene에서 Play 재생 후 Game Scene의 ‘Display 4’ 선택시 각각 AR, VR 공간이 Semantic Segmentation 된 TopView를 볼 수 있음)


공간 정렬 (Table Anchor 기준)
Table의 중심 (x,z) 값을 Table Anchor로 지정하고 AR Table Anchor와 VR Table Anchor의 Transform 값을 할당함. VR Client의 공간에 AR Host의 공간을 불러와 정합하는 시나리오이므로 VR Table Anchor에 AR Table Anchor를 이동시키고 AR Table Anchor로 ARSpace를 이동시킴


공간 정합 알고리즘
AR Host 공간의 Rotation
공간 정합의 목표는 몰입감 있는 협업을 위함. 단순히 공간의 넓이를 최대화하는 것이 목표가 아니기에 테이블과 벽면의 정렬을 맞춰줘야할 필요가 있음. 이를 위해 AR 호스트 공간과 VR 클라이언트 공간이 수직 수평을 이루며 정렬을 이루는 범위 내에서 회전값을 조절함.
VR Client 공간의 Scale (with Relative Translation Gains)
Redirected Walking 기술을 활용해 가상현실 사용자의 공간을 변형하는 효과를 주어 보다 넓은 협업 환경을 생성하기 위함. VR2022에 투고한 논문에서 Small X Furnished 조건(면적: 27.04m^2, 테이블과 가구가 배치된 컨퍼런스 룸)에서 Relative Translation gain threshold는 0.96 ~ 1.24로 추정됨. 즉, 한 축에 비교하여 다른 한 축을 4% 더 줄이거나 24% 더 늘여주는 것이 가능. [Steinicke et al. 2016]에서 추정한 결과에 의하면 한 축은 최대 14% 줄일 수 있고, 26%까지 늘어날 수 있음. 이 두 가지 범위 내에서 공간 변형을 시행함.
최적값 계산
Rotation 값과 Scale 값을 조합하여 최대의 공간 정합률을 달성하는 최적의 값들을 찾아냄. 

