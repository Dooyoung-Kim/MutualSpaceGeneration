using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UVR.TranSpace.MutualSpace
{   
    public class SetARTableAnchorAsParent : MonoBehaviour
    {
        public static SetARTableAnchorAsParent ARAnchorAsParent;
        public GameObject ARAnchor;

        void Start()
        {
            if (ARAnchorAsParent && ARAnchorAsParent != this)
                Destroy(this);
            else
                ARAnchorAsParent = this;

            transform.position = ARAnchor.transform.position;
            transform.rotation = ARAnchor.transform.rotation;
        }
        void Update()
        {
            transform.position = ARAnchor.transform.position;
            transform.rotation = ARAnchor.transform.rotation;
        }

        public void setPositionToARAnchor(){
            transform.position = ARAnchor.transform.position;
            transform.rotation = ARAnchor.transform.rotation;
            Debug.Log("AR Host Space position and rotation is set to AR Anchor");
        }
    }
}

