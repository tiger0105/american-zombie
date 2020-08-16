using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScratchRandom : MonoBehaviour {

    [SerializeField] private Sprite[] m_Images;
    [SerializeField] private Image m_Scratch;

    public void Change()
    {
        m_Scratch.sprite = m_Images[Random.Range(0, m_Images.Length)];
    }
}
