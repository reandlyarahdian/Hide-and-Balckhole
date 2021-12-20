using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    public PolygonCollider2D hole2D;
    public PolygonCollider2D ground2D;
    public MeshCollider meshCollider;
    public float initScale;
    public float speed;
    public Mesh generateMesh;

    void FixedUpdate()
    {
        Vector3 movePos = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        transform.Translate(movePos * speed * Time.deltaTime);

        if(transform.hasChanged == true)
        {
            transform.hasChanged = false;
            hole2D.transform.position = new Vector3(transform.position.x, transform.position.z);
            hole2D.transform.localScale = transform.localScale * initScale;
            MakeHole2D();
            MakeMesh();
        }
    }

    void MakeHole2D()
    {
        Vector2[] pointPos = hole2D.GetPath(0);

        for (int i = 0; i < pointPos.Length; i++)
        {
            pointPos[i] = hole2D.transform.TransformPoint(pointPos[i]);
        }

        ground2D.pathCount = 2;
        ground2D.SetPath(1, pointPos);
    }

    void MakeMesh()
    {
        if (generateMesh != null) Destroy(generateMesh);
        generateMesh = ground2D.CreateMesh(true, true);
        meshCollider.sharedMesh = generateMesh;
    }
}
