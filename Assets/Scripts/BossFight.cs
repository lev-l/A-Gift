using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BossStates
{
    public BossStates UpdateState();

    public void ReachedDestination();
}

public abstract class BossState : BossStates
{
    protected Transform _boss;
    protected Transform _playerTransform;
    protected RotateToTarget _rotations;
    private Transform _bossSprite;

    protected BossState(Transform boss, Transform player)
    {
        _boss = boss;
        _playerTransform = player;
        _rotations = boss.GetComponent<RotateToTarget>();
        _bossSprite = _boss.GetChild(0);
    }

    public virtual BossStates UpdateState()
    {
        return this;
    }

    public virtual void ReachedDestination()
    {
        throw new System.NotImplementedException();
    }

    protected Quaternion GetRotationToPlayer()
    {
        return Quaternion.Euler(Vector3.forward * _rotations.Rotate(_boss.position, _playerTransform.position));
    }

    protected void LookToPlayer()
    {
        _bossSprite.rotation = Quaternion.Euler(Vector3.forward * _rotations.Rotate(_boss.position, _playerTransform.position));
    }
}

public class WaitingPlayer : BossState
{
    public WaitingPlayer(Transform boss, Transform player) : base(boss, player)
    {
    }

    public override BossStates UpdateState()
    {
        float distance = Vector2.Distance(_boss.position, _playerTransform.position);
        if (distance < 13)
        {
            GameObject wall = Resources.Load<GameObject>("Wall");
            MonoBehaviour.Instantiate(wall);
            return new EatingPlayer(_boss, _playerTransform);
        }
        return base.UpdateState();
    }

    public override void ReachedDestination()
    {
    }
}

public class EatingPlayer : BossState
{
    private PlayerHealth _player;
    private BossFight _bossFight;
    private bool _reachedPlayer;

    public EatingPlayer(Transform boss, Transform player) : base(boss, player)
    {
        _player = player.GetComponent<PlayerHealth>();
        _bossFight = boss.GetComponent<BossFight>();
    }

    public override BossStates UpdateState()
    {
        _bossFight.SetDestination(_playerTransform.position);
        LookToPlayer();

        if (_reachedPlayer)
        {
            return new AcidAttacks(_boss, _playerTransform);
        }
        return base.UpdateState();
    }

    public override void ReachedDestination()
    {
        _player.Hurt(10);
        _reachedPlayer = true;
    }
}

public class AcidAttacks : BossState
{
    private bool _isInCenter;
    private GameObject _acidSplash;
    private int _timeToBlast = 20;
    private int _lastTime;
    private float _timeToTheNextState = 15;

    public AcidAttacks(Transform boss, Transform player) : base(boss, player)
    {
        boss.GetComponent<BossFight>()
            .SetDestination(boss.GetComponent<BossFight>().CenterPosition);
        _acidSplash = Resources.Load<GameObject>("AcidSplash");

        _lastTime = _timeToBlast;
    }

    public override BossStates UpdateState()
    {
        if (_isInCenter)
        {
            _lastTime--;
            LookToPlayer();
            
            if(_lastTime == 0)
            {
                GameObject newAcidSplash = Object.Instantiate(_acidSplash, _boss);
                newAcidSplash.transform.SetParent(null);
                newAcidSplash.transform.rotation = GetRotationToPlayer();
                _lastTime = _timeToBlast;
            }

            _timeToTheNextState -= Time.deltaTime;
            if(_timeToTheNextState <= 0)
            {
                return new EatingPlayer(_boss, _playerTransform);
            }
        }
        return base.UpdateState();
    }

    public override void ReachedDestination()
    {
        _isInCenter = true;
    }
}

[RequireComponent(typeof(Seeker), typeof(RotateToTarget))]
public class BossFight : MonoBehaviour
{
    public float Speed;
    public Vector2 CenterPosition;

    private BossStates _states;
    private Transform _transform;
    private Seeker _seeker;
    private Path _path;
    private List<Vector3> _way;
    private int _nextWaypoint;

    private void Start()
    {
        _transform = GetComponent<Transform>();
        _states = new WaitingPlayer(_transform, FindObjectOfType<PlayerHealth>().transform);
        _seeker = GetComponent<Seeker>();
        _way = new List<Vector3>();

        CenterPosition = _transform.position;
        _seeker.pathCallback = new OnPathDelegate(PathCalculated);
        StartCoroutine(StatesChanging());
    }

    private IEnumerator StatesChanging()
    {
        while (true)
        {
            _states = _states.UpdateState();
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void SetDestination(Vector2 target)
    {
        _seeker.StartPath(start: _transform.position,
                            end: target);
    }

    private IEnumerator UpdatePosition()
    {
        while (_path != null)
        {
            if (_nextWaypoint >= _way.Count)
            {
                _path = null;
                _way.Clear();
                _nextWaypoint = 0;

                _states.ReachedDestination();
            }
            else
            {
                _transform.Translate((_way[_nextWaypoint] - _transform.position).normalized * Speed * Time.deltaTime);

                float distanceToNextWaypoint = Vector2.Distance(_transform.position, _way[_nextWaypoint]);
                if (distanceToNextWaypoint < 1)
                {
                    _nextWaypoint++;
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private void PathCalculated(Path path)
    {
        _path = path;
        _way.Clear();
        _way = _path.vectorPath;
        _nextWaypoint = 0;

        StopCoroutine(nameof(UpdatePosition));
        StartCoroutine(nameof(UpdatePosition));
    }
}
