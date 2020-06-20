using System;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
    public class AutoMoveAndRotate : MonoBehaviour
    {
        public Vector3andSpace moveUnitsPerSecond;
        public Vector3andSpace rotateDegreesPerSecond;
        public bool ignoreTimescale;
        public float speed = 1f;
        private float m_LastRealTime;
        private Client client;
        
        


        private void Start()
        {
            if (gameObject.tag.Equals("Enemy"))
            {
                client = GameObject.FindGameObjectWithTag("client").GetComponent<Client>();
                int level = client.getInt("LevelSelect");

                switch (level)
                {
                    case 1:
                        speed = 1.3f;
                        break;
                    case 2:
                        speed = 1.6f;
                        break;
                    case 3:
                        speed = 2f;
                        break;
              
                }
            }
            
            m_LastRealTime = Time.realtimeSinceStartup;
        }


        // Update is called once per frame
        private void Update()
        {
            float deltaTime = Time.deltaTime;
            if (ignoreTimescale)
            {
                deltaTime = (Time.realtimeSinceStartup - m_LastRealTime);
                m_LastRealTime = Time.realtimeSinceStartup;
            }
            transform.Translate(moveUnitsPerSecond.value*deltaTime*speed, moveUnitsPerSecond.space);
            transform.Rotate(rotateDegreesPerSecond.value*deltaTime, moveUnitsPerSecond.space);
        }


        [Serializable]
        public class Vector3andSpace
        {
            public Vector3 value;
            public Space space = Space.Self;
        }
    }
}
