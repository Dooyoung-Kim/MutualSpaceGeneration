using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UVR.TranSpace.MutualSpace
{   
    public class MatchManager : MonoBehaviour
    {
        public static MatchManager matchStatic;
        GameObject ARDT_2D, VRDT_2D;
        GameObject ARAnchor, VRAnchor;
        private double maxMatchRate = 0, currentMatchRate = 0;
        private double currentRotationMatchRate = 0, currentScaleMatchRate = 0, startingMatchRate;
        private float computedMateRatio;
        private Quaternion maxRotationValue, currentRotation;
        private Vector3 maxScaleValue;
        private float alpha = 1, beta = 1, currentAlpha, currentBeta;

        private bool isBegin = true;

        //private int i,j;

        public Text StartRateText;
        public Text CurrentRateText;
        public Text MaxRateText;
        public Text FinalValueText;
        public Text CurrentValueText;
        public Slider StartRate, CurrentRate, MaxRate;
        private Vector3 ARScale;
        private Vector3 VRScale;
        void Awake()
        {
            ARDT_2D = GameObject.FindWithTag("ARDT_2D");
            VRDT_2D = GameObject.FindWithTag("VRDT_2D");
            ARAnchor = GameObject.FindWithTag("ARAnchor");
            VRAnchor = GameObject.FindWithTag("VRAnchor");

            // Set transform of Anchor to VR client space's table origin.
        }

        void Start()
        {
            if (matchStatic && matchStatic != this)
                Destroy(this);
            else
                matchStatic = this;

            ARAnchor.transform.position = VRAnchor.transform.position;
            ARAnchor.transform.rotation = VRAnchor.transform.rotation;
        }

        void Update()
        {

            if(Input.GetKey(KeyCode.Z)){
                ComputeMatchRatio.matchRatio.computeMatchRate();
            }

            // if(Input.GetKey(KeyCode.X)){
            //     computeMaxRotationSetting();
            //     setARHostToMaxMatchRotation();
            // }

            if(Input.GetKey(KeyCode.Space)){
                //ScaleAdjusting();
                StartCoroutine(OptimalValueFinding());
                setARHostToMaxMatchRotation();
                setVRClientToMaxMatchScale();
            }

            // if(Input.GetKey(KeyCode.Space)){
            //     findMaxMatchRatioSetting();
            //     setARHostToMaxMatchRotation();
            //     setVRClientToMaxMatchScale();
            // }
        }

        private void texting()
        {
            StartRate.value = (float) startingMatchRate;
            CurrentRate.value = (float) currentScaleMatchRate;
            MaxRate.value = (float) maxMatchRate;

            StartRateText.text = "" + string.Format("{0:0.###}", startingMatchRate);
            //CurrentValueText.text = "Current Values\n" + "Rotation : " + Quaternion.Angle(Quaternion.Euler(0f, 0f, 0f), currentRotation) + "??\n" + "Scale Rate\n- Alpha : " + currentAlpha + "\n- Beta : " + currentBeta;
            CurrentValueText.text = "Current Values\n" + "Rotation : " + currentRotation.eulerAngles.y + "??\n" + "Scale Rate\n- Alpha : " + currentAlpha + "\n- Beta : " + currentBeta;
            CurrentRateText.text = "" + string.Format("{0:0.###}", currentScaleMatchRate);
            FinalValueText.text = "Optimized Values\n" + "Rotation : " + maxRotationValue.eulerAngles.y + "??\n" + "Scale Rate\n- Alpha : " + alpha + "\n- Beta : " + beta;
            MaxRateText.text = "" + string.Format("{0:0.###}", maxMatchRate);
        }
        
        IEnumerator OptimalValueFinding()
        {
            if(isBegin)
            {
                isBegin = false;
                SetARTableAnchorAsParent.ARAnchorAsParent.setPositionToARAnchor();
                ComputeMatchRatio.matchRatio.computeMatchRate();
                startingMatchRate = ComputeMatchRatio.matchRatio.getMatchRatio();
            }

            for (int k = 0; k < 4; k++)
            {
                //?????? ?????? loop?????? maxMatchRate??? previousMaxMatchRate??? ??????
                //var previousMaxMatchRate = maxMatchRate;
                
                //VRAnchor rotation??? ?????? ARAnchor rotation ????????? ?????????
                VRAnchor.transform.rotation = ARAnchor.transform.rotation;

                VRAnchor.transform.Rotate(0f, 90f * k, 0f);
                SetVRTableAnchorAsParent.VRAnchorAsParent.setPositionToVRAnchor();

                currentRotation = VRAnchor.transform.rotation;
                Debug.Log("Current Rotation Setting is " + currentRotation);

                // Translation Gain threshold range according to [Steinicke et al., 2010]
                for (int i = 85; i <= 125; i = i+5){
                    for (int j = 85; j <= 125; j = j+5){

                        // Initialize VRDT_2D Scale to original value, (1,1,1)
                        VRDT_2D.transform.localScale = VRAnchor.transform.localScale;
                        
                        // Relative Translation Gain Threshold Range Condition from Our Current Study (VR 2022 in progress)
                        if(((double) i <= (double) j * 1.24 && (double) i >= (double) j * 0.96) || ((double) j <= (double) i * 1.24 && (double) j >= (double) i * 0.96))
                        {
                            // Modify VRDT's x, z scale with following relative translation gains.
                            var newScale = new Vector3(
                            transform.localScale.x * (float) i/100,
                            transform.localScale.y,
                            transform.localScale.z * (float) j/100);

                            VRDT_2D.transform.localScale = newScale;
                            
                            currentAlpha = (float) i/100;
                            currentBeta = (float) j/100;
                            Debug.Log("Current (alpha, beta) : " + currentAlpha + " , " + currentBeta);
                            ComputeMatchRatio.matchRatio.computeMatchRate();
                            currentScaleMatchRate = ComputeMatchRatio.matchRatio.getMatchRatio();
                            setCurrentMatchRatio(currentScaleMatchRate);
                            
                            if (maxMatchRate < currentScaleMatchRate)
                            {
                                maxScaleValue = newScale;
                                maxRotationValue = Quaternion.Euler(0f, 90f * k, 0f);
                                maxMatchRate = currentScaleMatchRate;
                                alpha = currentAlpha;
                                beta = currentBeta;
                                Debug.Log("Max VR Client Space Rotation Setting : " + maxRotationValue);
                                Debug.Log("Max VR Client Space Scaling Setting (x scale, y scale, z scale) for VR Client Space is " + "(" + alpha + ", 1, " + beta + ")");
                                Debug.Log("Max Match Ratio : " + maxMatchRate);
                            }

                            texting();
                        }

                        yield return null;
                    }
                }
            }

            setVRClientToMaxMatchRotation();
            setVRClientToMaxMatchScale();

            texting();
        }

        // //Compute max rotation setting for AR host's space to VR client's space
        // private void computeMaxRotationSetting()
        // {   
        //     // There are only 4 possible rotation value that keep the parallel table edge condition
        //     for (int i = 0; i < 4; i++)
        //     {
        //         ARAnchor.transform.rotation = VRAnchor.transform.rotation;
        //         ARAnchor.transform.Rotate(0f, 90f * i, 0f);
        //         SetARTableAnchorAsParent.ARAnchorAsParent.setPositionToARAnchor();
        //         ComputeMatchRatio.matchRatio.computeMatchRate();
        //         currentRotationMatchRate = ComputeMatchRatio.matchRatio.getMatchRatio();
                
        //         if(currentRotationMatchRate > maxMatchRate)
        //         {
        //             maxMatchRate = currentRotationMatchRate;
        //             maxRotationValue = Quaternion.Euler(0f,90f * i,0f);
        //         }
        //     }

        //     Debug.Log("Match Ratio (with Max Rotation Value Applied) : " + maxMatchRate);
        //     Debug.Log("Maximizing Quaternion value for AR host's space is : " + maxRotationValue);
        // }

        // //Find both maximizing quaternion value for AR host's space and maximizing scaling value for VR client's space
        // private void findMaxMatchRatioSetting()
        // {   
        //     // There are only 4 possible rotation value that keep the parallel table edge condition
        //     for (int i = 0; i < 4; i++)
        //     {
        //         //?????? ?????? loop?????? maxMatchRate??? previousMaxMatchRate??? ??????
        //         var previousMaxMatchRate = maxMatchRate;
                
        //         //ARAnchor rotation??? ?????? VRAnchor rotation ????????? ?????????
        //         ARAnchor.transform.rotation = VRAnchor.transform.rotation;

        //         ARAnchor.transform.Rotate(0f, 90f * i, 0f);
        //         SetARTableAnchorAsParent.ARAnchorAsParent.setPositionToARAnchor();

        //         var currentRotation = ARAnchor.transform.rotation;
        //         Debug.Log("Current Rotation Setting is " + currentRotation);

        //         //ScaleAdjusting();
                
        //         if(maxMatchRate >= previousMaxMatchRate)
        //         {
        //             maxRotationValue = Quaternion.Euler(0f, 90f * i, 0f);
        //         }
        //     }

        //     Debug.Log("Max AR Host Space Rotation Setting : " + maxRotationValue);
        //     Debug.Log("Max VR Client Space Scaling Setting (x scale, y scale, z scale) for VR Client Space is " + "(" + alpha + ", 1, " + beta + ")");
        //     Debug.Log("Max Match Ratio : " + maxMatchRate);
        //     FinalValueText.text = "Optimized Values\n" + "Rotation : " + Quaternion.Angle(Quaternion.Euler(0f, 0f, 0f), maxRotationValue) + "??\n" + "Scale Rate\n- Alpha : " + alpha + "\n- Beta : " + beta;
        //     Debug.Log("Max Match Ratio : " + maxMatchRate);
        //     MaxRateText.text = "" + string.Format("{0:0.####}", maxMatchRate);
        // }

        private void setARHostToMaxMatchRotation(){
            ARAnchor.transform.rotation = maxRotationValue;
            SetARTableAnchorAsParent.ARAnchorAsParent.setPositionToARAnchor();
        }

        private void setVRClientToMaxMatchRotation(){
            VRAnchor.transform.rotation = maxRotationValue;
            SetVRTableAnchorAsParent.VRAnchorAsParent.setPositionToVRAnchor();
        }
        private void setVRClientToMaxMatchScale(){
            VRDT_2D.transform.localScale = new Vector3(
            transform.localScale.x * alpha,
            transform.localScale.y,
            transform.localScale.z * beta);
        }

        public void setCurrentMatchRatio(double matchRatio){
            currentMatchRate = matchRatio;
        }

        public double getCurrentMatchRatio(){
            return currentMatchRate;
        }

        public double getMaxMatchRatio(){
            return maxMatchRate;
        }

        public Quaternion getMaxRotationValue(){
            return maxRotationValue;
        }
        public float getMaxAlphaValue(){
            return alpha;
        }
        public float getMaxBetaValue(){
            return beta;
        }
    }
}
