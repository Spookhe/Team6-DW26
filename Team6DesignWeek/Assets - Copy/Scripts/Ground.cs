using UnityEngine;

public class Ground : MonoBehaviour
{
    private MeshRenderer groundRenderer;

    private void Awake()
    {
        groundRenderer = GetComponent<MeshRenderer>(); // Renders the ground quad
    }

    private void Update()
    {
        // Scrolls sprite texture based off gamespeed / scale
        float scrollSpeed = GameManager.Instance.gameSpeed / transform.localScale.x;

        // Sprite texture moves horizontally
        Vector2 offset = groundRenderer.material.mainTextureOffset;
        offset.x += scrollSpeed * Time.deltaTime;
        groundRenderer.material.mainTextureOffset = offset;
    }
}
