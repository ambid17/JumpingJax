using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfRampMeshGen : MonoBehaviour
{
    public Material material;
    public int rampLeftWidth = 8;
    public int rampRightWidth = 8;
    public int rampHeight = 8;


    public List<Vector3> rampTopPoints;
    public GameObject builtRamp;

    private MeshFilter myFilter;
    private MeshRenderer myRenderer;
    private MeshCollider myCollider;


    public void AddRampPoint()
    {
        rampTopPoints.Add(Vector3.zero);
    }

    public void BuildObject()
    {
        if (builtRamp == null)
        {
            InitializeRampObject();
        }

        if (rampTopPoints.Count < 2)
        {
            myFilter.sharedMesh = new Mesh();
            return;
        }

        if(material != null)
        {
            myRenderer.material = material;
        }

        Vector3[] vertices = GenerateVertices();
        int[] indices = GenerateIndices();
        Color[] colors = GenerateColors(vertices.Length);
        Vector3[] normals = GenerateNormals();


        Mesh newMesh = new Mesh();
        newMesh.vertices = vertices;
        newMesh.triangles = indices;
        newMesh.colors = colors;
        newMesh.normals = normals;
        newMesh.name = "generated mesh";

        myFilter.sharedMesh = newMesh;
        myCollider.sharedMesh = null;
        myCollider.sharedMesh = newMesh;
    }

    private void InitializeRampObject()
    {
        builtRamp = new GameObject();
        builtRamp.name = "Generated Mesh";
        builtRamp.AddComponent<MeshRenderer>();
        myRenderer = builtRamp.GetComponent<MeshRenderer>();

        builtRamp.AddComponent<MeshFilter>();
        myFilter = builtRamp.GetComponent<MeshFilter>();

        builtRamp.AddComponent<MeshCollider>();
        myCollider = builtRamp.GetComponent<MeshCollider>();
    }

    private Vector3[] GenerateVertices()
    {
        List<Vector3> vertices = new List<Vector3>();

        for (int i = 0; i < rampTopPoints.Count; i++)
        {
            Vector3 top = rampTopPoints[i];
            Vector3 left = rampTopPoints[i];
            left.z = rampLeftWidth;
            left.y -= rampHeight;
            Vector3 right = rampTopPoints[i];
            right.z = -rampRightWidth;
            right.y -= rampHeight;

            vertices.Add(top);
            vertices.Add(left);
            vertices.Add(right);
        }

        return vertices.ToArray();
    }

    private int[] GenerateIndices()
    {
        List<int> indices = new List<int>();

        int currentBaseIndex = 0;

        //Create front face
        indices.Add(currentBaseIndex);
        indices.Add(currentBaseIndex + 2);
        indices.Add(currentBaseIndex + 1);

        int numberOfRampSections = rampTopPoints.Count;

        for (int i = 1; i < numberOfRampSections; i++)
        {
            List<int> leftFace = new List<int>()
            {
                currentBaseIndex, currentBaseIndex + 3, currentBaseIndex + 2,
                currentBaseIndex + 3, currentBaseIndex + 5, currentBaseIndex + 2
            };

            List<int> rightFace = new List<int>()
            {
                currentBaseIndex, currentBaseIndex + 1, currentBaseIndex + 3,
                currentBaseIndex + 3, currentBaseIndex + 1, currentBaseIndex + 4
            };

            List<int> bottomFace = new List<int>()
            {
                currentBaseIndex + 1, currentBaseIndex + 2, currentBaseIndex + 4,
                currentBaseIndex + 4, currentBaseIndex + 2, currentBaseIndex + 5
            };

            indices.AddRange(leftFace);
            indices.AddRange(rightFace);
            indices.AddRange(bottomFace);

            currentBaseIndex += 3;
        }

        //Create back face
        indices.Add(currentBaseIndex + 0);
        indices.Add(currentBaseIndex + 1);
        indices.Add(currentBaseIndex + 2);

        return indices.ToArray();
    }

    private Color[] GenerateColors(int numColors)
    {
        Color[] colors = new Color[numColors];

        for(int i = 0; i < numColors; i++)
        {
            
            colors[i] = new Color(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1), 1);
        }

        return colors;
    }

    private Vector3[] GenerateNormals()
    {
        List<Vector3> normals = new List<Vector3>();

        for (int i = 0; i < rampTopPoints.Count; i++)
        {
            normals.Add(new Vector3(0, 1, 0));
            normals.Add(new Vector3(0, 1, 0));
            normals.Add(new Vector3(0, 1, 0));
        }

        return normals.ToArray();
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < rampTopPoints.Count; i++)
        {
            // Draw each vertex
            Gizmos.color = new Color(1, 1, 1, 1);
            Gizmos.DrawCube(rampTopPoints[i], new Vector3(0.3f, 0.3f, 0.3f));

            // Connect with lines
            if (i > 0)
            {
                Gizmos.DrawLine(rampTopPoints[i], rampTopPoints[i - 1]);
            }
        }
    }
}
