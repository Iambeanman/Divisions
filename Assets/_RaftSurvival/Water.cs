using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Water : MonoBehaviour
{

    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    public List<Vector3> vertexes = new List<Vector3>();

    public float wavesSpeed;

    public float waterHeight;

    public float amplitude = 1f;
    public float length = 2f;
    public float speed = 1f;
    public float offset = 0;

    public static Water get;

    private void Awake()
    {
        get = this;
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        wavesSpeed = meshRenderer.material.GetFloat("_WavesSpeed");
    }

    void Start()
    {
        vertexes = meshFilter.mesh.vertices.ToList();
    }

    // Update is called once per frame
    void Update()
    {
        //offset += Time.deltaTime * speed;

        //waterHeight = GetWaterHeight(Player.get.transform.position.x);
        //GenerateWaves();
    }

    void GenerateWaves()
    {
        Vector3[] vertices = meshFilter.mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i].y = GetWaterHeight(transform.position.x + vertices[i].x);
        }

        meshFilter.mesh.vertices = vertices;
        meshFilter.mesh.RecalculateNormals();
    }

    public float GetWaterHeight(float x)
    {
        return amplitude * Mathf.Sin(x / length + offset);
    }
}
