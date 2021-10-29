using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UVR.TranSpace.MutualSpace
{   
    public class MatchManager : MonoBehaviour
    {
        GameObject ARDT_2D, VRDT_2D;
        GameObject ARAnchor, VRAnchor;
        private float maxMatchRate = 0;
        private float currentMatchRate = 0;
        private float computedMateRatio;
        private Quaternion maxRotationValue;

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
            ARAnchor.transform.position = VRAnchor.transform.position;
            ARAnchor.transform.rotation = VRAnchor.transform.rotation;
        }

        void Update()
        {
            // There are only 4 possible rotation value that keep the parallel table edge condition
            for (int i = 0; i < 4; i++)
            {
                ARAnchor.transform.Rotate(0f, 90f*i, 0f);
                ARDT_2D.transform.rotation = ARAnchor.transform.rotation;
                currentMatchRate = ComputeMatchRate(ARDT_2D, VRDT_2D);
                if(currentMatchRate > maxMatchRate)
                {
                    maxMatchRate = currentMatchRate;
                    maxRotationValue = Quaternion.Euler(0f,90f*i,0f);
                }
            }
        }

        private float ComputeMatchRate(GameObject space1, GameObject space2)
        {

            return computedMateRatio;
        }
    }
}
