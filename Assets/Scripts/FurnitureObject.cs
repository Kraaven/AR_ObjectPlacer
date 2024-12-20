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

    public Bounds GetBounds()
    {
        FurnitureBounds.Encapsulate(cushionMeshFilter.mesh.bounds);
        FurnitureBounds.Encapsulate(supportMeshFilter.mesh.bounds);
        
        return FurnitureBounds;
    }

    public void SetCushionMat(Material material)
    {
        cushionRenderer.material = material;
    }
    
    public void SetSupportMat(Material material)
    {
        supportRenderer.material = material;
    }
    
        

    
    

}
