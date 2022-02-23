using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    [SerializeField]
    private LineRenderer alertLine;
    [SerializeField]
    private LineRenderer warningLine;
    [SerializeField]
    private MeshRenderer mesh;
    [SerializeField]
    private GameObject @object;
    [SerializeField]
    private LayerMask mask;

    private float warningRange = 7f;
    private float alertRange = 3f ;

    private float alertAngle;
    private float warningAngle;

    private List<EnemySight> enemies = new List<EnemySight>();

    private Coroutine Alert;
    private Coroutine Warning;

    public bool Test;

    private void Awake()
    {
        foreach(EnemySight enemy in @object.GetComponentsInChildren<EnemySight>())
        {
            enemies.Add(enemy);
        }
    }

    private void Update()
    {
        warningLine.SetPosition(0, transform.position);
        warningLine.SetPosition(1, transform.position + transform.TransformDirection(new Vector3(0.5f, 0, 1f)) * warningRange);
        warningLine.SetPosition(2, transform.position);
        warningLine.SetPosition(3, transform.position + transform.TransformDirection(new Vector3(-0.5f, 0, 1f)) * warningRange);

        alertLine.SetPosition(0, transform.position);
        alertLine.SetPosition(1, transform.position + transform.TransformDirection(new Vector3(0.5f, -0.1f, 1f)) * alertRange);
        alertLine.SetPosition(2, transform.position);
        alertLine.SetPosition(3, transform.position + transform.TransformDirection(new Vector3(-0.5f, -0.1f, 1f)) * alertRange);

        alertAngle = MeasureAngle(alertLine.GetPosition(1),
            alertLine.GetPosition(3));
        warningAngle = MeasureAngle(warningLine.GetPosition(1),
            warningLine.GetPosition(3));

        if (SeePlayer(warningAngle, warningRange) || Test)
        {
            if (SeePlayer(alertAngle, alertRange) || Test)
            {
                AlertState();
            }
            else if (mesh.material.color != Color.red)
            {
                WarningState();
            }
        }
    }

    private float MeasureAngle(Vector3 target, Vector3 target1)
    {
        Vector3 A = target - transform.position;
        Vector3 B = target1 - transform.position;
        float angle = Vector3.Angle(B,A);
        return angle;
    }

    public void WarningState()
    {
        Test = false;
        mesh.material.color = Color.yellow;
        enemies.Remove(this);

        if(Warning != null)
        {
            this.StopCoroutine(Warning);
        }

        Warning = this.StartCoroutine(WarningCor(10f));
    }

    IEnumerator WarningCor(float second)
    {
        yield return new WaitForSeconds(second);
        mesh.material.color = Color.white;
    }

    public void AlertState()
    {
        if (!enemies.Contains(this))
        {
            enemies.Add(this);
        }

        mesh.material.color = Color.red;

        if(Alert != null)
        {
            this.StopCoroutine(Alert);
        }
        
        Alert = this.StartCoroutine(AlertCor());

        foreach(var item in GetClosestEnemy(enemies.ToArray()))
        {
            item.Test = true;
        }

    }

    List<EnemySight> GetClosestEnemy(EnemySight[] enemies)
    {
        List<EnemySight> bestTarget = new List<EnemySight>();
        for (int i = 0; i < enemies.Length; i++)
        {
            Vector3 directionToTarget = enemies[i].transform.position - transform.position;
            float dToTarget = directionToTarget.magnitude;
            if (dToTarget < 10f)
            {
                bestTarget.Add(enemies[i]);
            }
        }

        return bestTarget;
    }

    IEnumerator AlertCor()
    {
        if (!SeePlayer(alertAngle, alertRange))
        {
            yield return new WaitForSeconds(3f);
            WarningState();
        }
    }

    private bool SeePlayer(float angle, float range)
    {
        if(Vector3.Distance(Player.instance.transform.position, transform.position) < range)
        {
            Vector3 dir = (Player.instance.transform.position - transform.position);
            float angleP = Vector3.Angle(transform.forward, dir);
            if(angleP < angle /2)
            {
                if(!Physics.Linecast(transform.position, Player.instance.transform.position, mask))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
