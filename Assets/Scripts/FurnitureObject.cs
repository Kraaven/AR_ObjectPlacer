using System;
using UnityEngine;

public class FurnitureObject : MonoBehaviour
{
    [Header("Original Materials")] 
    [SerializeField] private Material cushionMaterial;
    [SerializeField] private Material supportMaterial;
    [Header("Meshes")] 
    [SerializeField] private MeshFilter cushionMeshFilter;
    [SerializeField] private MeshFilter supportMeshFilter;
    public Bounds FurnitureBounds;
    [SerializeField] private MeshRenderer cushionRenderer;
    [SerializeField] private MeshRenderer supportRenderer;
    [Header("API")] private OpenAI API;
    private bool GenerationRunning;

    private Texture2D generatedTexture2D;

    private void Start()
    {
        API = FindFirstObjectByType<OpenAI>();
    }

    public Bounds GetBounds()
    {
        FurnitureBounds = new Bounds(transform.position, Vector3.zero);
        if (cushionMeshFilter != null && cushionMeshFilter.mesh != null)
        {
            Bounds cushionBounds = cushionMeshFilter.mesh.bounds;
            cushionBounds.center = cushionMeshFilter.transform.TransformPoint(cushionBounds.center);
            Vector3 cushionSize = Vector3.Scale(cushionBounds.size, cushionMeshFilter.transform.lossyScale);
            FurnitureBounds.Encapsulate(new Bounds(cushionBounds.center, cushionSize));
        }

        // Encapsulate the support's bounds
        if (supportMeshFilter != null && supportMeshFilter.mesh != null)
        {
            Bounds supportBounds = supportMeshFilter.mesh.bounds;
            supportBounds.center = supportMeshFilter.transform.TransformPoint(supportBounds.center);
            Vector3 supportSize = Vector3.Scale(supportBounds.size, supportMeshFilter.transform.lossyScale);
            FurnitureBounds.Encapsulate(new Bounds(supportBounds.center, supportSize));
        }
        
        return FurnitureBounds;
    }

    public void ReSetCushionMat(Material material)
    {
        cushionRenderer.material = material;
    }
    
    public void ReSetSupportMat(Material material)
    {
        supportRenderer.material = material;
    }

    public void SetCushionMaterial(string Mat)
    {
        if(GenerationRunning) return;

        GenerationRunning = true;
        // Debug.Log($"Set Material to : {Mat}");
        API.GenerateMaterial(Mat, (Material M) =>
        {
            cushionRenderer.material = M;
            GenerationRunning = false;
        }, () =>
        { 
            Debug.Log("Material Generation Failed");
            GenerationRunning = false;
        });
    }






}
