using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixcelCamera : MonoBehaviour
{
  
    public float spritePPU = 40f;  // �v���C���[��PPU
    public float lerpSpeed = 0.5f; // 0~1�Ŋۂߕ�ԑ��x

    private Vector3 targetPos;

    void Start()
    {
        targetPos = transform.position;
    }

    void LateUpdate()
    {
        // �܂��ڕW�ʒu���ۂ߂�
        Vector3 rounded = new Vector3(
            Mathf.Round(transform.position.x * spritePPU) / spritePPU,
            Mathf.Round(transform.position.y * spritePPU) / spritePPU,
            transform.position.z
        );

        // ���݈ʒu�Ɗۂ߈ʒu����
        targetPos = Vector3.Lerp(targetPos, rounded, lerpSpeed);

        transform.position = targetPos;
    }
}
