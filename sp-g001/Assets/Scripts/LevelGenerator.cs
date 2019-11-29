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
        if (pixelColor.a == 0)
        {
            return;
        }

        foreach (ColorToPrefab colorMapping in colorMappings)
        {
            if (colorMapping.color.r == pixelColor.r && colorMapping.color.g == pixelColor.g && colorMapping.color.b == pixelColor.b)
            {
                Vector3 position = new Vector3(xPlace, yPlace, zPlace);
                Instantiate(colorMapping.prefab, position, Quaternion.identity, transform);
                return;
            }
        }
    }
}