using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class MyAI : MonoBehaviour
{
    public enum ZombieState
    {
        Patrol,
        Chase,
        Attack
    }
    
    
    [SerializeField] private LinePath path;
    
    NavMeshAgent agent;
    Animator anim;
    public Transform target;
    GameObject player;

    private ZombieState _zombieState = ZombieState.Patrol;
    private Stopwatch _pathFindCooldown;
    private Stopwatch _attackTimer;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

        _pathFindCooldown = new Stopwatch();
        _pathFindCooldown.Start();

        _attackTimer = new Stopwatch();
    }

    private void Update()
    {
        if (_zombieState == ZombieState.Attack)
        {
            anim.SetBool("isAttackting",true);
        }
        else
        {
            anim.SetBool("isAttackting",false);
        }
    }

    private void FixedUpdate()
    {
        switch (_zombieState)
        {
            case ZombieState.Patrol:
                if (_pathFindCooldown.Elapsed.Seconds > 1)
                {
                    FollowPath();
                    _pathFindCooldown.Restart();
                }

                if (Vector3.Distance(player.transform.position, transform.position) < 5f)
                {
                    _zombieState = ZombieState.Chase;
                    _pathFindCooldown.Restart();
                }
                
                break;
            case ZombieState.Chase:

                if (_pathFindCooldown.Elapsed.Seconds > 1)
                {
                    SetTarget();
                    _pathFindCooldown.Restart();
                }
                
                if (Vector3.Distance(player.transform.position, transform.position) < 1f)
                {
                    _zombieState = ZombieState.Attack;
                    _attackTimer.Start();
                }
                
                break;
            case ZombieState.Attack:

                if (_attackTimer.Elapsed.Seconds > 3)
                {
                    _zombieState = ZombieState.Chase;
                    _pathFindCooldown.Restart();
                }
                
                break;
        }
    }


    private void SetTarget()
    {
        agent.SetDestination(target.position);
    } 

    private void FollowPath()
    {
        var param = path.GetParam(transform.position);
        if (path.IsAtEndOfPath(transform.position, param, out var finalDestination))
        {
            agent.isStopped = true;
        }
        else
        {
            param += 1;
            agent.isStopped = false;
            var position = path.GetPosition(param);
            agent.SetDestination(position);
        }
    }
}