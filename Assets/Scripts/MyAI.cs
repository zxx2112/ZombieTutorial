using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MyAI : MonoBehaviour
{
    [SerializeField] private Transform[] nodeTransforms;
    [SerializeField] private UnitPath path;
    [SerializeField] private float step;

    NavMeshAgent agent;
    Animator anim;
    public Transform target;
    GameObject player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

        var nodes = new Vector3[nodeTransforms.Length];
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i] = nodeTransforms[i].position;
        }
        path.Init(nodes);
        
        if(path == null)
            StartCoroutine(SetTarget());
        else
        {
            StartCoroutine(FollowPath());
        }
    }

    private void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < 1f)
        {
            anim.SetBool("isAttackting",true);
            anim.speed = 2f;
        }
        else
        {
            anim.SetBool("isAttackting",false);
            anim.speed = agent.velocity.magnitude / 2f;
        }
    }

    IEnumerator SetTarget()
    {
        yield return new WaitForSeconds(1f);
        agent.SetDestination(target.position);
        StartCoroutine(SetTarget());
    }

    IEnumerator FollowPath()
    {
        yield return new WaitForSeconds(1f);

        var param = path.GetParam(transform.position);
        if (path.IsAtEndOfPath(transform.position, param, out var finalDestination))
        {
            path.Reverse();
        }
        else
        {
            param += step;
            agent.isStopped = false;
            var position = path.GetPosition(param);
            agent.SetDestination(position);
        }

        StartCoroutine(FollowPath());

    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        
        for (int i = 0; i < nodeTransforms.Length - 1; i++)
        {
            Gizmos.DrawLine(nodeTransforms[i].position,nodeTransforms[i+1].position);
        }
    }
}