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
        // Start with an empty bounds
        FurnitureBounds = new Bounds(transform.position, Vector3.zero);
        // Debug.Log($"Original Total Bounds : {FurnitureBounds.size}");
        // Debug.Log($"Calculating Bounds for {name}");
        // Encapsulate the cushion's bounds
        if (cushionMeshFilter != null && cushionMeshFilter.mesh != null)
        {
            Bounds cushionBounds = cushionMeshFilter.mesh.bounds;
            //Debug.Log($"Original Cushion Bounds : {cushionBounds.size}");
            cushionBounds.center = cushionMeshFilter.transform.TransformPoint(cushionBounds.center);
            Vector3 cushionSize = Vector3.Scale(cushionBounds.size, cushionMeshFilter.transform.lossyScale);
            //Debug.Log($"Global Cush Scale : {cushionMeshFilter.transform.lossyScale}");
            //Debug.Log($"Rescale Cushion Bounds : {cushionSize}");
            FurnitureBounds.Encapsulate(new Bounds(cushionBounds.center, cushionSize));
        }

        // Encapsulate the support's bounds
        if (supportMeshFilter != null && supportMeshFilter.mesh != null)
        {
            Bounds supportBounds = supportMeshFilter.mesh.bounds;
            //Debug.Log($"Original Support Bounds : {supportBounds.size}");
            supportBounds.center = supportMeshFilter.transform.TransformPoint(supportBounds.center);
            Vector3 supportSize = Vector3.Scale(supportBounds.size, supportMeshFilter.transform.lossyScale);
            //Debug.Log($"Rescale Support Bounds : {supportBounds.size}");
            FurnitureBounds.Encapsulate(new Bounds(supportBounds.center, supportSize));
        }

        //Debug.Log($"Final Total Bounds : {FurnitureBounds.size}");
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
