using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    
    [SerializeField] private Transform ARCAM;
    [SerializeField] private TMP_Text debugArea;
    [SerializeField] private ObjectPlacer objectPlacer;
    [SerializeField] private LayerMask _AR_layerMask;
    [Header("Selection References")]
    [SerializeField] private Transform BlueTarget;
    [SerializeField] private Transform OrangeTarget;
    [SerializeField] private GameObject ShuffleButton;

    [Header("Object Customization")] 
    public GameObject CustomisationMenu;

    public MeshFilter SelectionMeshObject;
    
    //[SerializeField] private GameObject SelectionPoints;

    private Mesh SelectionMesh;
    

    private bool _InSelection;

    private void Start()
    {
        OrangeTarget.gameObject.SetActive(false);
        BlueTarget.gameObject.SetActive(true);
        SelectionMeshObject.gameObject.SetActive(false);
        ShuffleButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(ARCAM.position, ARCAM.forward, out RaycastHit hit, _AR_layerMask))
        {
            if (_InSelection)
            {
                OrangeTarget.position = hit.point;
            }
            else
            {
                BlueTarget.position = hit.point;
            }
        }
    }


    public void Select()
    {
        if (_InSelection)
        {
            _InSelection = false;
            BlueTarget.gameObject.SetActive(true);
            OrangeTarget.gameObject.SetActive(false);
            
            float length = Mathf.Abs(BlueTarget.position.x - OrangeTarget.position.x);
            float width = Mathf.Abs(BlueTarget.position.z - OrangeTarget.position.z);
            
            Vector3 centerPoint = (BlueTarget.position + OrangeTarget.position) / 2f;
            
            // Create vertices relative to center (0,0,0)
            Vector3[] vertices = new Vector3[]
            {
                new Vector3(-length/2, 0, -width/2),  // bottom left
                new Vector3(length/2, 0, -width/2),   // bottom right
                new Vector3(length/2, 0, width/2),    // top right
                new Vector3(-length/2, 0, width/2)    // top left
            };

            int[] triangles = new int[]
            {
                0, 1, 2, // First triangle
                2, 3, 0  // Second triangle
            };

            // Create and assign the mesh
            SelectionMesh = new Mesh();
            SelectionMesh.vertices = vertices;
            SelectionMesh.triangles = triangles;
            SelectionMesh.RecalculateNormals();
            SelectionMeshObject.mesh = SelectionMesh;

            // Position the mesh object at the center point
            SelectionMeshObject.transform.position = centerPoint;
            SelectionMeshObject.transform.rotation = Quaternion.identity;
            
            Bounds meshBounds = SelectionMesh.bounds;
            float extractedLength = meshBounds.size.x; // Length (x-axis)
            float extractedWidth = meshBounds.size.z;  // Width (z-axis)
            float extractedArea = extractedLength * extractedWidth * 100;

            // Display the area
            debugArea.text = $"Area from Mesh Bounds: {((float)Mathf.Round(extractedArea))/100}m\u00b2";
            Debug.Log($"Area from Mesh Bounds: {extractedArea}");
            
            SelectionMeshObject.gameObject.SetActive(true);
            
            ShuffleButton.SetActive(true);
            objectPlacer.PlaceObject(SelectionMesh,centerPoint);
        }
        else
        {
            _InSelection = true;
            OrangeTarget.gameObject.SetActive(true);
            SelectionMeshObject.gameObject.SetActive(false);
            ShuffleButton.SetActive(false);
            objectPlacer.ResetNewObject();
        }
    }
}
