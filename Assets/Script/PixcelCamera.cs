using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixcelCamera : MonoBehaviour
{
  
    public float spritePPU = 40f;  // プレイヤーのPPU
    public float lerpSpeed = 0.5f; // 0~1で丸め補間速度

    private Vector3 targetPos;

    void Start()
    {
        targetPos = transform.position;
    }

    void LateUpdate()
    {
        // まず目標位置を丸める
        Vector3 rounded = new Vector3(
            Mathf.Round(transform.position.x * spritePPU) / spritePPU,
            Mathf.Round(transform.position.y * spritePPU) / spritePPU,
            transform.position.z
        );

        // 現在位置と丸め位置を補間
        targetPos = Vector3.Lerp(targetPos, rounded, lerpSpeed);

        transform.position = targetPos;
    }
}
