using JSAM;
using UnityEngine;

public class TerrainTypeDetector : MonoBehaviour
{
    private Terrain terrain;
    private TerrainData terrainData;

    private void Start()
    {
        terrain = Terrain.activeTerrain;
        terrainData = terrain.terrainData;
    }

    public void PlayFootstepSound()
    {
        Vector3 playerPosition = transform.position;
        Vector3 terrainLocalPos = terrain.transform.InverseTransformPoint(playerPosition);
        int terrainTextureIndex = GetMainTextureIndex(terrainLocalPos);

        if (terrainTextureIndex >= 0 && terrainTextureIndex < terrainData.alphamapLayers)
        {
            string terrainType = terrainData.splatPrototypes[terrainTextureIndex].texture.name;

            if (terrainType.Contains("Sand"))
            {
                JSAM.AudioManager.StopSound(Sounds.StoneSteps);
                JSAM.AudioManager.PlaySound(Sounds.SandSteps);
            }
            else if (terrainType.Contains("Stone"))
            {
                JSAM.AudioManager.StopSound(Sounds.SandSteps);
                JSAM.AudioManager.PlaySound(Sounds.StoneSteps);
            }
            else
            {
                Debug.LogWarning("for the terrain " + terrainType + " was no sound assigned");
            }
        }
    }


    private int GetMainTextureIndex(Vector3 terrainLocalPos)
    {
        // Convert terrain local position to the texture coordinates
        float mapX = terrainLocalPos.x / terrainData.size.x;
        float mapZ = terrainLocalPos.z / terrainData.size.z;

        // Get the alphamap coordinate
        int coordX = Mathf.FloorToInt(mapX * terrainData.alphamapWidth);
        int coordZ = Mathf.FloorToInt(mapZ * terrainData.alphamapHeight);

        // Get the texture index with the highest weight at the coordinate
        float[,,] alphaMaps = terrainData.GetAlphamaps(coordX, coordZ, 1, 1);
        float[] weights = new float[terrainData.alphamapLayers];
        for (int i = 0; i < terrainData.alphamapLayers; i++)
        {
            weights[i] = alphaMaps[0, 0, i];
        }
        return GetMaxIndex(weights);
    }

    private int GetMaxIndex(float[] array)
    {
        float max = 0f;
        int maxIndex = 0;
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] > max)
            {
                max = array[i];
                maxIndex = i;
            }
        }
        return maxIndex;
    }
}


























