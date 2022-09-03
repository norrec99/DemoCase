using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour
{
    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Move(Vector3 target)
    {
        agent.SetDestination(target);
    }

    public void SetAgent()
    {
        agent = GetComponent<NavMeshAgent>();
    }
}
