using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Pipe : MonoBehaviour
{
    public float pipeRadius;
    public float ringDistance;

    public int pipeSegmentCount;
    
    public float minCurveRadius;
    public float maxCurveRadius;
    public int minCurveSegmentCount;
    public int maxCurveSegmentCount;
    
    public PipeObstacleGenerator[] generators;

    public int CurveSegmentCount { get; private set; }

    public float CurveRadius { get; private set; }
    public float CurveAngle { get; private set; }

    public float RelativeRotation { get; private set; }

    private Mesh _mesh;
    private Vector3[] _vertices;
    private int[] _triangles;
    private Vector2[] _uv;

    private void Awake () {
        GetComponent<MeshFilter>().mesh = _mesh = new Mesh();
        _mesh.name = "Pipe";
    }

    public void Generate (bool withItems = true) {
        CurveRadius = Random.Range(minCurveRadius, maxCurveRadius);
        CurveSegmentCount =
            Random.Range(minCurveSegmentCount, maxCurveSegmentCount + 1);
        _mesh.Clear();
        SetVertices();
        SetUv();
        SetTriangles();
        _mesh.RecalculateNormals();
        for (var i = 0; i < transform.childCount; i++) {
            Destroy(transform.GetChild(i).gameObject);
        }
        if (withItems)
        {
            generators[Random.Range(0, generators.Length)].GenerateItems(this);
        }
    }

    private void SetUv () {
        _uv = new Vector2[_vertices.Length];
        for (var i = 0; i < _vertices.Length; i+= 4) {
            _uv[i] = Vector2.zero;
            _uv[i + 1] = Vector2.right;
            _uv[i + 2] = Vector2.up;
            _uv[i + 3] = Vector2.one;
        }
        _mesh.uv = _uv;
    }
    
    private void SetTriangles()
    {
        _triangles = new int[pipeSegmentCount * CurveSegmentCount * 6];
        for (int t = 0, i = 0; t < _triangles.Length; t += 6, i += 4) {
            _triangles[t] = i;
            _triangles[t + 1] = _triangles[t + 4] = i + 2;
            _triangles[t + 2] = _triangles[t + 3] = i + 1;
            _triangles[t + 5] = i + 3;
        }
        _mesh.triangles = _triangles;
    }

    private void SetVertices()
    {
        _vertices = new Vector3[pipeSegmentCount * CurveSegmentCount * 4];

        float uStep = ringDistance / CurveRadius;
        CurveAngle = uStep * CurveSegmentCount * (360f / (2f * Mathf.PI));
        CreateFirstQuadRing(uStep);
        int iDelta = pipeSegmentCount * 4;
        for (int u = 2, i = iDelta; u <= CurveSegmentCount; u++, i += iDelta) {
            CreateQuadRing(u * uStep, i);
        }
        _mesh.vertices = _vertices;
    }

    private void CreateFirstQuadRing (float u) 
    {
        var vStep = (2f * Mathf.PI) / pipeSegmentCount;

        var vertexA = GetPointOnTorus(0f, 0f);
        var vertexB = GetPointOnTorus(u, 0f);
        for (int v = 1, i = 0; v <= pipeSegmentCount; v++, i += 4) {
            _vertices[i] = vertexA;
            _vertices[i + 1] = vertexA = GetPointOnTorus(0f, v * vStep);
            _vertices[i + 2] = vertexB;
            _vertices[i + 3] = vertexB = GetPointOnTorus(u, v * vStep);
        }
    }

    private void CreateQuadRing (float u, int i) 
    {
        float vStep = (2f * Mathf.PI) / pipeSegmentCount;
        int ringOffset = pipeSegmentCount * 4;
		
        Vector3 vertex = GetPointOnTorus(u, 0f);
        for (int v = 1; v <= pipeSegmentCount; v++, i += 4) {
            _vertices[i] = _vertices[i - ringOffset + 2];
            _vertices[i + 1] = _vertices[i - ringOffset + 3];
            _vertices[i + 2] = vertex;
            _vertices[i + 3] = vertex = GetPointOnTorus(u, v * vStep);
        }
    }

    public void AlignWith (Pipe pipe) {
        RelativeRotation =
            Random.Range(0, CurveSegmentCount) * 360f / pipeSegmentCount;
		
        transform.SetParent(pipe.transform, false);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0f, 0f, -pipe.CurveAngle);
        transform.Translate(0f, pipe.CurveRadius, 0f);
        transform.Rotate(RelativeRotation, 0f, 0f);
        transform.Translate(0f, -CurveRadius, 0f);
        transform.SetParent(pipe.transform.parent);
        transform.localScale = Vector3.one;
    }
    
    /**
     * Returns a point on the surface of the torus
     * @param u defines the angle along the curve in radians (rang: 0-2pi)
     * @param v defines the angle along the pipe
     */
    private Vector3 GetPointOnTorus(float u, float v)
    {
        Vector3 p;
        var r = CurveRadius + pipeRadius * Mathf.Cos(v);
        p.x = r * Mathf.Sin(u);
        p.y = r * Mathf.Cos(u);
        p.z = pipeRadius * Mathf.Sin(v);
        return p;
    }
}
