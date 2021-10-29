using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UVR.TranSpace.MutualSpace
{   
    public class SetTableAnchorAsParent : MonoBehaviour
    {
        public static SetTableAnchorAsParent anchorAsParent;
        public GameObject Anchor;

        void Start()
        {
            transform.position = Anchor.transform.position;
            transform.rotation = Anchor.transform.rotation;
        }
        void Update()
        {
            transform.position = Anchor.transform.position;
            transform.rotation = Anchor.transform.rotation;
        }

        public void setPositionToAnchor(){

        }
    }
}

