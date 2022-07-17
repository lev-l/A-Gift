using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BossStates
{
    public BossStates UpdateState();
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
}

public class WaitingPlayer : BossState
{
    public WaitingPlayer(GameObject boss, Transform player) : base(boss, player)
    {
    }

    public override BossStates UpdateState()
    {
        float distance = Vector2.Distance(_boss.transform.position, _playerTransform.position);
        if (distance < 11)
        {
            GameObject wall = Resources.Load<GameObject>("Wall");
            MonoBehaviour.Instantiate(wall);
            return new EatingPlayer(_boss, _playerTransform);
        }
        return base.UpdateState();
    }
}

public class EatingPlayer : BossState
{
    private PlayerHealth _player;

    public EatingPlayer(GameObject boss, Transform player) : base(boss, player)
    {
        _player = player.GetComponent<PlayerHealth>();
        _boss.GetComponent<BossFight>().SetDestination(_playerTransform.position);
    }

    public override BossStates UpdateState()
    {
        return base.UpdateState();
    }
}

[RequireComponent(typeof(Seeker))]
public class BossFight : MonoBehaviour
{
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
            if (_nextWaypoint >= _way.Count - 1)
            {
                _path = null;
                _way.Clear();
                _nextWaypoint = 0;
            }
            else
            {
                print(_way[_nextWaypoint]);
                _transform.Translate((_way[_nextWaypoint] - _transform.position).normalized * 5 * Time.deltaTime);
                if (Vector2.Distance(_transform.position, _way[_nextWaypoint]) < 0.2f)
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
        _way = _path.vectorPath;
        StartCoroutine(UpdatePosition());
    }
}
