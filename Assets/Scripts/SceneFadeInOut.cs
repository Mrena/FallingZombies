using UnityEngine;
using System.Collections;

public class SceneFadeInOut : MonoBehaviour
{

    public float fadeSpeed = 0.75f;

    private bool sceneStarting = true;

     void Awake()
    {
        guiTexture.pixelInset = new Rect(0,0,Screen.width,Screen.height);

    }

     void Update()
    {
         if (sceneStarting)
         {
             StartScene();
         }
    }

    private void FadeToClear()
    {
        guiTexture.color = Color.Lerp(guiTexture.color, Color.clear, fadeSpeed*Time.deltaTime);

    }

    private void FadeToBlack()
    {
        guiTexture.color = Color.Lerp(guiTexture.color, Color.black, fadeSpeed*Time.deltaTime);
    }

    private void StartScene()
    {
        FadeToClear();

        if (guiTexture.color.a <= 0.02f)
        {
            guiTexture.color = Color.clear;
            guiTexture.enabled = false;
            GameState.sceneStarting = sceneStarting = false;
        }

    }

    public void EndScene()
    {
        guiTexture.enabled = true;
        FadeToBlack();

        if (guiTexture.color.a >= 0.95f)
        {
            guiTexture.color = Color.black;
        }
    }


}
