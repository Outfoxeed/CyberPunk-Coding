using System;
using UnityEngine;

namespace CyberPunkCoding
{
    public class ZoneTarget : MonoBehaviour
    {
        [SerializeField] private BoxCollider boxCollider;
        public BoxCollider BoxCollider => boxCollider;
    }
}