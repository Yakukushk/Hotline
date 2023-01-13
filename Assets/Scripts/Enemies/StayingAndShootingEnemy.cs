using UnityEngine;

public class StayingAndShootingEnemy : MonoBehaviour
{
    private float timeToRotate = 1f;
    private float speedOfRotation = 20f;
    private float startWaitTime = 1f;
    private float timeToRotateToStartRotation = 10f;

    [SerializeField] private float viewRadius = 15f;
    [SerializeField] private float viewAngle = 90f;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask obstacleMask;

    private Vector3 playerPositionForEnemy = Vector3.zero;
    private Quaternion startPlayerRotation;

    private float enemyWaitTime;
    private float enemyTimeToRotate;
    private float enemyTimeToRotateToStartRotation;
    
    private bool enemyLookingOnPlayer = false;
    private bool playerInEnemyRange = false;
    private bool playerIsNearEnemy;
    private bool enemyIsPatrolingOnPoint = true;
    private bool enemyLookngOnStartPoint = true;


    private void Start()
    {
        startPlayerRotation = transform.rotation;
        enemyWaitTime = startWaitTime;
        enemyTimeToRotate = timeToRotate;
        enemyTimeToRotateToStartRotation = timeToRotateToStartRotation;
    }

    private void Update()
    {
        EnviromentView();

        if (enemyIsPatrolingOnPoint == false)
        {
            if (playerInEnemyRange)
            {
                if (enemyLookingOnPlayer == false)
                {
                    TurnToPlayerPosition();
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
        var targetLook = new Vector3(playerPositionForEnemy.x, transform.position.y, playerPositionForEnemy.z);
        
        transform.LookAt(targetLook);
    }

    private void PatrolingOnPoint()
    {
        if (playerIsNearEnemy)
        {
            if (enemyTimeToRotate <= 0)
            {
                PlayerWasDetectedByEnemy(playerPositionForEnemy);
            }
            else
            {
                enemyTimeToRotate -= Time.deltaTime;
            }
        }
        else
        {
            enemyWaitTime = startWaitTime;
            enemyTimeToRotate = timeToRotate;
        }

        if (!enemyLookngOnStartPoint)
        {
            if (enemyTimeToRotateToStartRotation <= 0)
            {
                TurnToStartPointOfView();
            }
            else
            {
                enemyTimeToRotateToStartRotation -= Time.deltaTime;
            }
        }
    }

    private void TurnToPlayerPosition()
    {
        Vector3 dirToPlayer = (playerPositionForEnemy - transform.position).normalized;
        
        if (Vector3.Angle(transform.forward, dirToPlayer) <= 5f)
        {
            enemyLookingOnPlayer = true;
        }
        else
        {
            enemyTimeToRotateToStartRotation = timeToRotateToStartRotation;
            enemyLookngOnStartPoint = false;
            
            var newRotation =
                Quaternion.LookRotation(playerPositionForEnemy - transform.position, Vector3.forward);

            newRotation = new Quaternion(0, newRotation.y, 0, newRotation.w);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation,
                speedOfRotation * Time.deltaTime);
        }
    }

    private void TurnToStartPointOfView()
    {
        if (transform.rotation != startPlayerRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, startPlayerRotation,
                speedOfRotation * 10 * Time.deltaTime);

        }
        else
        {
            enemyLookngOnStartPoint = true;
            enemyTimeToRotateToStartRotation = timeToRotateToStartRotation;
        }
    }

    private void PlayerWasDetectedByEnemy(Vector3 player)
    {
        if (enemyWaitTime <= 0)
        {
            enemyIsPatrolingOnPoint = false;
            playerInEnemyRange = true;
            
            enemyWaitTime = startWaitTime;
            enemyTimeToRotate = timeToRotate;
        }
        else
        {
            enemyWaitTime -= Time.deltaTime;
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
                    playerPositionForEnemy = player.position;
                    
                }
                else
                {
                    playerIsNearEnemy = false;
                }

                if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2 && Vector3.Distance(transform.position, player.position) <= viewRadius)
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
                playerPositionForEnemy = player.transform.position;
                
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