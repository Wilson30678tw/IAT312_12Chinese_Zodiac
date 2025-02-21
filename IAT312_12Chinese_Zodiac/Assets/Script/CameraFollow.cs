using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // 玩家 Transform
    public Vector3 offset = new Vector3(0, 2, -10); // 摄像机偏移量

    void Update()
    {
        if (player != null)
        {
            transform.position = player.position + offset; // 让摄像机始终跟随玩家
        }
    }
}