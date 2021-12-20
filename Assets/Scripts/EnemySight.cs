using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    public bool spread;
    [SerializeField]
    private Player player;
    [SerializeField]
    private Transform SightFarR, SightFarL, SightR, SightL;
    [SerializeField]
    private float viewFar, ViewNear;
    [SerializeField]
    LayerMask mask;
    public Material yellow, red, def;
    private float angleSightF, angleSightN;
    private MeshRenderer mesh;
    bool seeFar;
    bool seeNear;

    [SerializeField]
    private EnemySight[] enemies;
    private void Awake()
    {
        enemies = FindObjectsOfType<EnemySight>();
    }

    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        def = mesh.material;
        player = FindObjectOfType<Player>();
        angleSightF = Vector3.Angle(SightFarR.position, SightFarL.position);
        angleSightN = Vector3.Angle(SightR.position, SightL.position);
    }


    private void Update()
    {
        seeFar = SeePlayer(angleSightF, viewFar) && !SeePlayer(angleSightN, ViewNear);
        seeNear = SeePlayer(angleSightN, ViewNear);
        if (seeFar && !spread) mesh.material = yellow;
        else if (seeNear || spread) { Spread(); mesh.material = red;}
        else {mesh.material = def; spread = false; }   
    }

    void Spread()
    {
        foreach(EnemySight enemy in GetClosestEnemy(enemies))
        {
            enemy.spread = true;
        }
    }

    private bool SeePlayer(float angleSight, float viewDistance)
    {
        if(Vector3.Distance(player.transform.position, transform.position) < viewDistance)
        {
            Vector3 dir = (player.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, dir);
            if(angle < angleSight / 2)
            {
                if(!Physics.Linecast(transform.position, player.transform.position, mask))
                {
                    return true;
                }
            }
        }
        return false;
    }

    List<EnemySight> GetClosestEnemy(EnemySight[] enemies)
    {
        List<EnemySight> bestTarget = new List<EnemySight>();
        for (int i = 0; i < enemies.Length; i++)
        {
            Vector3 directionToTarget = enemies[i].transform.position - transform.position;
            float dToTarget = directionToTarget.magnitude;
            if(dToTarget < 10f)
            {
                bestTarget.Add(enemies[i]);
            }
        }

        return bestTarget;
    }
}
