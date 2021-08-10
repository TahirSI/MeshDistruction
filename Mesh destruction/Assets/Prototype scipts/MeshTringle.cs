using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTringle : MonoBehaviour
{
    private List<Vector3> vertasies = new List<Vector3>();
    private List<Vector3> normals = new List<Vector3>();
    private List<Vector2> uvs = new List<Vector2>();
    private int subMeshIndaies;

    public List<Vector3> Vertasies { get { return vertasies; } set { vertasies = value; } }
    public List<Vector3> Normals { get { return normals; } set { normals = value; } }
    public List<Vector2> Uvs { get { return uvs; } set { uvs = value; } }
    public int SubMeshIndex { get { return subMeshIndaies; } set { subMeshIndaies = value; } }


    public MeshTringle(Vector3[] _vertasies, Vector3[] _normals, Vector2[] _uvs, int _subMeshIndaies)
    {
        Clear();

        vertasies.AddRange(_vertasies);
        normals.AddRange(_normals);
        uvs.AddRange(_uvs);

        subMeshIndaies = _subMeshIndaies;
    }

    public void Clear()
    {
        vertasies.Clear();
        normals.Clear();
        uvs.Clear();

        subMeshIndaies = 0;
    }

}
