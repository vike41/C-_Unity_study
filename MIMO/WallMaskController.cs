using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WallMaskController : MonoBehaviour
{
    Vector3 minScale;
    public Vector3 widthScale;
    public Vector3 upScale;
    public float speed = 2f;
    public float durationUp = 2f;
    public float durationWidth = 5f;
    private Button button;

    // Start is called before the first frame update

    public void Init()
    {
        this.button = GameObject.FindGameObjectWithTag("MaskButton").GetComponent<Button>();
        if (this.button != null)
        {
            this.button.onClick.AddListener(() => StartCoroutine(OpenWall()));
        }
    }

    public IEnumerator RepeatLerp(Vector3 a, Vector3 b, float time)
    {
        float i = 0.0f;
        float rate = (1.0f / time) * speed;
        while(i < 1.0f)
        {
            i += Time.deltaTime * rate;
            transform.localScale = Vector3.Lerp(a, b, i);
            yield return null;
        }
    }

    public IEnumerator OpenWall()
    {
        minScale = transform.localScale;
        yield return RepeatLerp(minScale, upScale, durationUp);
        yield return RepeatLerp(upScale, widthScale, durationWidth);
    }

}
