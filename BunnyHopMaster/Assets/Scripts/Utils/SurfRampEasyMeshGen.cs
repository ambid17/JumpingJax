using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfRampEasyMeshGen : MonoBehaviour
{
    public Material material;
    public int rampLeftWidth = 8;
    public int rampRightWidth = 8;
    public int rampHeight = 8;


    public Vector3 startPoint;
    public Vector3 endPoint;
    public Vector3 controlPoint;

    [Range(1,100)]
    public int numberOfSections = 1;

    public GameObject builtRamp;

    private MeshFilter myFilter;
    private MeshRenderer myRenderer;

    public void BuildObject()
    {
        if (builtRamp == null)
        {
            InitializeRampObject();
        }

        if (material != null)
        {
            myRenderer.material = material;
        }

        Vector3[] vertices = GenerateVertices();
        int[] indices = GenerateIndices();
        Color[] colors = GenerateColors(vertices.Length);
        Vector2[] uvs = GenerateUVs(vertices.Length);
        Vector3[] normals = GenerateNormals();


        Mesh newMesh = new Mesh();
        newMesh.vertices = vertices;
        newMesh.triangles = indices;
        newMesh.colors = colors;
        newMesh.uv = uvs;
        newMesh.normals = normals;
        newMesh.name = "generated mesh";
        //newMesh.RecalculateNormals();

        myFilter.sharedMesh = newMesh;
    }

    private void InitializeRampObject()
    {
        builtRamp = new GameObject();
        builtRamp.name = "Easy Generated Mesh";
        builtRamp.AddComponent<MeshRenderer>();
        builtRamp.AddComponent<MeshFilter>();
        builtRamp.AddComponent<MeshCollider>();
        myFilter = builtRamp.GetComponent<MeshFilter>();
        myRenderer = builtRamp.GetComponent<MeshRenderer>();
    }

    private Vector3[] GenerateVertices()
    {
        List<Vector3> vertices = new List<Vector3>();

        for (int i = 0; i <= numberOfSections; i++)
        {
            Vector3 currentPoint = CalculateQuadraticBezierPoint(i, startPoint, endPoint, controlPoint);
            Vector3 left = currentPoint;
            Vector3 right = currentPoint;

            if (i == 0)
            {
                Vector3 nextPoint = CalculateQuadraticBezierPoint(i + 1, startPoint, endPoint, controlPoint);

                float angle = Mathf.Atan2((nextPoint.z - currentPoint.z), (nextPoint.x - currentPoint.x));
                float sin = Mathf.Sin(angle);
                float cos = Mathf.Cos(angle);

                if (float.IsNaN(angle))
                {
                    angle = 0;
                }

                if (angle < 0)
                {
                    angle += 360;
                }

                left.x -= sin * rampLeftWidth;
                left.z += cos * rampLeftWidth;

                right.x += sin * rampRightWidth;
                right.z -= cos * rampRightWidth;

                left.y -= rampHeight;
                right.y -= rampHeight;
            } else
            {
                Vector3 previousPoint = CalculateQuadraticBezierPoint(i - 1, startPoint, endPoint, controlPoint);

                float angle = Mathf.Atan2((currentPoint.z - previousPoint.z), (currentPoint.x - previousPoint.x));
                float sin = Mathf.Sin(angle);
                float cos = Mathf.Cos(angle);

                if (float.IsNaN(angle))
                {
                    angle = 0;
                }

                if (angle < 0)
                {
                    angle += 360;
                }

                left.x -= Mathf.Sin(angle) * rampLeftWidth;
                left.z += Mathf.Cos(angle) * rampLeftWidth;

                right.x += Mathf.Sin(angle) * rampRightWidth;
                right.z -= Mathf.Cos(angle) * rampRightWidth;

                left.y -= rampHeight;
                right.y -= rampHeight;
            }
            

            vertices.Add(currentPoint);
            vertices.Add(left);
            vertices.Add(right);
        }

        return vertices.ToArray();
    }

    private Vector3 CalculateQuadraticBezierPoint(int currentStep, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float t = (float)currentStep / numberOfSections;
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;
        return p;
    }

    private int[] GenerateIndices()
    {
        List<int> indices = new List<int>();

        int currentBaseIndex = 0;

        //Create front face
        indices.Add(currentBaseIndex);
        indices.Add(currentBaseIndex + 2);
        indices.Add(currentBaseIndex + 1);


        for (int i = 0; i < numberOfSections; i++)
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

        for (int i = 0; i < numColors; i++)
        {

            colors[i] = new Color(Random.Range(0, 1), Random.Range(0, 1), Random.Range(0, 1), 1);
        }

        return colors;
    }

    public Vector2[] GenerateUVs(int numUVs)
    {
        Vector2[] uvs = new Vector2[numUVs];

        for (int i = 0; i < numUVs; i++)
        {

            uvs[i] = new Vector2(Random.Range(0, 1), Random.Range(0, 1));
        }

        return uvs;
    }

    private Vector3[] GenerateNormals()
    {
        List<Vector3> normals = new List<Vector3>();

        for (int i = 0; i <= numberOfSections; i++)
        {
            normals.Add(new Vector3(0, 1, 0));
            normals.Add(new Vector3(1, 1, 0));
            normals.Add(new Vector3(0, 1, 1));
        }

        return normals.ToArray();
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i <= numberOfSections; i++)
        {
            Gizmos.color = new Color(1, 0, 1, 1);
            Gizmos.DrawCube(controlPoint, new Vector3(0.5f, 0.5f, 0.5f));

            Vector3 currentPoint = CalculateQuadraticBezierPoint(i, startPoint, endPoint, controlPoint);
            // Draw each vertex
            Gizmos.color = new Color(1, 1, 1, 1);
            Gizmos.DrawCube(currentPoint, new Vector3(0.3f, 0.3f, 0.3f));


            // Connect with lines
            if (i > 0)
            {
                Vector3 previousPoint = CalculateQuadraticBezierPoint(i - 1, startPoint, endPoint, controlPoint);

                Gizmos.DrawLine(previousPoint, currentPoint);
            }
        }
    }
}
