using UnityEngine;
using UnityEngine.AI;


public class enemy : MonoBehaviour
{
    NavMeshAgent agent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public int Health = 5;

/* Unmerged change from project 'Assembly-CSharp.enemy'
Before:
    public int maxHealth = 5;
After:
    private int maxHealth = 5;
*/
    private int maxHealth = 5;

    public int MaxHealth { get => maxHealth; set => maxHealth = value; }

    // Update is called once per frame
    void Update()
    {
        agent.destination = GameObject.Find("Player").transform.position;

    }
}
