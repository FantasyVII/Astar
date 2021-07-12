using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AstarAgent))]
public class Enemy : MonoBehaviour
{
    [SerializeField] Vector3Int startNode;
    [SerializeField] Vector3Int endNode;
    [SerializeField] float speed;

    AstarAgent agent;
    List<AStarNode> path;

    int i;
    bool arrived;

    void Start()
    {
        agent = GetComponent<AstarAgent>();
        path = agent?.FindPath(startNode, endNode);

        i = 0;
        arrived = false;
    }

    void Update()
    {
        if(path.Count > 0 && !arrived)
        {
            Vector3 direction = (path[i].Position - transform.position).normalized;

            transform.position += direction * speed * Time.deltaTime;
            float distance = Vector3.Distance(transform.position, path[i].Position);

            if (distance <= 0.1f)
            {
                i++;

                if (i >= path.Count)
                    arrived = true;
            }
        }
    }
}