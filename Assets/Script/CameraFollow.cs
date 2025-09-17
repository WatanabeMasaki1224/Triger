using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform player;
    public Vector2 minPosition;
    public Vector2 maxPosition;
    public float smoothSpeed = 0.1f;

    private Vector3 velocity = Vector3.zero;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (player == null) return;

        // �J�����T�C�Y�v�Z
        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        // Clamp �p�̍ŏ��ő�
        float minX = minPosition.x + camWidth;
        float maxX = maxPosition.x - camWidth;
        float minY = minPosition.y + camHeight;
        float maxY = maxPosition.y - camHeight;

        // �v���C���[�Ǐ]
        Vector3 targetPos = new Vector3(player.position.x, player.position.y, transform.position.z);
        targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);
        targetPos.y = Mathf.Clamp(targetPos.y, minY, maxY);

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothSpeed);
    }
}
