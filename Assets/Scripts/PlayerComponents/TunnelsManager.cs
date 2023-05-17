using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelsManager : MonoBehaviour
{
    public class teleports_pair
    {
        public Transform getTeleport1()
        {
            return teleport1;
        }
        public Transform getTeleport2()
        {
            return teleport2;
        }

        public teleports_pair(Transform t1, Transform t2)
        {
            teleport1 = t1;
            teleport2 = t2;
        }

        private Transform teleport1;
        private Transform teleport2;
    };

    // Update is called once per frame
    void Update()
    {
        
    }
}
