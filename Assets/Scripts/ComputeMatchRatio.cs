using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UVR.TranSpace.MutualSpace
{
    public class ComputeMatchRatio : MonoBehaviour {
        public static ComputeMatchRatio matchRatio;
        public Camera ARCam;
        public Camera VRCam;
        //public SpriteRenderer sprite; 
        private int totalIntersectedPixel, totalMatchedPixel;
        private Color _ARcolor, _VRcolor;
        private static Color black;
        Texture2D texture, ARCamTexture, VRCamTexture;
        private double computedMatchRatio;

        void Start()
        {
            if (matchRatio && matchRatio != this)
                Destroy(this);
            else
                matchRatio = this;
        }

        void Update()
        {

        }

        // Compute Match Rate between AR host Space and VR client space
        public void computeMatchRate()
        {   
            totalIntersectedPixel = 0;
            totalMatchedPixel = 0;

            ARCamTexture = RTImage(ARCam);
            VRCamTexture = RTImage(VRCam);

            for(int i=1; i<=256; i++)
            {
                for(int j=1; j<=256; j++)
                {
                    _ARcolor = ARCamTexture.GetPixel(i,j);
                    _VRcolor = VRCamTexture.GetPixel(i,j);

                    if(_ARcolor != black && _VRcolor != black)
                    {
                        totalIntersectedPixel++;
                        if(_ARcolor == _VRcolor) totalMatchedPixel++;
                    }
                }
            }

            Debug.Log("Total Intersected Pixel : " + totalIntersectedPixel);
            Debug.Log("Total Matched Pixel Number : " + totalMatchedPixel);
            Debug.Log("Match Ratio between AR host space and VR client space is "+ (double) totalMatchedPixel/ (double) totalIntersectedPixel);
        
            setMatchRatio((double) totalMatchedPixel/ (double) totalIntersectedPixel);
        }

        Texture2D RTImage(Camera cam)
        {
            RenderTexture currentRT = RenderTexture.active;
            RenderTexture.active = cam.targetTexture;

            cam.Render();

            Texture2D image = new Texture2D(cam.targetTexture.width, cam.targetTexture.height);
            image.ReadPixels(new Rect(0, 0, cam.targetTexture.width, cam.targetTexture.height),0,0);
            image.Apply();
            RenderTexture.active = currentRT;
            
            return image;
        }

        public void setMatchRatio(double ratio){
            computedMatchRatio = ratio;
        }

        public double getMatchRatio(){
            return computedMatchRatio;
        }
    }
}