using UnityEngine;

namespace CyberPunkCoding
{
    public abstract class Objective : MonoBehaviour
    {
        public bool Valid => valid;
        protected bool valid;

        public virtual void Reset()
        {
            valid = false;
        }
    }
}