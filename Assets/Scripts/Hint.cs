using UnityEngine;

namespace CyberPunkCoding
{
    [CreateAssetMenu(fileName = "FILENAME", menuName = "Hint", order = 0)]
    public class Hint : ScriptableObject
    {
        [SerializeField] private string hint;
        public override string ToString() => hint;
    }
}