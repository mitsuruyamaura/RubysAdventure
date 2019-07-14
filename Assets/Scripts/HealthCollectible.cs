using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 回復アイテムの制御クラス
/// </summary>
public class HealthCollectible : MonoBehaviour
{
    [SerializeField, Header("ライフの回復量")]
    public int recoveryHealth;
    [SerializeField, Header("回復エフェクト")]
    public ParticleSystem bonusEffectPrefab;
    [SerializeField, Header("回復用SE")]
    public AudioClip collectedClip;

    private void OnTriggerEnter2D(Collider2D col) {
        RubyController ruby = col.gameObject.GetComponent<RubyController>();
        if (ruby != null) {
            if (ruby.Health < ruby.maxHealth) {  // プロパティでcurrentHealthを取得
                // 回復処理、エフェクトとSE再生
                ruby.ChangeHealth(recoveryHealth);
                ParticleSystem effect = Instantiate(bonusEffectPrefab,transform);
                effect.Play();
                Destroy(gameObject,1.0f);
                ruby.PlaySound(collectedClip);
            }
        }
    }
}
