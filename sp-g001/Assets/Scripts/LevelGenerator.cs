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
        int yPlace = 0;
        int xPlace = 0, zPlace = 0;
        for (int x = 0; x < map.width; x++)
        {
            if (xPlace == 20)
            {
                xPlace = 0;
                yPlace = 1;
            }
            else if (xPlace == 40)
            {
                xPlace = 0;
                yPlace = 2;
            }
            for (int z = 0; z < map.height; z++)
            {
                zPlace = z;
                GenerateTile(x, z, xPlace, yPlace, zPlace);
            }
            xPlace++;
        }
    }
    void GenerateTile(int x, int z, int xPlace, int yPlace, int zPlace)
    {
        Color pixelColor = map.GetPixel(x, z);
        Debug.Log(pixelColor);
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
                /*if (pixelColor.a == 1 && colorMapping.prefab != groundTile)
                {
                    //Debug.Log("")
                    // Block with ground
                    Vector3 groundPosition = new Vector3(xPlace, yPlace, zPlace);
                    Instantiate(groundTile, groundPosition, Quaternion.identity, transform);
                }*/
                //Debug.Log("Mapping Color :: " + colorMapping.color);
                //Debug.Log("Color matches");
                Vector3 position = new Vector3(xPlace, yPlace, zPlace);
                /*if (pixelColor.r == 0 && pixelColor.g == 0 && pixelColor.b == 0)
                {
                    position.y = -3;
                }
                else if (pixelColor.r == 0 && pixelColor.g == 77 && pixelColor.b == 255)
                {
                    position.y = 0;
                }*/
                //Debug.Log("Generating tile");
                Instantiate(colorMapping.prefab, position, Quaternion.identity, transform);
                return;
            }
        }
    }
}