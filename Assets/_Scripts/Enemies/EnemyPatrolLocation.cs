using UnityEngine;

public class EnemyPatrolLocation : MonoBehaviour
{
    [SerializeField] private Transform _path;

    private Transform[] _wayPoints;
    private int _currentPoint;
    private Enemy _enemy;

    private void Awake() => _enemy = GetComponent<Enemy>();
    private void Start() => GetPatrolPoints();

    public void MoveToPatrolPoints(float speed)
    {
        Transform target = _wayPoints[_currentPoint];

        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > _wayPoints[_currentPoint].position.x && _enemy.IsFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            _enemy.IsFlipped = false;
        }
        else if (transform.position.x < _wayPoints[_currentPoint].position.x && !_enemy.IsFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            _enemy.IsFlipped = true;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (transform.position == target.position)
        {
            _currentPoint++;

            if (_currentPoint >= _wayPoints.Length)
                _currentPoint = 0;
        }
    }

    private void GetPatrolPoints()
    {
        _wayPoints = new Transform[_path.childCount];
        for (int i = 0; i < _path.childCount; i++)
        {
            _wayPoints[i] = _path.GetChild(i);
        }
    }
}


