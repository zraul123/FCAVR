using UnityEngine;
using UnityEngine.Analytics;

[ExecuteInEditMode]
public class ScrollContainerSizer : MonoBehaviour
{
    public int MinHeight = 300;
    public int OffsetBetweenItems = 50;
    public int SizeOffset = 50;

    private RectTransform rectTransform;

    public void ComputeRect(int count)
    {
        rectTransform = GetComponent<RectTransform>();
        var height = SizeOffset * count;
        if (height < MinHeight)
        {
            height = MinHeight;
        }
        else
        {
            rectTransform.localPosition = CalculateLocalPosition(height);
        }

        rectTransform.sizeDelta = CalculateSizeDelta(height);
    }

    private Vector3 CalculateLocalPosition(float height)
    {
        var position = rectTransform.localPosition;
        position.y = (MinHeight - height) / 2;

        return position;
    }

    private Vector2 CalculateSizeDelta(float height)
    {
        var sizeDelta = rectTransform.sizeDelta;
        return new Vector2(sizeDelta.x, height);
    }
}
