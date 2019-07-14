using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダメージエリアの制御クラス
/// </summary>
public class DamageZone : MonoBehaviour
{
    public int damage;

    private void OnTriggerStay2D(Collider2D col) {
        RubyController ruby = col.gameObject.GetComponent<RubyController>();
        if (ruby != null) {
            if (ruby.Health > 0) {
                ruby.ChangeHealth(-damage);
            }
        }
    }
}
