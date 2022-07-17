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
        GameObject wall = Resources.Load<GameObject>("Wall");
        _player = player.GetComponent<PlayerHealth>();
    }

    public override BossStates UpdateState()
    {
        return base.UpdateState();
    }
}

public class BossFight : MonoBehaviour
{
    private BossStates _states;

    private void Start()
    {
        _states = new WaitingPlayer(gameObject, FindObjectOfType<PlayerHealth>().transform);

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
}
