using UnityEngine;
using UnityEngine.AI;

public class StayingAndShootingEnemy : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private float timeToRotate = 2f;
    private float startWaitTime = 2f;

    [SerializeField] private float viewRadius = 15f;
    [SerializeField] private float viewAngle = 120f;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask obstacleMask;
    private float meshResolution = 1f;
    private int edgeIterations = 4;
    private float edgeDistance = 0.5f;

    [SerializeField] private Transform[] wayPoints;
    private int m_СurrentWayPointIndex;

    private Vector3 playerLastPosition = Vector3.zero;
    private Vector3 m_PlayerPosition;

    private float m_WaitTime;
    private float m_TimeToRotate;
    private bool PlayerInEnemyRange;
    private bool playerIsNearEnemy;
    private bool enemyIsPatrolingOnPoint;
    private bool enemyCanShoot;
    private bool isDetectedFromBehind;
    
    
    private void Start()
    {
        m_WaitTime = startWaitTime;
        
        m_PlayerPosition = Vector3.zero;
        enemyIsPatrolingOnPoint = true;
        enemyCanShoot = true;
        isDetectedFromBehind = false;
        PlayerInEnemyRange = false;
        m_TimeToRotate = timeToRotate;

        m_СurrentWayPointIndex = 0;
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _navMeshAgent.isStopped = false;
    }
    
    private void Update()
    {
        EnviromentView();

        if (!enemyIsPatrolingOnPoint)
        {
            Shooting();
        }
        else
        {
            PatrolingOnPoint();
        }
    }

    private void Shooting()
    {
        playerLastPosition = Vector3.zero;
        
        if (enemyCanShoot)
        {
            var targetLook = new Vector3( m_PlayerPosition.x, transform.position.y, m_PlayerPosition.z);

            transform.LookAt(targetLook);
        }
    }

    private void PatrolingOnPoint()
    {
        if (playerIsNearEnemy)
        {
            if (m_TimeToRotate <= 0)
            {
                LookingOnPlayer(playerLastPosition);
            }
            else
            {
                m_TimeToRotate -= Time.deltaTime;
            }
        }
        else
        {
            playerLastPosition = Vector3.zero;
        }
    }

    private void LookingOnPlayer(Vector3 player)
    {
        if (Vector3.Distance(transform.position, player) <= 20f)
        {
            if (m_WaitTime <= 0)
            {
                Shooting();
                m_WaitTime = startWaitTime;
                m_TimeToRotate = timeToRotate;
            }
            else
            {
                m_WaitTime -= Time.deltaTime;
            }
        }
    }

    private void EnviromentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            
            if (Vector3.Distance(transform.position, player.position) <= 20f)
            {
                playerIsNearEnemy = true;
                m_PlayerPosition = player.position;
            }
            else
            {
                playerIsNearEnemy = false;
                m_WaitTime = startWaitTime;
                m_TimeToRotate = timeToRotate;
            }
            
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            
            if (!Physics.Raycast(transform.position, dirToPlayer, distanceToPlayer, obstacleMask))
            {
                if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
                {
                    PlayerInEnemyRange = true;
                    enemyIsPatrolingOnPoint = false;
                }
                else
                {
                    PlayerInEnemyRange = false;
                }
            }

            if (Vector3.Distance(transform.position, player.position) > viewRadius)
            {
                PlayerInEnemyRange = false;
            }
            
            if (PlayerInEnemyRange)
            {
                m_PlayerPosition = player.transform.position;
            }
        }
    }

    
}
