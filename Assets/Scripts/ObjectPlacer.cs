using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    private FurnitureObject[] Objects;
    [SerializeField] private InteractionManager _interactionManager;
    private bool AllowShuffle;
    private Vector3 FocusSpawnPoint;
    private int OBJ_index;
    private GameObject Current;

    private List<GameObject> OrderedList;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Objects = Resources.LoadAll<FurnitureObject>("Furniture");
        Debug.Log($"Number of Objects {Objects.Length}");
        OrderedList = new List<GameObject>();
    }

    public void PlaceObject(Mesh TargetArea, Vector3 SpawnPoint)
    {
        if (Objects == null || Objects.Length == 0)
        {
            //Debug.LogError("No objects loaded to place!");
            return;
        }

        // Get the bounds of the target area
        Bounds targetBounds = TargetArea.bounds;
        Vector3 targetSize = targetBounds.size;

        // Create a list to store objects that can fit along with their size differences
        List<(FurnitureObject obj, float difference)> fittingObjects = new List<(FurnitureObject, float)>();

        // Compare each object's bounds with the target area
        foreach (FurnitureObject obj in Objects)
        {
            // Get the bounds of the object
            Vector3 objSize = obj.GetBounds().size;

            //Vector3 objSize = Vector3.Scale(objBounds.size, obj.transform.localScale);
            // Transform the bounds to world space
            //Vector3 objSize = Vector3.Scale(objBounds.size, obj.transform.lossyScale);

            // Check if the object is smaller than the target area in all dimensions
            if (objSize.x <= targetSize.x && objSize.z <= targetSize.z)
            {
                // Calculate the difference in size
                float difference = Vector3.Distance(targetSize, objSize);

                // Add the object and its difference to the list
                fittingObjects.Add((obj, difference));
                // Debug.Log($"Object : {obj.name} Has Difference : {difference}\n" +
                //           $"Object : {objSize}, Reference : {targetBounds.size}");
            }
        }

        // Check if there are any fitting objects
        if (fittingObjects.Count == 0)
        {
            Debug.LogWarning("No objects found that fit within the target area!");
            OrderedList.Clear(); // Clear the list if no objects fit
            return;
        }

        // Sort the fitting objects by their size differences (ascending)
        fittingObjects.Sort((a, b) => a.difference.CompareTo(b.difference));

        // Populate the OrderedList with the first 5 best-fitting objects
        OrderedList.Clear();
        for (int i = 0; i < Mathf.Min(5, fittingObjects.Count); i++)
        {
            OrderedList.Add(fittingObjects[i].obj.gameObject);
            //Debug.Log($"Added {fittingObjects[i].obj.gameObject.name} to OrderedList with difference {fittingObjects[i].difference}");
        }

        if(OrderedList.Count == 0) return;
        
        Debug.Log($"Number of Available Objects : {OrderedList.Count}");
        FocusSpawnPoint = SpawnPoint;
        OBJ_index = 0;
        Current = Instantiate(OrderedList[OBJ_index], FocusSpawnPoint, Quaternion.identity);
    }

    public void ShuffleNextObject()
    {
        Destroy(Current.gameObject);
        OBJ_index++;
        if (OBJ_index >= OrderedList.Count) OBJ_index = 0;
        Current = Instantiate(OrderedList[OBJ_index], FocusSpawnPoint, Quaternion.identity);
    }

    public void ResetNewObject()
    {
        Current = null;
        AllowShuffle = false;
    }
}
