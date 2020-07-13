using System.Collections.Generic;
using UnityEngine;

public abstract class Activatable : BlockingObject
{
    protected bool playerCanActivate;
    public Activatable connectedTo;    

    public abstract void activate(GameObject activator);
    public abstract void setStatus(GameObject activator, bool value);


    // Creates a line renderer that follows a Sin() function
    // and animates it.

    public Color c1 = Color.yellow;
    public Color c2 = Color.red;
    private int lengthOfLineRenderer = 2;
    public Vector3 origin;
    public Transform end;
    public Vector3 offSetVector;
    [Min(0.01f)]
    public float width;

    void Start()
    {
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.widthMultiplier = width;
        lineRenderer.positionCount = lengthOfLineRenderer;

        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        lineRenderer.colorGradient = gradient;
    }

    void Update()
    {
        if (connectedTo)
        {
            origin = transform.position;
            end = connectedTo.gameObject.transform;
            LineRenderer lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.widthMultiplier = width;          
            Vector3[] path = calculatePath();
            lineRenderer.positionCount = path.Length;
            lineRenderer.SetPositions(path);
        }
    }

    private Vector3[] calculatePath()
    {
        int xDistance = Mathf.CeilToInt(Mathf.Abs(origin.x - end.position.x));
        int yDistance = Mathf.CeilToInt(Mathf.Abs(origin.y - end.position.y));
        List<Vector3> path = new List<Vector3>();

        if (origin.x < end.position.x)
        {
            for (int x = Mathf.CeilToInt(origin.x); x <= end.position.x; x++)
            {
                path.Add(new Vector3(x, origin.y) + offSetVector);
            }
        }
        else
        {
            for (int x = Mathf.CeilToInt(origin.x); x >= end.position.x; x--)
            {
                path.Add(new Vector3(x, origin.y) + offSetVector);
            }
        }
        if (origin.y < end.position.y)
        {
            for (int y = Mathf.CeilToInt(origin.y); y <= end.position.y; y++)
            {
                path.Add(new Vector3(end.position.x, y) + offSetVector);
            }
        }
        else
        {
            for (int y = Mathf.CeilToInt(origin.y); y >= end.position.y; y--)
            {
                path.Add(new Vector3(end.position.x, y) + offSetVector);
            }
        }

        return path.ToArray();

    }
}



