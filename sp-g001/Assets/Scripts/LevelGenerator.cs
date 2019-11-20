using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    public Texture2D map;

    public ColorToPrefab[] colorMappings;
    public GameObject groundTile;

    // Use this for initialization
    void Start()
    {
        GenerateLevel();
        GameObject.FindWithTag("MainCamera").GetComponent<CameraController>().InitCamera();
    }

    void GenerateLevel()
    {
        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                GenerateTile(x, y);
            }
        }
    }

    void GenerateTile(int x, int y)
    {
        Color pixelColor = map.GetPixel(x, y);

        if (pixelColor.a == 0)
        {
            // The pixel is transparrent. Let's ignore it!
            return;
        }

        //Debug.Log("Pixel Color :: " + pixelColor);

        foreach (ColorToPrefab colorMapping in colorMappings)
        {
            //colorMapping.color.Equals(pixelColor)
            if (colorMapping.color.r == pixelColor.r && colorMapping.color.g == pixelColor.g && colorMapping.color.b == pixelColor.b)
            {
                //Debug.Log("Pixel Alpha :: " + pixelColor.a);
                if(pixelColor.a == 1 && colorMapping.prefab != groundTile)
                {
                    //Debug.Log("")
                    // Block with ground 
                    Vector3 groundPosition = new Vector3(x, -3, y);
                    Instantiate(groundTile, groundPosition, Quaternion.identity, transform);
                }

                //Debug.Log("Mapping Color :: " + colorMapping.color);
                //Debug.Log("Color matches");
                Vector3 position = new Vector3(x, -2.5f, y);
                if (pixelColor.r == 0 && pixelColor.g == 0 && pixelColor.b == 0)
                {
                    position.y = -3;
                } else if(pixelColor.r == 0 && pixelColor.g == 77 && pixelColor.b == 255)
                {
                    position.y = 0;
                }

                //Debug.Log("Generating tile");
                Instantiate(colorMapping.prefab, position, Quaternion.identity, transform);
                return;
            }
        }
    }

}
