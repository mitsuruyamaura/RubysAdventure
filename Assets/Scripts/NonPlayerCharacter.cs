using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerCharacter : MonoBehaviour
{
    [SerializeField, Header("会話表示用キャンバス")]
    public GameObject dialogCanvas;
    [SerializeField, Header("キャンバス表示時間")]
    public float displayTime = 2.5f;
    private float timerDisplay;

    void Start(){
        // ダイアログを非表示にし、タイマーもカウントダウン外の数字に設定
        dialogCanvas.SetActive(false);
        timerDisplay = -1.0f;
    }

    void Update(){
        if (timerDisplay >= 0) {
             timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0) {
                dialogCanvas.SetActive(false);
            }
        }
    }

    /// <summary>
    /// プレイヤーが会話ボタンを押したときに呼ばれる
    /// </summary>
    public void DisplayDialog() {
        // タイマーを初期化し、ダイアログを表示
        timerDisplay = displayTime;
        dialogCanvas.SetActive(true);
    }
}
