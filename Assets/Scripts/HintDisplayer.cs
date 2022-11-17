using System.Collections;
using System.Collections.Generic;
using CyberPunkCoding;
using TMPro;
using UnityEngine;

public class HintDisplayer : MonoBehaviour
{
    [SerializeField] private Hint hint;
    [SerializeField] private TMP_Text text;
    
    void Awake()
    {
        text.text = hint.ToString();
    }
}
