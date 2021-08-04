using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutter
{
    public static bool currentlyCutting;
    public static Mesh orignalMesh;

    public static void Cut(GameObject _orignalGameObject, Vector3 _contactPoint, Vector3 _direction,
        Material _cutMaterial = null, bool _fill = true, bool _addRidedBody = false)
    {
        if (currentlyCutting)
            return;

        currentlyCutting = true;

        Plane plane = new Plane(_orignalGameObject.transform.InverseTransformDirection(-_direction),
            _orignalGameObject.transform.InverseTransformPoint(_contactPoint));

        orignalMesh = _orignalGameObject.GetComponent<MeshFilter>().mesh;
        List<Vector3> addVertaices = new List<Vector3>();


        GenratedMesh leftMesh = new GenratedMesh();
        GenratedMesh rightMesh = new GenratedMesh();


        int[] SubMeshIndaies;
        int triangleIndexA, triangleIndexB, triangleIndexC;


        for (int i = 0; i < orignalMesh.subMeshCount; i++)
        {
            SubMeshIndaies = orignalMesh.GetTriangles(i);

            for (int j = 0; j < SubMeshIndaies.Length; j++)
            {
                triangleIndexA = SubMeshIndaies[j];
                triangleIndexB = SubMeshIndaies[j + 1];
                triangleIndexC = SubMeshIndaies[j + 2];

                MeshTringle currentTringle = GetTriangle(triangleIndexA, triangleIndexB, triangleIndexC, i);

                bool trianglALeLeftSide = plane.GetSide(orignalMesh.vertices[triangleIndexA]);
                bool trianglBLeLeftSide = plane.GetSide(orignalMesh.vertices[triangleIndexB]);
                bool trianglCLeLeftSide = plane.GetSide(orignalMesh.vertices[triangleIndexC]);

                if (trianglALeLeftSide && trianglBLeLeftSide && trianglCLeLeftSide)
                {
                    leftMesh.AddTringle(currentTringle);
                }
                else if (!trianglALeLeftSide && !trianglBLeLeftSide && !trianglCLeLeftSide)
                {
                    rightMesh.AddTringle(currentTringle);

                }
                else
                {
                    CutTriangle(plane, currentTringle, trianglALeLeftSide, trianglBLeLeftSide, trianglCLeLeftSide, leftMesh, rightMesh, addVertaices);
                }

            }
        }


    }

    internal static void Cut(GameObject ob, Ray ray, bool v)
    {
        throw new NotImplementedException();
    }

    private static MeshTringle GetTriangle(int triangleIndexA, int triangleIndexB, int triangleIndexC, int submeshIndex)
    {
        {
            Vector3[] vertices = {
             orignalMesh.vertices[triangleIndexA],
             orignalMesh.vertices[triangleIndexB],
             orignalMesh.vertices[triangleIndexC],
        };


            Vector3[] normals = {
             orignalMesh.normals[triangleIndexA],
             orignalMesh.normals[triangleIndexB],
             orignalMesh.normals[triangleIndexC],
        };


            Vector2[] uvs = {
             orignalMesh.uv[triangleIndexA],
             orignalMesh.uv[triangleIndexB],
             orignalMesh.uv[triangleIndexC],
        };


            MeshTringle meshTringle = new MeshTringle(vertices, normals, uvs, submeshIndex);
            return meshTringle;
        }
    }


    private static void CutTriangle(Plane _plan, MeshTringle _triangle, bool _triangleIndexA, bool _triangleIndexB, bool _triangleIndexC,
        GenratedMesh _lefSide, GenratedMesh _rightSide, List<Vector3> _addVertacies)
    {
        List<bool> leftide = new List<bool>();
        leftide.Add(_triangleIndexA);
        leftide.Add(_triangleIndexB);
        leftide.Add(_triangleIndexC);

        MeshTringle leftMeshTringle = new MeshTringle(new Vector3[2], new Vector3[2], new Vector2[2], _triangle.SubMeshIndex);
        MeshTringle rightMeshTringle = new MeshTringle(new Vector3[2], new Vector3[2], new Vector2[2], _triangle.SubMeshIndex);

        bool left = false;
        bool right = false;

        for (int i = 0; i < 3; i++)
        {
            if (leftide[i])
            {
                if (!left)
                {
                    left = true;

                    leftMeshTringle.Vertasies[0] = _triangle.Vertasies[i];
                    leftMeshTringle.Vertasies[1] = leftMeshTringle.Vertasies[0];

                    leftMeshTringle.Normals[0] = _triangle.Normals[i];
                    leftMeshTringle.Normals[1] = leftMeshTringle.Normals[0];

                    leftMeshTringle.Uvs[0] = _triangle.Uvs[i];
                    leftMeshTringle.Uvs[1] = leftMeshTringle.Uvs[0];
                }
                else
                {
                    leftMeshTringle.Vertasies[1] = _triangle.Vertasies[i];
                    leftMeshTringle.Normals[1] = _triangle.Normals[i];
                    leftMeshTringle.Uvs[1] = _triangle.Uvs[i];

                }
            }
            else
            {
                if (!right)
                {
                    right = true;

                    rightMeshTringle.Vertasies[0] = _triangle.Vertasies[i];
                    rightMeshTringle.Vertasies[1] = leftMeshTringle.Vertasies[0];

                    rightMeshTringle.Normals[0] = _triangle.Normals[i];
                    rightMeshTringle.Normals[1] = leftMeshTringle.Normals[0];

                    rightMeshTringle.Uvs[0] = _triangle.Uvs[i];
                    rightMeshTringle.Uvs[1] = leftMeshTringle.Uvs[0];
                }
                else
                {
                    rightMeshTringle.Vertasies[1] = _triangle.Vertasies[i];
                    rightMeshTringle.Normals[1] = _triangle.Normals[i];
                    rightMeshTringle.Uvs[1] = _triangle.Uvs[i];

                }
            }
        }

        float normlisedDirection;
        float distences;

        _plan.Raycast(new Ray(leftMeshTringle.Vertasies[0], (rightMeshTringle.Vertasies[0] - leftMeshTringle.Vertasies[0]).normalized), out distences);

        normlisedDirection = distences / (rightMeshTringle.Vertasies[0] - leftMeshTringle.Vertasies[0]).magnitude;
        Vector3 vertextLength = Vector3.Lerp(leftMeshTringle.Vertasies[0], rightMeshTringle.Vertasies[0], normlisedDirection);
        _addVertacies.Add(vertextLength);

        Vector3 normalLeft = Vector3.Lerp(leftMeshTringle.Normals[0], rightMeshTringle.Normals[0], normlisedDirection);
        Vector2 uvleft = Vector2.Lerp(leftMeshTringle.Uvs[0], rightMeshTringle.Uvs[0], normlisedDirection);


        _plan.Raycast(new Ray(leftMeshTringle.Vertasies[1], (rightMeshTringle.Vertasies[1] - leftMeshTringle.Vertasies[1]).normalized), out distences);

        normlisedDirection = distences / (rightMeshTringle.Vertasies[1] - leftMeshTringle.Vertasies[1]).magnitude;
        Vector3 vertextRight = Vector3.Lerp(leftMeshTringle.Vertasies[1], rightMeshTringle.Vertasies[1], normlisedDirection);
        _addVertacies.Add(vertextRight);

        Vector3 normalRight = Vector3.Lerp(leftMeshTringle.Normals[1], rightMeshTringle.Normals[0], normlisedDirection);
        Vector2 uvRight = Vector2.Lerp(leftMeshTringle.Uvs[1], rightMeshTringle.Uvs[1], normlisedDirection);


        // Left side of the triangles

        MeshTringle currentTriangle;
        Vector3[] updateVertaices = new Vector3[] { leftMeshTringle.Vertasies[0], vertextLength, vertextRight };
        Vector3[] updateNormasl = new Vector3[] { leftMeshTringle.Normals[0], normalLeft, normalRight };
        Vector2[] updateUvs = new Vector2[] { leftMeshTringle.Uvs[0], uvleft, uvRight };

        currentTriangle = new MeshTringle(updateVertaices, updateNormasl, updateUvs, _triangle.SubMeshIndex);


        if (updateVertaices[0] != updateVertaices[1] && updateVertaices[0] != updateVertaices[2])
        {
            if (Vector3.Dot(Vector3.Cross(updateVertaices[1] - updateVertaices[0], updateVertaices[2] - updateVertaices[0]), updateNormasl[0]) < 0)
            {
                FlipTriangle(currentTriangle);
            }

            _lefSide.AddTringle(currentTriangle);
        }

        // Right side of triangles

        updateVertaices = new Vector3[] { leftMeshTringle.Vertasies[0], leftMeshTringle.Vertasies[1], vertextRight };
        updateNormasl = new Vector3[] { leftMeshTringle.Normals[0], leftMeshTringle.Normals[1], normalRight };
        updateUvs = new Vector2[] { leftMeshTringle.Uvs[0], leftMeshTringle.Uvs[1], uvRight };

        currentTriangle = new MeshTringle(updateVertaices, updateNormasl, updateUvs, _triangle.SubMeshIndex);

        if (updateVertaices[0] != updateVertaices[1] && updateVertaices[0] != updateVertaices[2])
        {
            if (Vector3.Dot(Vector3.Cross(updateVertaices[1] - updateVertaices[0], updateVertaices[2] - updateVertaices[0]), updateNormasl[0]) < 0)
            {
                FlipTriangle(currentTriangle);
            }

            _rightSide.AddTringle(currentTriangle);
        }
    }

    private static void FlipTriangle(MeshTringle _triangle)
    {
        Vector3 lastVertex = _triangle.Vertasies[_triangle.Vertasies.Count - 1];
        _triangle.Vertasies[_triangle.Vertasies.Count - 1] = _triangle.Vertasies[0];
        _triangle.Vertasies[0] = lastVertex;

        Vector3 lastNormal = _triangle.Normals[_triangle.Normals.Count - 1];
        _triangle.Normals[_triangle.Normals.Count - 1] = lastNormal;
        _triangle.Normals[0] = lastNormal;

        Vector2 lastUV = _triangle.Uvs[_triangle.Uvs.Count - 1];
        _triangle.Uvs[_triangle.Uvs.Count - 1] = _triangle.Uvs[0];
        _triangle.Uvs[0] = lastUV;
    }


    public static void FillCut(List<Vector3> _addedVertaices, Plane _plane, GenratedMesh _leftMesh, GenratedMesh _rightMesh)
    {
        List<Vector3> vertacies = new List<Vector3>();
        List<Vector3> polygone = new List<Vector3>();

        for (int i = 0; i < _addedVertaices.Count; i++)
        {
            if (!vertacies.Contains(_addedVertaices[i]))
            {
                polygone.Clear();
                polygone.Add(_addedVertaices[i]);
                polygone.Add(_addedVertaices[i + 1]);

                vertacies.Add(_addedVertaices[i]);
                vertacies.Add(_addedVertaices[i + 1]);

                EvaluatePerais(_addedVertaices, vertacies, polygone);
                Fill(polygone, _plane, _leftMesh, _rightMesh);
            }
        }
    }

    public static void EvaluatePerais(List<Vector3> _addedVertaices, List<Vector3> _vertaices, List<Vector3> _polygone)
    {
        bool isDone = false;

        while (!isDone)
        {
            isDone = true;

            for (int i = 0; i < _addedVertaices.Count; i += 2)
            {
                if (_addedVertaices[i] == _polygone[_polygone.Count - 1] && !_vertaices.Contains(_addedVertaices[i + 1]))
                {
                    isDone = false;
                    _polygone.Add(_addedVertaices[i + 1]);
                    _vertaices.Add(_addedVertaices[i + 1]);
                }
                else if (_addedVertaices[i + 1] == _polygone[_polygone.Count - 1] && !_vertaices.Contains(_addedVertaices[i]))
                {
                    isDone = false;
                    _polygone.Add(_addedVertaices[i]);
                    _vertaices.Add(_addedVertaices[i]);
                }
            }
        }
    }

    public static void Fill(List<Vector3> _vertcis, Plane _plane, GenratedMesh _leftMesh, GenratedMesh _rightMesh)
    {
        // Calculate to get the center
        Vector3 centerPoint = Vector3.zero;
        for (int i = 0; i < _vertcis.Count; i++)
        {
            centerPoint += _vertcis[i];
        }

        // upwords the axies to that we can use plave to cut the mesh with
        centerPoint = centerPoint / _vertcis.Count;
        Vector3 up = new Vector3()
        {
            x = _plane.normal.x,
            y = _plane.normal.y,
            z = _plane.normal.z
        };

        Vector3 left = Vector3.Cross(_plane.normal, _plane.normal);

        Vector3 displacement = Vector3.zero;
        Vector2 uv1 = Vector2.zero;
        Vector2 uv2 = Vector2.zero;

        for (int i = 0; i < _vertcis.Count; i++)
        {
            displacement = _vertcis[i] - centerPoint;
            uv1 = new Vector2()
            {
                x = .5f + Vector3.Dot(displacement, left),
                y = .5f + Vector3.Dot(displacement, up)
            };

            displacement = _vertcis[(i + 1) % _vertcis.Count] - centerPoint;
            uv2 = new Vector2()
            {
                x = .5f + Vector3.Dot(displacement, left),
                y = .5f + Vector3.Dot(displacement, up)
            };

            Vector3[] vertaices = new Vector3[] { _vertcis[i], _vertcis[(i + 1) % _vertcis.Count], centerPoint };
            Vector3[] normals = new Vector3[] { _plane.normal, -_plane.normal, -_plane.normal };
            Vector2[] uvs = new Vector2[] { uv1, uv2, new Vector3(0.5f, 0.5f) };

            MeshTringle currentTinage = new MeshTringle(vertaices, normals, uvs, orignalMesh.subMeshCount + 1);

            if (Vector3.Dot(Vector3.Cross(vertaices[1] - vertaices[0], vertaices[2] - vertaices[0]), normals[0]) < 0)
            {
                FlipTriangle(currentTinage);
            }
            _leftMesh.AddTringle(currentTinage);

            normals = new Vector3[] { _plane.normal, _plane.normal };
            currentTinage = new MeshTringle(vertaices, normals, uvs, orignalMesh.subMeshCount + 1);

            if (Vector3.Dot(Vector3.Cross(vertaices[1] - vertaices[0], vertaices[2] - vertaices[0]), normals[0]) < 0)
            {
                FlipTriangle(currentTinage);
            }
            _rightMesh.AddTringle(currentTinage);
        }
    }
}
