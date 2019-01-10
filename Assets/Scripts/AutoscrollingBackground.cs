using UnityEngine;

public class AutoscrollingBackground : MonoBehaviour
{

    public float ScrollSpeed;
    private Vector2 savedOffset;
    Renderer rendererReference;

    public bool verticalScroll;

    // Use this for initialization
    void Start()
    {
        rendererReference = GetComponent<Renderer>();
        savedOffset = rendererReference.sharedMaterial.GetTextureOffset("_MainTex");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 offset;

        if (verticalScroll)
        {
            float y = Mathf.Repeat(Time.time * ScrollSpeed, 1);
            offset = new Vector2(savedOffset.x, y);
        }
        else
        {
            float x = Mathf.Repeat(Time.time * ScrollSpeed, 1);
            offset = new Vector2(x, savedOffset.y);
        }

        rendererReference.sharedMaterial.SetTextureOffset("_MainTex", offset);
    }

    private void OnDisable()
    {
        if (rendererReference != null)
            rendererReference.sharedMaterial.SetTextureOffset("_MainTex", savedOffset);
    }
}
