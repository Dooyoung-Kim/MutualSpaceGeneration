using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UVR.TranSpace.MutualSpace
{   
    public class SetARTableAnchorAsParent : MonoBehaviour
    {
        public static SetARTableAnchorAsParent ARAnchorAsParent;
        public GameObject Anchor;

        void Start()
        {
            if (ARAnchorAsParent && ARAnchorAsParent != this)
                Destroy(this);
            else
                ARAnchorAsParent = this;

            transform.position = Anchor.transform.position;
            transform.rotation = Anchor.transform.rotation;
        }
        void Update()
        {
            transform.position = Anchor.transform.position;
            transform.rotation = Anchor.transform.rotation;
        }

        public void setPositionToARAnchor(){
            transform.position = Anchor.transform.position;
            transform.rotation = Anchor.transform.rotation;
            Debug.Log("AR Host Space position and rotation is set to AR Anchor");
        }
    }
}

