using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UVR.TranSpace.MutualSpace
{   
    public class SetVRTableAnchorAsParent : MonoBehaviour
    {
        public static SetVRTableAnchorAsParent VRAnchorAsParent;
        public GameObject VRAnchor;

        void Start()
        {
            if (VRAnchorAsParent && VRAnchorAsParent != this)
                Destroy(this);
            else
                VRAnchorAsParent = this;

            transform.position = VRAnchor.transform.position;
            transform.rotation = VRAnchor.transform.rotation;
        }
        void Update()
        {
            transform.position = VRAnchor.transform.position;
            transform.rotation = VRAnchor.transform.rotation;
        }

        public void setPositionToVRAnchor(){
            transform.position = VRAnchor.transform.position;
            transform.rotation = VRAnchor.transform.rotation;
            Debug.Log("VR Client Space position and rotation is set to VR Anchor");
        }
    }
}

