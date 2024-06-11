using UnityEngine;
using UnityEngine.AI;
using static UnityEditorInternal.ReorderableList;
using Random = UnityEngine.Random;

public class HumanStateMachine : MonoBehaviour
{
    [Header("NavMesh Components")]
    public NavMeshAgent _HumanAgent;

    [Header("Global Values")]
    public Animator HumanAnimator;
    public GameObject _TrackedHuman;
    public GameObject _FoundHuman;
    public GameObject humanDetectionArea;
    public GameObject infectedDetectionArea;
    public LayerMask _WallMask;
    public float _WalkingRange = 20f;
    public bool _IsWalking;
    public Vector3 _LastPosition;

    public float _HaltedSpeed = 1f;

    [Header("Human State")]
    public bool _IsHuman;
    public bool _IsInfected;
    
    [Header("Alive States")]
    public bool _IsAfraid;
    public bool _IsCaught;

    [Header("Infected States")]
    public bool _IsChasing;
    public bool InfectedOnCooldown = false;

    HumanBaseState currentState;
    HumanStateFactory states;

    public HumanBaseState CurrentState { get { return currentState;} set { currentState = value; } }

    private void Awake()
    {
        _IsHuman = true;
        _HumanAgent = GetComponent<NavMeshAgent>();
        states = new HumanStateFactory(this);
        currentState = states.Idle();
        currentState.EnterState();
    }

    private void Start()
    {
    }

    private void Update()
    {
        Debug.Log("Stopped: "+ _HumanAgent.isStopped);
        if (_IsInfected&&humanDetectionArea.activeSelf)
        {
            humanDetectionArea.SetActive(false);
            infectedDetectionArea.SetActive(true);
            _HumanAgent.speed = 3f*_HaltedSpeed;
        }
        currentState.UpdateStates();
    }

    public Vector3 RandomNavSphere(Vector3 origin, float dist)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        randDirection.y = 0.5f;
        NavMeshHit navHit;
        if (NavMesh.SamplePosition(randDirection, out navHit, dist, 1))
        {
            return navHit.position;
        }
        else
        {
            return Vector3.zero;
        }
    }

    public bool pathComplete()
    {
        if (Vector3.Distance(_HumanAgent.destination, _HumanAgent.transform.position) <= _HumanAgent.stoppingDistance)
        {
            if (!_HumanAgent.hasPath || _HumanAgent.velocity.sqrMagnitude == 0f)
            {
                return true;
            }
        }
        if (Vector3.Distance(_HumanAgent.destination, _HumanAgent.transform.position) <= _HumanAgent.stoppingDistance*2)
        {
            AddDestination();
        }
        return false;
    }

    public void NewDestination(bool repeat, float range)
    {
        Vector3 targetPosition = RandomNavSphere(transform.position, range);
        if (targetPosition == Vector3.zero)
        {
            repeat = true;
        }
        else
        {
            _HumanAgent.SetDestination(targetPosition);
            _IsWalking = true;
        }
    }

    public void AddDestination()
    {
        Vector3 targetPosition = RandomNavSphere(transform.position, _WalkingRange);
        if (targetPosition == Vector3.zero)
        {
        }
        else
        {
            _HumanAgent.nextPosition = targetPosition;
            _IsWalking = true;
        }
    }

    public void CheckIfWalking()
    {
        if (_LastPosition == gameObject.transform.position)
        {
            _IsWalking = false;
        }
        _LastPosition = gameObject.transform.position;
    }

}
