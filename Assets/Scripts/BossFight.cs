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
    protected GameObject _boss;
    protected Transform _playerTransform;

    protected BossState(GameObject boss, Transform player)
    {
        _boss = boss;
        _playerTransform = player;
    }

    public virtual BossStates UpdateState()
    {
        return this;
    }

    public virtual void ReachedDestination()
    {
        throw new System.NotImplementedException();
    }
}

public class WaitingPlayer : BossState
{
    public WaitingPlayer(GameObject boss, Transform player) : base(boss, player)
    {
    }

    public override BossStates UpdateState()
    {
        float distance = Vector2.Distance(_boss.transform.position, _playerTransform.position);
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

    public EatingPlayer(GameObject boss, Transform player) : base(boss, player)
    {
        _player = player.GetComponent<PlayerHealth>();
        _bossFight = boss.GetComponent<BossFight>();
    }

    public override BossStates UpdateState()
    {
        _bossFight.SetDestination(_playerTransform.position);

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

    public AcidAttacks(GameObject boss, Transform player) : base(boss, player)
    {
        boss.GetComponent<BossFight>()
            .SetDestination(boss.GetComponent<BossFight>().CenterPosition);
    }

    public override BossStates UpdateState()
    {
        return base.UpdateState();
    }

    public override void ReachedDestination()
    {
        _isInCenter = true;
    }
}

[RequireComponent(typeof(Seeker))]
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
        _states = new WaitingPlayer(gameObject, FindObjectOfType<PlayerHealth>().transform);
        _transform = GetComponent<Transform>();
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
        
        StopCoroutine(nameof(UpdatePosition));
    }

    private IEnumerator UpdatePosition()
    {
        print(_way[_way.Count - 1]);
        while (_path != null)
        {
            if (_nextWaypoint >= _way.Count - 1)
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
                if (distanceToNextWaypoint < 0.2f)
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
        StartCoroutine(UpdatePosition());
    }
}
