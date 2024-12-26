using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine.Networking;

public class OpenAI : MonoBehaviour
{
     private const string ApiUrl = "https://api.openai.com/v1/images/generations";
     private const string ApiKey = "API KEY"; 
    [System.Serializable]
    public class DalleRequest
    {
        public string model = "dall-e-3";
        public string prompt = "A cute baby sea otter";
        public int n = 1;
        public string size = "1024x1024";
        public string response_format = "b64_json";

        public DalleRequest(string Model, string Size, string Prompt)
        {
            model = Model;
            size = Size;
            prompt = Prompt;
        }

        public DalleRequest(string Prompt)
        {
            prompt = Prompt;
        }
    }

    [System.Serializable]
    public class DalleResponse
    {
        public long created;
        public List<Data> data;

        [System.Serializable]
        public class Data
        {
            public string revised_prompt;
            public string b64_json;
        }
    }

    public Dictionary<string, string> GetMaterialRequest()
    {
        var Req = new Dictionary<string, string>()
        {
            { "Use", "3D Model" },
            {"Description of texture", "fuzzy"},
            {"Colour", "turquoise"},
            {"Material", " fabric"},
            {"Additional Info", "Flat 2D image of the texture."}
        };

        return Req;
    }

    public Dictionary<string, string> GetMaterialRequest(string Material, string Color, string texture)
    {
        var Req = GetMaterialRequest();
        Req["Material"] = Material;
        Req["Colour"] = Color;
        Req["Description of texture"] = texture;

        return Req;
    }

    public void GenerateMaterial(string RequestedMat, string col, Action<Material> callback, Action Fallback)
    {
        //string P = $"a 2D seamless texture of {RequestedMat}";
        string P = JsonConvert.SerializeObject(GetMaterialRequest(RequestedMat,col,"Fuzzy"));
        StartCoroutine(GenerateImage(P,callback,Fallback));  
    }

    private IEnumerator GenerateImage(string Prompt,Action<Material> callback, Action Fallback)
    {
        // Create the request body
        DalleRequest requestBody = new DalleRequest(Prompt);
        string jsonBody = JsonUtility.ToJson(requestBody);

        // Create the POST request
        UnityWebRequest request = new UnityWebRequest(ApiUrl, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + ApiKey);

        
        print("Sending Request to API");
        // Send the request and wait for a response
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Response: " + request.downloadHandler.text);

            // Parse the response
            DalleResponse response = JsonConvert.DeserializeObject<DalleResponse>(request.downloadHandler.text);

            if (response.data != null && response.data.Count > 0)
            {
                try
                {
                    // Decode Base64 string to byte array
                    byte[] imageData = System.Convert.FromBase64String(response.data[0].b64_json);

                    // Create a new Texture2D
                    Texture2D texture = new Texture2D(2, 2);

                    // Load image data into the texture
                    if (!texture.LoadImage(imageData))
                    {
                        Debug.LogError("Failed to load image data into Texture2D.");
                        Fallback.Invoke();
                        yield break;
                    }
                    else
                    {
                        Debug.Log("Image data loaded");
                    }

                    // Create a new material with the specified shader
                    Material material = new Material(Shader.Find("Universal Render Pipeline/Lit"));

                    // Assign the texture to the material
                    material.mainTexture = texture;
                    
                    Debug.Log("Material Created");

                    callback.Invoke(material);
                }
                catch (System.Exception ex)
                {
                    //Debug.LogError($"Error converting Base64 image to material: {ex.Message}");
                    Fallback.Invoke();
                    yield break;
                }
            }
        }
        else
        {
            Debug.LogError("Error: " + request.error);
            Fallback.Invoke();
        }
    }
    
    
    
}
