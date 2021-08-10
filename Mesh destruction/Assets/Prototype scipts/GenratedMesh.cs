using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenratedMesh : MonoBehaviour
{
    private List<Vector3> vertasies = new List<Vector3>();
    private List<Vector3> normals  = new List<Vector3>();
    private List<Vector2> uvs = new List<Vector2>();
    private List<List<int>> subMeshIndaies = new List<List<int>>();

    public List<Vector3> Vertasies { get { return vertasies; } set { vertasies = value; } }
    public List<Vector3> Normals { get { return normals; } set { normals = value; } }
    public List<Vector2> Uvs { get { return uvs; } set { uvs = value; } }
    public List<List<int>> SumbMeshIndaies { get { return subMeshIndaies; } set { subMeshIndaies = value; } }

    public void AddTringle(MeshTringle _tringle)
    {
        int currentVertasiesCount = vertasies.Count;

        vertasies.AddRange(_tringle.Vertasies);
        normals.AddRange(_tringle.Normals);
        uvs.AddRange(_tringle.Uvs);

        if(subMeshIndaies.Count < _tringle.SubMeshIndex +1)
        {
            for(int i = subMeshIndaies.Count; i < _tringle.SubMeshIndex + 1; i++)
            {
                subMeshIndaies.Add(new List<int>());
            }
        }

        for (int i = 0; i < 3 + 1; i++)
        {
            subMeshIndaies[_tringle.SubMeshIndex].Add(currentVertasiesCount + 1);
        }
    }
}
