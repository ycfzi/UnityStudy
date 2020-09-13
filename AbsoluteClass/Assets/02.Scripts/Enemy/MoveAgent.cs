using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveAgent : MonoBehaviour
{
    public List<Transform> wayPoints;
    public int nextIdx;

    //변숫값을 변경할 필요가 없는 변수는 readonly나 const 키워드를 이용해
    //가비지 컬렉터에 쌓이지 않도록 하길 권한다
    private readonly float patrolSpeed = 1.5f;
    private readonly float traceSpeed = 4.0f;
    private float damping = 1.0f; // 회전할 때 속도 조절 계수

    private NavMeshAgent agent;
    private Transform enemyTr;

    //순찰여부판단
    private bool _patrolling;
    //patrolling 프로퍼티 정의 (getter, setter)
    public bool patrolling
    {
        get { return _patrolling; }
        set
        {
            _patrolling = value;
            if (_patrolling)
            {
                agent.speed = patrolSpeed;
                damping = 1.0f;
                MoveWayPoint();
            }
        }
    }

    //추적대상위치
    private Vector3 _traceTarget;
    //traceTarget 프로퍼티 정의 (getter, setter)
    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set
        {
            _traceTarget = value;
            agent.speed = traceSpeed;
            damping = 7.0f;
            TraceTarget(_traceTarget);
        }
    }

    public float speed
    {
        get { return agent.velocity.magnitude; }
    }

    void Start()
    {
        enemyTr = GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        //목적지에 가까워지면 속도가 줄어드는 것을 false
        agent.autoBraking = false;
        //자동회전 비활성화
        agent.updateRotation = false;
        agent.speed = patrolSpeed;

        var group = GameObject.Find("WayPointGroup");

        if (group != null)
        {
            //waypointgroup 하위에 있는 모든 transform 컴포넌트를 추출한 후
            //List 타입의 waypoints 배열에 추가
            group.GetComponentsInChildren<Transform>(wayPoints);
            //wayPointgroup 자체도 순찰 지점으로 활용하겠다면 아래 주석
            wayPoints.RemoveAt(0);
        }

        //처음 순찰 지점 찍은 뒤 움직이는 거 확인했던 함수
        //MoveWayPoint();
        this.patrolling = true;
    }

    void MoveWayPoint()
    {
        //최단 경로가 아니면 리턴
        if (agent.isPathStale) return;

        //목적지로 이동
        agent.destination = wayPoints[nextIdx].position;
        agent.isStopped = false;
    }

    void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale) return;

        agent.destination = pos;
        agent.isStopped = false;
    }

    public void Stop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        _patrolling = false;
    }

    void Update()
    {
        //적 캐릭터가 이동 중일 때만
        if (agent.isStopped == false)
        {
            //NavMeshAgent가 가야 할 방향 벡터를 쿼터니언 타입 각도로 변환
            Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
            //(a, b, float) a각도에서 b각도 사이를 시간 t에 따라 점진적으로 반환하는 함수
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);
        }

        //순찰 중이 아니라면 리턴
        if (!_patrolling) return;

        //움직이는 중이고 목적지에 도착했는지 확인
        if (agent.velocity.sqrMagnitude >= 0.2f * 0.2f &&
            agent.remainingDistance <= 0.5f)
        {
            //다음 목적지의 배열 첨자 계산
            nextIdx = ++nextIdx % wayPoints.Count;
            MoveWayPoint();
        }
    }
}
