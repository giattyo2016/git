using UnityEngine;

public class MovingLaser : MonoBehaviour
{
    [Header("移動の設定")]
    [SerializeField] private float speed = 3f;        // 移動速度
    [SerializeField] private float leftWallX = -5f;    // 左側の壁のX座標
    [SerializeField] private float rightWallX = 5f;   // 右側の壁のX座標

    private int direction = 1; // 1: 右へ移動, -1: 左へ移動

    void Update()
    {
        // 現在の位置を取得
        Vector3 currentPosition = transform.position;

        // X軸方向に時間をかけて移動させる
        currentPosition.x += speed * direction * Time.deltaTime;

        // 右の壁に到達、または超えたら左へ反転
        if (currentPosition.x >= rightWallX)
        {
            currentPosition.x = rightWallX; // 位置を壁に固定
            direction = -1;                 // 反転（左向きへ）
        }
        // 左の壁に到達、または超えたら右へ反転
        else if (currentPosition.x <= leftWallX)
        {
            currentPosition.x = leftWallX;   // 位置を壁に固定
            direction = 1;                  // 反転（右向きへ）
        }

        // 計算した新しい位置を適用
        transform.position = currentPosition;
    }
}
