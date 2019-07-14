using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIHealthBar : MonoBehaviour
{
    //シングルトン化
    public static UIHealthBar instance { get; private set; }

    public Image mask;
    private float originalSize;

    void Awake() {
        instance = this;
    }

    void Start()
    {
        originalSize = mask.rectTransform.rect.width;
    }
    
    /// <summary>
    /// HealthBarとライフの現在値を合わせる処理
    /// </summary>
    /// <param name="value"></param>
    public void SetValue(float value) {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
    }
}
