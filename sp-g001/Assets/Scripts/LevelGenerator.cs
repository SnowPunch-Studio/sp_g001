using System;
using UnityEngine;
public class LevelGenerator : MonoBehaviour
{
    public Texture2D map;
    public ColorToPrefab[] colorMappings;
    public GameObject groundTile;

    void Start()
    {
        GenerateLevel();
        GameObject.FindWithTag("MainCamera").GetComponent<CameraController>().InitCamera();
    }

    void GenerateLevel()
    {
        int prefabPositionX = 0;
        int prefabPositionY = 0;
        int prefabPositionZ = 0;
        for (int levelMapX = 0; levelMapX < map.width; levelMapX++)
        {
            if (levelMapX == 20)
            {
                prefabPositionX = 0;
                prefabPositionY = 1;
            }
            else if (levelMapX == 40)
            {
                prefabPositionX = 0;
                prefabPositionY = 2;
            }
            for (int levelMapZ = 0; levelMapZ < map.height; levelMapZ++)
            {
                prefabPositionZ = levelMapZ;
                GenerateTile(levelMapX, levelMapZ, prefabPositionX, prefabPositionY, prefabPositionZ);
            }
            prefabPositionX++;
        }
    }

    void GenerateTile(int levelMapX, int levelMapZ, int prefabPositionX, int prefabPositionY, int prefabPositionZ)
    {
        Color pixelColor = map.GetPixel(levelMapX, levelMapZ);
        if (pixelColor.a == 0)
        {
            return;
        }
        Debug.Log("Pixel Color :: " + pixelColor);

        foreach (ColorToPrefab colorMapping in colorMappings)
        {
            Debug.Log("Mapping Color :: " + colorMapping.color);
            Debug.Log("Are equal :: " + (pixelColor == colorMapping.color));

            if (Math.Round(colorMapping.color.r, 3) == Math.Round(pixelColor.r, 3) && Math.Round(colorMapping.color.g, 3) == Math.Round(pixelColor.g, 3) && Math.Round(colorMapping.color.b, 3) == Math.Round(pixelColor.b, 3))
            {
                Debug.Log("Creating prefab for :: " + pixelColor);
                Vector3 position = new Vector3(prefabPositionX, prefabPositionY, prefabPositionZ);
                Instantiate(colorMapping.prefab, position, Quaternion.identity, transform);
                return;
            }
        }
    }
}