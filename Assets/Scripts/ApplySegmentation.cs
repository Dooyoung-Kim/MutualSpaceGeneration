using System.Collections.Generic;
using Unity.Simulation;
using UnityEngine;

public class ApplySegmentation : MonoBehaviour
{

    public Shader segmentShader;
    public Camera segmentCamera;

    Dictionary<string, Color32> segmentDict = new Dictionary<string, Color32>();

    void Start()
    {
        Debug.Log(Application.persistentDataPath + "/" + Configuration.Instance.GetAttemptId());

        // Fill the Dictionary with Tag names and corresponding colors
        segmentDict.Add("Table", new Color32(255, 0, 0, 255));
        segmentDict.Add("Floor", new Color32(0, 255, 0, 255));
        //segmentDict.Add("Furniture", new Color32(0, 0, 255, 255));
        segmentDict.Add("ARFurniture", new Color32(162, 40, 255, 255));
        segmentDict.Add("VRFurniture", new Color32(0, 0, 255, 255));
        segmentDict.Add("Wall", new Color32(165, 42, 42, 255));


        // Find all GameObjects with Mesh Renderer and add a color variable to be
        // used by the shader in it's MaterialPropertyBlock
        var renderers = FindObjectsOfType<MeshRenderer>();
        var mpb = new MaterialPropertyBlock();
        foreach (var r in renderers)
        {

            if (segmentDict.TryGetValue(r.transform.tag, out Color32 outColor))
            {
                mpb.SetColor("_SegmentColor", outColor);
                r.SetPropertyBlock(mpb);
            }
        }

        // Finally set the Segment shader as replacement shader
        segmentCamera.SetReplacementShader(segmentShader, "RenderType");
    }
}
