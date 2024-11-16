using UnityEngine;

public class RadioDisplay : MonoBehaviour
{
    [SerializeField]
    Transform StartPoint;

    [SerializeField]
    Transform EndPoint;

    [SerializeField]
    Transform Needle;

    [SerializeField]
    private float NeedlePosition = 0.5f;

    private float needlePositionLastFrame = 0;

    public float GetNeedlePosition() 
    {
        return NeedlePosition;
    }

    public void SetNeedlePosition(float needlePos)
    {
        NeedlePosition = Mathf.Clamp(needlePos, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if(NeedlePosition == needlePositionLastFrame)
        {
            return;
        }

        Vector3 newPos = Vector3.Lerp(StartPoint.localPosition, EndPoint.localPosition, NeedlePosition);
        newPos.y = Needle.transform.localPosition.y;

        Needle.transform.localPosition = newPos;
    }
}
