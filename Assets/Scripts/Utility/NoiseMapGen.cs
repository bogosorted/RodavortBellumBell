using UnityEngine;
using UnityEngine.UI;

public class NoiseMapGen : MonoBehaviour
{
    private Texture2D GenerateMap(int width, int heigth, int yMult, int xMult)
    {
        Texture2D finalTexture = new Texture2D(width, heigth);
        Color[] pixelColor = new Color[width * heigth];
        finalTexture = new Texture2D(width, heigth);

        for (int y = 0; y < heigth; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var color = Mathf.PerlinNoise(x * xMult, y * yMult);
                pixelColor[y * width + x] = new Color(color, color, color);
            }
        }

        finalTexture.SetPixels(pixelColor);
        finalTexture.Apply();

        return finalTexture;
    }
}
