using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPolling : MonoBehaviour
{
    public int poolObject;
    public GameObject poolPrefabs;

    List<GameObject> objects = new List<GameObject>();
    List<GameObject> test = new List<GameObject>();
    float RespawnTime = 5f;

    private void Awake()
    {
        for(int i = 0; i < poolObject; i++)
        {
            GameObject game = Instantiate(poolPrefabs, transform.position, transform.rotation, transform);
            game.SetActive(false);
            objects.Add(game);
        }
    }

    private void Update()
    {
        GameObject game = Spawn(transform.position, transform.rotation);
        if (game) test.Add(game);

        float times = 0;
        times += Time.deltaTime;

        if(times>= RespawnTime)
        {
            times = 0;
            game = test[0];
            Return(game);
            test.Remove(game);
        }
    }

    void Return(GameObject obj)
    {
        if (!objects.Contains(obj))
        {
            obj.transform.position = transform.position;
            obj.transform.rotation = transform.rotation;
            obj.SetActive(false);
            objects.Add(obj);
        }
    }

    GameObject Request()
    {
        GameObject obj = null;
        if(objects.Count > 0)
        {
            obj = objects[0];
            objects.Remove(obj);
        }
        return obj;
    }

    GameObject Spawn(Vector3 pos, Quaternion rot)
    {
        GameObject obj = Request();
        if (obj)
        {
            obj.transform.position = pos;
            obj.transform.rotation = rot;
            obj.SetActive(true);
        }
        return obj;
    }
}
