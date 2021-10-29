using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UVR.TranSpace.MutualSpace
{   
    public class SetARSpaceToAnchor : MonoBehaviour
    {
        public static SetARSpaceToAnchor anchorAsParent;
        GameObject Anchor = GameObject.FindWithTag("ARDT_2D");

        void Start()
        {
            if (anchorAsParent && anchorAsParent != this)
                Destroy(this);
            else
                anchorAsParent = this;
        }

        void Update()
        {
            setPositionToAnchor();
        }

        public void setPositionToAnchor()
        {
            transform.position = Anchor.transform.position;
            transform.rotation = Anchor.transform.rotation;
        }
    }
}

