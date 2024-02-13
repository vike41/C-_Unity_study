using PathCreation;
using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PathGenerator : MonoBehaviour
{
    public void Connect(Vector3 start, Vector3 end)
    {
        var nextStart = new Vector3(start.x, start.y, start.z + 1);
        var nextEnd = new Vector3(end.x, end.y, end.z + 1);
        var initPoints = new List<Vector3>() { start, nextStart, nextEnd, end };
        var points = new List<Vector3>();
        for (int i = 0; i < initPoints.Count; i++)
        {
            if(i + 1 >= initPoints.Count)
            {
                break;
            }

            if (i % 2 == 0)
            {
                points.Add(initPoints[i]);
                points.Add(GeneratePoints(initPoints[i], initPoints[i + 1], 1)[0]);
                points.Add(initPoints[i + 1]);
            }
            else
            {
                points.Add(GeneratePoints(initPoints[i], initPoints[i + 1], 1)[0]);
            }

        }

        GeneratePath(points.ToArray(), false);
        AdjustControllpoints();

        GetComponent<PipeCreator>().Init();
        GetComponent<BezierParticle>().CanRunParticleInit = true;
    }


    // Siehe https://github.com/SebLague/Path-Creator/issues/60
    private void AdjustControllpoints()
    {
        GetComponent<PathCreator>().bezierPath.ControlPointMode = BezierPath.ControlMode.Aligned;
        var numberpoints = GetComponent<PathCreator>().bezierPath.NumPoints;
        for (int i = 0; i < numberpoints; i += 6)
        {
            var cornerPointIndex = i;
            var nextStraightPointIndex = cornerPointIndex + 3;
            var cornerPointControllPointIndex = cornerPointIndex + 1;
            if (nextStraightPointIndex > numberpoints)
            {
                cornerPointIndex = i;
                nextStraightPointIndex = cornerPointIndex - 3;
                cornerPointControllPointIndex = cornerPointIndex - 1;
            }
            var cornerPoint = GetComponent<PathCreator>().bezierPath.GetPoint(cornerPointIndex);
            var straightPoint = GetComponent<PathCreator>().bezierPath.GetPoint(nextStraightPointIndex);
            var controllPoint = (straightPoint - cornerPoint).normalized * 0.2f + cornerPoint;
            GetComponent<PathCreator>().bezierPath.MovePoint(cornerPointControllPointIndex, controllPoint, true);
        }
    }

    private void GeneratePath(Vector3[] points, bool closedPath)
    {
        // Create a closed, 2D bezier path from the supplied points array
        // These points are treated as anchors, which the path will pass through
        // The control points for the path will be generated automatically
        BezierPath bezierPath = new BezierPath(points, closedPath, PathSpace.xyz);

        // Then create a vertex path from the bezier path, to be used for movement etc
        GetComponent<PathCreator>().bezierPath = bezierPath;
    }

    private List<Vector3> GeneratePoints(Vector3 from, Vector3 to, int amount)
    {
        if (amount == 0)
        {
            Debug.LogError("amount must be > 0 instead of " + amount);
            throw new Exception("amount must be > 0 instead of " + amount);
        }

        List<Vector3> verticies = new List<Vector3>();
        if (amount == 1)
        {
            verticies.Add(Vector3.Lerp(from, to, 0.5f));//Return half/middle point
        }
        else
        {
            float divider = 1f / amount;
            float linear = divider/2;
            for (int i = 0; i < amount; i++)
            {
                verticies.Add(Vector3.Lerp(from, to, linear));
                linear += divider;
            }
        }

        return verticies;
    }
}
