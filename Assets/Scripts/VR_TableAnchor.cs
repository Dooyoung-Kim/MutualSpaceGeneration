using UnityEngine;

namespace UVR.TranSpace.MutualSpace
{
    public class VR_TableAnchor : MonoBehaviour
    {
        public static VR_TableAnchor VRInstance;
        GameObject VRTableOrigin;
        void Awake()
        {
            VRTableOrigin = GameObject.FindWithTag("VRTableOrigin");
            transform.position = VRTableOrigin.transform.position;
            transform.rotation = VRTableOrigin.transform.rotation;
        }
        private void Start()
        {

        }

        void Update()
        {
            
        }
    }
}