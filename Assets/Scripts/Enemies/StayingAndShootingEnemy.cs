using UnityEngine;

public class StayingAndShootingEnemy : MonoBehaviour
{
    private float timeToRotate = 2f;
    private float speedOfRotation = 5f;
    private float startWaitTime = 2f;

    [SerializeField] private float viewRadius = 15f;
    [SerializeField] private float viewAngle = 120f;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask obstacleMask;
    
    private Vector3 m_PlayerPosition;
    private Vector3 firstPlayerRotation;

    private float m_WaitTime;
    private float m_TimeToRotate;
    private bool enemyLookingOnPlayer;
    private bool playerInEnemyRange;
    private bool playerIsNearEnemy;
    private bool enemyIsPatrolingOnPoint;
    private bool enemyCanShoot;
    private bool isDetectedFromBehind;
    
    
    private void Start()
    {
        firstPlayerRotation = transform.localEulerAngles;
        m_WaitTime = startWaitTime;
        
        m_PlayerPosition = Vector3.zero;
        enemyIsPatrolingOnPoint = true;
        enemyLookingOnPlayer = false;
        enemyCanShoot = true;
        isDetectedFromBehind = false;
        playerInEnemyRange = false;
        m_TimeToRotate = timeToRotate;
        
    }
    
    private void Update()
    {
        //Debug.Log("enemyLookingOnPlayer = " + enemyLookingOnPlayer + " PlayerInEnemyRange = " + playerInEnemyRange + " isPatrolling = " + enemyIsPatrolingOnPoint + " playerIsNearEnemy = " + playerIsNearEnemy);
        EnviromentView();
        
        if (enemyIsPatrolingOnPoint == false)
        {
            if (playerInEnemyRange)
            {
                if (enemyLookingOnPlayer == false)
                {
                    Vector3 dirToPlayer = (m_PlayerPosition - transform.position).normalized;
                    if (Vector3.Angle(transform.forward, dirToPlayer) <= 5f)
                    {
                        enemyLookingOnPlayer = true;
                    }
                    else
                    {
                        var newRotation =
                            Quaternion.LookRotation(m_PlayerPosition - transform.position, Vector3.forward);

                        newRotation = new Quaternion(0, newRotation.y, 0, newRotation.w);
                        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation,
                            speedOfRotation * Time.deltaTime);
                    }
                }
                else
                {
                    Shooting();
                }
            }
        }
        else
        {
            PatrolingOnPoint();
        }
    }

    private void Shooting()
    {
        var targetLook = new Vector3( m_PlayerPosition.x, transform.position.y, m_PlayerPosition.z);
        transform.LookAt(targetLook);
    }

    private void PatrolingOnPoint()
    {
        if (playerIsNearEnemy)
        {
            if (m_TimeToRotate <= 0)
            {
                PlayerWasDetectedByEnemy(m_PlayerPosition);
            }
            else
            {
                m_TimeToRotate -= Time.deltaTime;
            }
        }
        else
        {
            m_WaitTime = startWaitTime;
            m_TimeToRotate = timeToRotate;
        }
    }

    private void PlayerWasDetectedByEnemy(Vector3 player)
    {
        if (m_WaitTime <= 0)
            {
                enemyIsPatrolingOnPoint = false;
                playerInEnemyRange = true;
                m_WaitTime = startWaitTime;
                m_TimeToRotate = timeToRotate;
            }
            else
            {
                m_WaitTime -= Time.deltaTime;
            }
    }

    private void EnviromentView()
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;

            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            
            if (!Physics.Raycast(transform.position, dirToPlayer, distanceToPlayer, obstacleMask))
            {
                if (Vector3.Distance(transform.position, player.position) <= 5f)
                {
                    playerIsNearEnemy = true;
                    m_PlayerPosition = player.position;
                }
                else
                {
                    playerIsNearEnemy = false;
                }
                
                if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
                {
                    playerInEnemyRange = true;
                    enemyIsPatrolingOnPoint = false;
                }
            }
            else
            {
                playerInEnemyRange = false;
            }
            
            if (Vector3.Distance(transform.position, player.position) > viewRadius)
            {
                playerInEnemyRange = false;
            }

            if (playerInEnemyRange)
            {
                m_PlayerPosition = player.transform.position;
                enemyIsPatrolingOnPoint = false;
            }
            else
            {
                enemyLookingOnPlayer = false;
                enemyIsPatrolingOnPoint = true;
            }
            
        }
    }
}