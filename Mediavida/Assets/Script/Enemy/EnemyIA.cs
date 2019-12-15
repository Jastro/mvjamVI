using UnityEngine;
using UnityEngine.SceneManagement;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class EnemyIA : MonoBehaviour {
    public float walkMovementSpeed = 1.0f;
    public float runMovementSpeed = 2.0f;

    public float visionAreaLength = 3f;
    public float visionAreaWide = 25.0f;

    public WaypointIA[] waypoints;

    private float _timeToWait = 0.0f;
    private int _nextWaypoint = 0;
    
    private Vector2 _lastSelfPosition = Vector2.zero;

    private bool _onPatrol = true;
    private bool _onChase = false;
    private bool _onReturnPatrol = false;

    public bool Vision = false;
    public float Distance = 0.0f;
    public float Anglo = 0.0f;
    
    /** Cached **/
    private GameObject _player;

    private Transform _transform;
    private Rigidbody2D _rigidbody;
    
    private void Start() {
        if (waypoints.Length == 0) {
            Destroy(gameObject);
            return;
        }
        
        _player = GameObject.FindGameObjectWithTag("Player");

        _transform = transform;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        OnActorMove();
    }

    private bool IsPlayerOnActorView() {
        Vector3 playerPosition = _player.transform.position;
        Vector3 selfPosition = _transform.position;
        
        Vector2 lookingAt = selfPosition + transform.right.normalized;
        
        Vector2 visionDirection = lookingAt - (Vector2)selfPosition;
        Vector2 playerDirection = (Vector2)playerPosition - (Vector2)selfPosition;
        
        float distance = Vector3.Distance(playerPosition, selfPosition);
        float angle = Vector3.Angle(visionDirection, playerDirection);
        Distance = distance;
        Anglo = angle;
        if (distance < visionAreaLength && angle < visionAreaWide) {
            RaycastHit2D playerRay = Physics2D.Raycast(selfPosition, playerDirection, distance);
            Debug.DrawRay(selfPosition, playerDirection, Color.magenta);
            Debug.Log(playerRay.collider);
            if (playerRay.collider.gameObject.CompareTag("Player")) {
                return true;
            }
        }

        return false;
    }

    private void DrawDebugActorView() {
        Vector3 selfPosition = _transform.position;

        var rotation = _transform.localEulerAngles.z;
        for (var i = 1.0f; i <= visionAreaWide; i += 2.5f) {
            var incrementalAngle = (rotation + i) * Mathf.Deg2Rad;
            var decrementalAngle = (rotation - i) * Mathf.Deg2Rad;
            
            var leftPoint = visionAreaLength * new Vector2(
                Mathf.Cos(incrementalAngle),
                Mathf.Sin(incrementalAngle)
            ) + (Vector2)selfPosition;
            
            var rightPoint = visionAreaLength * new Vector2(
                 Mathf.Cos(decrementalAngle),
                 Mathf.Sin(decrementalAngle)
             ) + (Vector2)selfPosition;
            
            Debug.DrawLine(selfPosition, leftPoint, Color.blue);
            Debug.DrawLine(selfPosition, rightPoint, Color.blue);
        }
    }

    private void OnActorView() {
        if (IsPlayerOnActorView()) {
            if (!_onChase && !_onReturnPatrol) {
                _lastSelfPosition = _transform.position;
            }

            Vision = true;

            _onPatrol = false;
            _onReturnPatrol = false;
            _onChase = true;
        }
        else if (_onChase) {
            Vision = false;
            
            _onChase = false;
            _onReturnPatrol = true;
        }
        
        DrawDebugActorView();
    }
    
    private void OnActorPatrol() {
        Vector3 selfPosition = _transform.position;
        
        if (_timeToWait > 0) {
            int lastWaypoint = _nextWaypoint - 1 < 0 ? waypoints.Length - 1 : _nextWaypoint - 1;
            WaypointIA waypoint = waypoints[lastWaypoint];
            
            if (waypoint.HasLookAtWaiting()) {
                RotateTowards(waypoint.lookAtWaiting.position);            
            }
            
            _timeToWait -= Time.deltaTime;
            return;
        }
        
        WaypointIA nextWaypoint = waypoints[_nextWaypoint];
        Vector3 nextWaypointPosition = nextWaypoint.transform.position;
        if (nextWaypoint.HasLookAtWalking()) {
            RotateTowards(nextWaypoint.lookAtWalking.position);            
        }

        float distance = Vector3.Distance(selfPosition, nextWaypointPosition);
        float movement = walkMovementSpeed * Time.deltaTime;

        if (movement >= distance) {
            movement = distance;
            _nextWaypoint++;
            _timeToWait = nextWaypoint.waitTime;

            if (_nextWaypoint >= waypoints.Length) {
                _nextWaypoint = 0;
            }
        }

        Vector3 finalPosition = Vector3.MoveTowards(selfPosition, nextWaypointPosition, movement);
        _rigidbody.MovePosition(finalPosition);
    }

    private void OnActorChase() {
        Vector3 playerPosition = _player.transform.position;
        Vector3 selfPosition = _transform.position;
        
        RotateTowards(playerPosition);

        float distance = Vector3.Distance(selfPosition, playerPosition);
        float movement = runMovementSpeed * Time.deltaTime;

        if (distance < 1) {
            SceneManager.LoadScene(4);
        }

        if (movement >= distance) {
            movement = distance;
        }

        Vector3 finalPosition = Vector3.MoveTowards(selfPosition, playerPosition, movement);
        _rigidbody.MovePosition(finalPosition);
    }

    private void OnActorReturnPatrol() {
        Vector3 selfPosition = _transform.position;
        
        RotateTowards(_lastSelfPosition);

        float distance = Vector3.Distance(selfPosition, _lastSelfPosition);
        float movement = walkMovementSpeed * Time.deltaTime;

        if (movement >= distance) {
            movement = distance;

            _onReturnPatrol = false;
            _onPatrol = true;

            _lastSelfPosition = Vector2.zero;
        }

        Vector3 finalPosition = Vector3.MoveTowards(selfPosition, _lastSelfPosition, movement);
        _rigidbody.MovePosition(finalPosition);
    }

    private void OnActorMove() {
        OnActorView();

        if (_onChase) {
            OnActorChase();
            return;
        }

//        if (_onSearch) {
//            
//        }

        if (_onReturnPatrol) {
            OnActorReturnPatrol();
            return;
        }

        if (_onPatrol) {
            OnActorPatrol();
            return;
        }

//        if (goingToLastLoc) {
//            Debug.Log("Checking last position from player");
//            RotateTowards(playerLastPos);
//
//            if(hit2.collider.gameObject.tag == "Player" || Vector3.Distance(transform.position, _player.transform.position) < 1.5f) {
//                followingPlayer = true;
//            }
//
//            if(Vector3.Distance(this.transform.position, playerLastPos) < distanceDetect) {
//                patrol = true;
//                goingToLastLoc = false;
//            }
//        }
    }

    private void RotateTowards(Vector2 target) {
        Vector2 direction = target - (Vector2)transform.position;
        direction.Normalize();
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
    }
}
