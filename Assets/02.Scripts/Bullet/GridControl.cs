using UnityEngine;

public class GridControl : MonoBehaviour
{
    [Header("Grid trail")]
    [SerializeField] private Camera targetCamera;
    [SerializeField, Min(64)] private int textureSize = 512;
    [SerializeField, Min(0f)] private float stampSize = 0.045f;
    [SerializeField, Min(0f)] private float trailStrength = 1f;
    [SerializeField, Range(0f, 1f)] private float trailFade = 0.93f;

    private static RenderTexture trailTexture;
    private static RenderTexture workTexture;
    private static Material stampMaterial;
    private static bool backgroundBound;
    private static int lastFrame = -1;

    private static readonly int TrailTextureId = Shader.PropertyToID("_GridDistortionTex");
    private static readonly int StampPositionId = Shader.PropertyToID("_StampPosition");
    private static readonly int StampSizeId = Shader.PropertyToID("_StampSize");
    private static readonly int StampStrengthId = Shader.PropertyToID("_StampStrength");
    private static readonly int FadeId = Shader.PropertyToID("_Fade");
    private static readonly int TrailTexelSizeId = Shader.PropertyToID("_GridTrailTexelSize");

    private void Awake()
    {
        if (targetCamera == null) targetCamera = Camera.main;
        EnsureResources(textureSize);
    }

    private void LateUpdate()
    {
        if (targetCamera == null) targetCamera = Camera.main;
        if (targetCamera == null || !EnsureResources(textureSize)) return;

        if (lastFrame != Time.frameCount)
        {
            lastFrame = Time.frameCount;
            FadeTrail(trailFade);
        }

        Vector3 viewport = targetCamera.WorldToViewportPoint(transform.position);
        if (viewport.z > 0f && viewport.x >= 0f && viewport.x <= 1f && viewport.y >= 0f && viewport.y <= 1f)
            StampTrail(new Vector2(viewport.x, viewport.y), stampSize, trailStrength);
    }

    private static bool EnsureResources(int requestedSize)
    {
        if (trailTexture != null && stampMaterial != null) return true;
        Shader shader = Shader.Find("Hidden/GridTrailStamp");
        if (shader == null)
        {
            Debug.LogWarning("GridControl: Hidden/GridTrailStamp shader를 찾을 수 없습니다.");
            return false;
        }

        int size = Mathf.Max(64, requestedSize);
        trailTexture = CreateTexture(size, "Grid Trail A");
        workTexture = CreateTexture(size, "Grid Trail B");
        stampMaterial = new Material(shader) { hideFlags = HideFlags.HideAndDontSave };
        Shader.SetGlobalTexture(TrailTextureId, trailTexture);
        Shader.SetGlobalVector(TrailTexelSizeId, new Vector4(1f / size, 1f / size, size, size));
        BindBackgroundMaterial();
        return true;
    }

    private static void BindBackgroundMaterial()
    {
        if (backgroundBound) return;
        GameObject background = GameObject.Find("GridBackGround");
        Renderer renderer = background == null ? null : background.GetComponent<Renderer>();
        if (renderer == null) return;

        renderer.material.SetTexture(TrailTextureId, trailTexture);
        backgroundBound = true;
    }

    private static RenderTexture CreateTexture(int size, string textureName)
    {
        var texture = new RenderTexture(size, size, 0, RenderTextureFormat.R8)
        {
            name = textureName, filterMode = FilterMode.Bilinear,
            wrapMode = TextureWrapMode.Clamp, useMipMap = false, autoGenerateMips = false
        };
        texture.Create();
        Clear(texture);
        return texture;
    }

    private static void Clear(RenderTexture target)
    {
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = target;
        GL.Clear(false, true, Color.black);
        RenderTexture.active = previous;
    }

    private static void FadeTrail(float fade)
    {
        stampMaterial.SetFloat(FadeId, fade);
        stampMaterial.SetVector(StampPositionId, new Vector4(-10f, -10f, 0f, 0f));
        stampMaterial.SetFloat(StampSizeId, 0f);
        stampMaterial.SetFloat(StampStrengthId, 0f);
        Graphics.Blit(trailTexture, workTexture, stampMaterial);
        SwapTextures();
    }

    private static void StampTrail(Vector2 viewportPosition, float size, float strength)
    {
        stampMaterial.SetFloat(FadeId, 1f);
        stampMaterial.SetVector(StampPositionId, new Vector4(viewportPosition.x, viewportPosition.y, 0f, 0f));
        stampMaterial.SetFloat(StampSizeId, size);
        stampMaterial.SetFloat(StampStrengthId, strength);
        Graphics.Blit(trailTexture, workTexture, stampMaterial);
        SwapTextures();
    }

    private static void SwapTextures()
    {
        RenderTexture temporary = trailTexture;
        trailTexture = workTexture;
        workTexture = temporary;
        Shader.SetGlobalTexture(TrailTextureId, trailTexture);
        if (backgroundBound)
        {
            GameObject background = GameObject.Find("GridBackGround");
            Renderer renderer = background == null ? null : background.GetComponent<Renderer>();
            if (renderer != null) renderer.material.SetTexture(TrailTextureId, trailTexture);
        }
    }
}
