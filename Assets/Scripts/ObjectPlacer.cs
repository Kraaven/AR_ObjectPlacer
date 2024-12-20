using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    private MeshFilter[] Objects;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Objects = Resources.LoadAll<MeshFilter>("Cubes");
        Debug.Log($"Number of Objects {Objects.Length}");
    }

    public void PlaceObject(Mesh TargetArea, Vector3 SpawnLocation)
    {
        if (Objects == null || Objects.Length == 0)
        {
            Debug.LogError("No objects loaded to place!");
            return;
        }

        // Get the bounds of the target area
        Bounds targetBounds = TargetArea.bounds;
        Vector3 targetSize = targetBounds.size;

        // Variables to track the best matching object
        MeshFilter bestMatch = null;
        float smallestDifference = float.MaxValue;

        // Compare each object's bounds with the target area
        foreach (MeshFilter obj in Objects)
        {
            Bounds objBounds = obj.sharedMesh.bounds;

            // Transform the bounds to world space
            Vector3 objSize = Vector3.Scale(objBounds.size, obj.transform.lossyScale);

            // Check if object is smaller than target area in all dimensions
            if (objSize.x <= targetSize.x && 
                objSize.z <= targetSize.z)
            {
                // Calculate the difference in size
                float difference = Vector3.Distance(targetSize, objSize);

                Debug.Log($"Object {obj.gameObject.name} has difference {difference}");
                // Update best match if this object is closer in size
                if (difference < smallestDifference)
                {
                    smallestDifference = difference;
                    bestMatch = obj;
                    Debug.Log($"Object {bestMatch.gameObject.name} is the smallest so far.");
                }
            }
        }

        // Check if we found a valid match
        if (bestMatch == null)
        {
            Debug.LogWarning("No objects found that fit within the target area!");
            //Instantiate(Objects[Random.Range(0, Objects.Length)].gameObject, SpawnLocation, Quaternion.identity);
            return;
        }

        // Instantiate the best matching object
        Instantiate(bestMatch.gameObject, SpawnLocation, Quaternion.Euler(-90,0,0));
    }
}
