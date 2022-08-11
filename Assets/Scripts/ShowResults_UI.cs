using UnityEngine;
using UnityEngine.UI;

public class ShowResults_UI : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private RawImage showImage;
    [SerializeField] private Animator animOfShowImage;
    [Space(10)]

    [Header("Texture")]
    [SerializeField] private Texture wonImage;
    [SerializeField] private Texture loseImage;

    public void ShowWin()
    {
        animOfShowImage.SetBool("ShowResult", true);
        showImage.texture = wonImage;
    }

    public void ShowLose()
    {
        animOfShowImage.SetBool("ShowResult", true);
        showImage.texture = loseImage;
    }
}
