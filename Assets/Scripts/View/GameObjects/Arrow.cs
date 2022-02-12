using UnityEngine;

public class Arrow : MonoBehaviour
{
    private GameObject _parentNode;
    private GameObject _childNode;
    private Vector3 localScaleValue;

    public GameObject parentNode
    {
        get { return _parentNode; }
        set { _parentNode = value; value.GetComponent<GameobjectNode>().OnTransformChanged += Arrow_OnTransformChanged; }
    }

    public GameObject childNode
    {
        get { return _childNode; }
        set { _childNode = value; value.GetComponent<GameobjectNode>().OnTransformChanged += Arrow_OnTransformChanged; }
    }

    private void Arrow_OnTransformChanged(object source, System.EventArgs args)
    {
        refreshPosition();
    }

    public void updateScale(Vector3 newScale)
    {
        this.localScaleValue = new Vector3(newScale.x, newScale.y, localScaleValue.z);
        refreshPosition();
    }

    private void refreshPosition()
    {
        if (_parentNode == null || _childNode == null)
            return;

        float distanceToTargetNode = Vector3.Distance(_parentNode.transform.position, _childNode.transform.position);
        transform.position = _parentNode.transform.position;
        transform.LookAt(_childNode.transform);

        if (localScaleValue == Vector3.zero)
        {
            localScaleValue = new Vector3(0.3f, 0.3f, distanceToTargetNode / 5.5f);
        }
        transform.localScale = localScaleValue;
    }


}
