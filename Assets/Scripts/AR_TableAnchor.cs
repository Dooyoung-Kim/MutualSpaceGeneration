using UnityEngine;

namespace UVR.TranSpace.MutualSpace
{
    public class AR_TableAnchor : MonoBehaviour
    {
        public static AR_TableAnchor ARInstance;
        GameObject ARTableOrigin;

        void Awake()
        {
            ARTableOrigin = GameObject.FindWithTag("ARTableOrigin");
            transform.position = ARTableOrigin.transform.position;
            transform.rotation = ARTableOrigin.transform.rotation;
        }
        private void Start()
        {

        }
    }
}