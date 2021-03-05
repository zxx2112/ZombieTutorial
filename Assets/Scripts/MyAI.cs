using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MyAI : MonoBehaviour
{
    NavMeshAgent agent;
    Animator anim;
    public Transform target;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(SetTarget());
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
}