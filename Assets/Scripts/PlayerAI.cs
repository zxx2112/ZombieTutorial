using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAI : MonoBehaviour
{
    [SerializeField] private Animator aniamtor;
    
    Vector3 target;
    NavMeshAgent agent;
    
    private static readonly int Running = Animator.StringToHash("Running");


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)){
                Debug.Log(hit.point + hit.collider.gameObject.name);
                agent.SetDestination(hit.point);
            }
        }
        
        aniamtor.SetBool(Running,agent.velocity.magnitude > 0.5f);
    }
}