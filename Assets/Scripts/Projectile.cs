using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾の制御をするクラス
/// </summary>
public class Projectile : MonoBehaviour
{
    public Rigidbody2D rb;

    /// <summary>
    /// 弾の発射処理
    /// 発射する方向と速度を引数でもらう
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="force"></param>
    public void Launch(Vector2 direction, float force) {        
        rb.AddForce(direction * force);
    }

    private void OnCollisionEnter2D(Collision2D col) {
        EnemyController enemy = col.gameObject.GetComponent<EnemyController>();
        if (enemy != null) {
            enemy.Fix();
        }
        // 他のコライダーに接触したら破壊する
        Destroy(gameObject);
    }

    private void Update() {
        // 画面外に離れて残っていたら破壊する
        if (transform.position.magnitude > 10.0f) {
            Destroy(gameObject);
        }
    }
}
