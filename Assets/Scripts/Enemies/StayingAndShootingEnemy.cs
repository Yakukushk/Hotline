using System.Collections;
using UnityEngine;

public class StayingAndShootingEnemy : MonoBehaviour
{
    [SerializeField] private float timeBeforeRotateOnPlayer = 1f;
    [SerializeField] private float speedOfRotation = 20f;
    [SerializeField] private float timeToRotateToStartRotation = 10f;
    
    [SerializeField] private float viewRadius = 15f;
    [SerializeField] private float viewAngle = 90f;
    
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask obstacleMask;

    private Vector3 playerPositionForEnemy = Vector3.zero;
    private Quaternion startPlayerRotation;

    private bool playerDetectedByEnemyCoroutioneWasStarted;
    private bool turnToStartRotationCorutineWasStarted;
    private bool enemyLookingOnPlayer;
    private bool playerInEnemyRange;
    private bool playerIsNearEnemy;
    private bool enemyIsPatrolingOnPoint = true;
    private bool enemyLookngOnStartPoint = true;

    private Coroutine _coroutineForRotateToStartRotation;
    private Coroutine _coroutineForDetectingPlayer;

    private void Start()
    {
        startPlayerRotation = transform.rotation;
    }


    private void Update()
    {
        EnviromentView();

        if (enemyIsPatrolingOnPoint == false)
        {
            if (playerInEnemyRange)
            {
                turnToStartRotationCorutineWasStarted = false;
                StopCoroutine(_coroutineForRotateToStartRotation);
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
            if (!playerDetectedByEnemyCoroutioneWasStarted)
            {
                playerDetectedByEnemyCoroutioneWasStarted = true;
                
                _coroutineForDetectingPlayer = StartCoroutine(CoroutinePlayerWasDetectedByEnemy());
            }
        }

        if (!turnToStartRotationCorutineWasStarted)
        {
            turnToStartRotationCorutineWasStarted = true;
            
            _coroutineForRotateToStartRotation = StartCoroutine(WaitCoroutine());
        }
    }

    private IEnumerator WaitCoroutine()
    {
        yield return new WaitForSecondsRealtime(timeToRotateToStartRotation);

        while (true)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, startPlayerRotation,
                speedOfRotation * 10 * Time.deltaTime);
            yield return new WaitForSecondsRealtime(Time.deltaTime);
            if (transform.rotation == startPlayerRotation)
            {
                yield break;
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
            enemyLookngOnStartPoint = false;

            var newRotation =
                Quaternion.LookRotation(playerPositionForEnemy - transform.position, Vector3.forward);

            newRotation = new Quaternion(0, newRotation.y, 0, newRotation.w);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation,
                speedOfRotation * Time.deltaTime);
        }
    }

    private IEnumerator CoroutinePlayerWasDetectedByEnemy()
    {
        yield return new WaitForSecondsRealtime(timeBeforeRotateOnPlayer);

        enemyIsPatrolingOnPoint = false;
        playerInEnemyRange = true;
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
                    StopCoroutine(_coroutineForDetectingPlayer);
                    playerDetectedByEnemyCoroutioneWasStarted = false;
                    playerIsNearEnemy = false;
                }

                if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2 &&
                    Vector3.Distance(transform.position, player.position) <= viewRadius)
                {
                    playerInEnemyRange = true;
                    enemyIsPatrolingOnPoint = false;
                }
            }
            else
            {
                StopCoroutine(_coroutineForDetectingPlayer);
                playerDetectedByEnemyCoroutioneWasStarted = false;
                playerIsNearEnemy = false;
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