using TMPro;
using UnityEngine;

public class TitleAnimation : MonoBehaviour
{
    public TMP_Text titleText;
    public float zRotateMax = 2;
    public float rotateSpeed = 0.0003f;
    private bool reverse = false;
    public Quaternion rot;
    public Vector3 scale;

    // Update is called once per frame
    void Update()
    {
        rot = titleText.rectTransform.rotation;
        scale = titleText.rectTransform.localScale;


        if (!reverse)
        {
            if((rot.z*100) <= zRotateMax) 
            {
                rot.z += rotateSpeed;
                scale += (Vector3.one * rotateSpeed * .3f);
                titleText.rectTransform.localRotation = rot;
                titleText.rectTransform.localScale = scale;
            }
            else
            {
                reverse = !reverse;
            }
        }
        else
        {
            if ((rot.z * 100) >= -zRotateMax)
            {
                rot.z -= rotateSpeed;
                scale -= (Vector3.one * rotateSpeed * .3f);
                titleText.rectTransform.localRotation = rot;
                titleText.rectTransform.localScale = scale;
            }
            else
            {
                reverse = !reverse;
            }
        }
    }
}
