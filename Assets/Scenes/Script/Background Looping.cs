using UnityEngine;

public class BackgroundLooping : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float loopspeed;
    public Renderer rend;

    // Update is called once per frame
    void Update()
    {
        rend.material.mainTextureOffset += new Vector2(loopspeed * Time.deltaTime, 0f);

    }
}
