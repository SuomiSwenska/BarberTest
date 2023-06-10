using UnityEngine;

public class Cutter : MonoBehaviour
{
    private HaircutSystem _haircutSystem;
    public Mesh _mesh;

    public Vector3[] Vertices;
    public bool IsMustDestroy;

    private void Awake()
    {
        _haircutSystem = FindObjectOfType<HaircutSystem>();
        _mesh = GetComponent<MeshFilter>().mesh;
        Vertices = GetComponent<MeshFilter>().mesh.vertices;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<ScissorsBlade>() != null)
        {
            ContactPoint contactPoint = collision.contacts[0];
            _haircutSystem.BladeContact(contactPoint.point, this);
        }
    }

    public void RedrawCuttingMesh(Vector3[] vertices)
    {
        ChangeMesh(vertices);
    }

    private void ChangeMesh(Vector3[] vertices)
    {
        Vertices = vertices;
        _mesh.vertices = vertices;
        GetComponent<MeshCollider>().sharedMesh = _mesh;
        _mesh.RecalculateNormals();
    }
}
