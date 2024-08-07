using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class ExitButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_ColorGradient TMP_ColorGradient;
    TMP_Text TMP_Text;


    // Start is called before the first frame update
    void Start()
    {
        if (TMP_Text == null)
        {
            TMP_Text = GetComponentInChildren<TMP_Text>();
            TMP_Text.enableVertexGradient = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (TMP_Text != null)
        {
            TMP_Text.colorGradientPreset = TMP_ColorGradient;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (TMP_Text != null)
        {
            TMP_Text.colorGradientPreset = null;

        }
    }
}
