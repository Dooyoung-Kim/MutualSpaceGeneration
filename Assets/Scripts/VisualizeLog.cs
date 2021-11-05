using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UVR.TranSpace.MutualSpace
{
    public class VisualizeLog : MonoBehaviour
    {
        private TextMesh currentMatchRatio;
        private TextMesh maxMatchRatio;
        private TextMesh maxValues;
        // Start is called before the first frame update
        void Start()
        {
            currentMatchRatio = gameObject.transform.Find("CurrentMatchRatio").gameObject.GetComponent<TextMesh>();
            maxMatchRatio = gameObject.transform.Find("MaxMatchRatio").gameObject.GetComponent<TextMesh>();
            maxValues = gameObject.transform.Find("MaxValues").gameObject.GetComponent<TextMesh>();
        }

        // Update is called once per frame
        void Update()
        {
            currentMatchRatio.text = "Current Match Ratio: " + string.Format("{0:0.####}", MatchManager.matchStatic.getCurrentMatchRatio());
            maxMatchRatio.text = "Max Match Ratio: " + string.Format("{0:0.####}", MatchManager.matchStatic.getMaxMatchRatio());
            maxValues.text = "Max Values " + "\n" + "Rotation : " + Quaternion.Angle(Quaternion.Euler(0f, 0f, 0f), MatchManager.matchStatic.getMaxRotationValue()) + "º\n" + "Scale Rate\n- x scale : " + MatchManager.matchStatic.getMaxAlphaValue() + "\n- z scale : " + MatchManager.matchStatic.getMaxBetaValue();
        }
    }
}