using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;


public class SequenceImage_MeshRenderer : MonoBehaviour
{
    public enum ImageExtended
    {
        PNG,
        JPG
    }

    [SerializeField]
    private Renderer renderer;

    [SerializeField, Header("連番画像の拡張子")]
    private ImageExtended imageExtended;

    [SerializeField, Header("Assets/Resources/XXX/temp.pngのXXX")]
    private string folder;

    [SerializeField, Header("何秒で切り替えるか"), Min(0.0f)]
    private float changeFrameSecond;

    private Texture[] images;
    private int imageLength;
    private int counter;

    // Start is called before the first frame update
    void Start()
    {
        counter = 0;

        string[] imageNames = Directory.GetFiles(Application.dataPath + @"/Resources/" + folder, "*" + GetExtended())
            .Select(x => x.Replace(GetExtended(), ""))
            .ToArray();
        imageLength = imageNames.Length;
        images = new Texture[imageLength];
        for (int i = 0; i < imageLength; i++)
        {
            string fileName = Path.GetFileName(imageNames[i]);
            images[i] = Resources.Load<Texture>(folder + "/" + fileName);
        }
        StartCoroutine("Sequence");
    }

    IEnumerator Sequence()
    {
        renderer.sharedMaterial.SetTexture("_MainTex",images[counter]);
        while (true)
        {
            yield return new WaitForSeconds(changeFrameSecond);
            counter++;
            counter %= imageLength;
            renderer.sharedMaterial.SetTexture("_MainTex", images[counter]);
        }
    }

    private string GetExtended()
    {
        switch (imageExtended)
        {
            case ImageExtended.JPG:
                return ".jpg";
            case ImageExtended.PNG:
                return ".png";
            default:
                return "nothing";
        }
    }
}
