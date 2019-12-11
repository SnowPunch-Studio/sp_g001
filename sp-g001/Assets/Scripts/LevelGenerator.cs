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

        foreach (ColorToPrefab colorMapping in colorMappings)
        {
            if (colorMapping.color.r == pixelColor.r && colorMapping.color.g == pixelColor.g && colorMapping.color.b == pixelColor.b)
            {
                Vector3 position = new Vector3(prefabPositionX, prefabPositionY, prefabPositionZ);
                Instantiate(colorMapping.prefab, position, Quaternion.identity, transform);
                return;
            }
        }
    }
}