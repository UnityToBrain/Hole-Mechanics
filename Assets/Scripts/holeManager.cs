using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class holeManager : MonoBehaviour
{
    public MeshFilter meshFilter; // The Mesh-filter of the circular plane that has a hole in the center 
    public MeshCollider meshCollider;
    private Mesh _mesh;

    private List<int> VerticesIndx = new List<int>(); //Stores the number of vertices that the circular plane has. for using the list in your project you need to import

    //System.Collections.Generic
    private readonly List<Vector3> offSet = new List<Vector3>(); // Stores the distance of each vertex that has the proper distance to the hole navigator object 

    public float standardDistance;
    public float holeSize = 1f; // determines hole size
    
    
    void Start()
    {
        _mesh = meshFilter.mesh;

        for (int i = 0; i < _mesh.vertices.Length; i++)
        {

            var distance = Vector3.Distance(transform.position, _mesh.vertices[i]);

            if (distance <= standardDistance)
            {
                VerticesIndx.Add(i);
                offSet.Add(_mesh.vertices[i] - transform.position);
                
            }

        }
    }

    void LateUpdate()
    {
        Vector3[] vertices = _mesh.vertices;

        for (int i = 0; i < VerticesIndx.Count; i++)
        {
            vertices[VerticesIndx[i]] = transform.position + offSet[i] * holeSize;
        }

        _mesh.vertices = vertices;

        meshFilter.mesh = _mesh;

        meshCollider.sharedMesh = _mesh;
    }
    
    
    // [SerializeField] private Slider slider;
    // private float value;
    // private void Start()
    // {
    //     GameObject[] Objects = GameObject.FindGameObjectsWithTag("block");
    //
    //     value = 100f / Objects.Length;
    //
    //     slider.maxValue = 100f;
    // }
    //
    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("block"))
    //     {
    //         other.gameObject.SetActive(false);
    //         slider.value += value;
    //     }
    // }
}
