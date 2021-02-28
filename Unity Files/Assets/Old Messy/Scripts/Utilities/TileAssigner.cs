using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

/*
 * Helper class to auto assign tile material and script to all tile node objects
 * TODO: DELETE IF NEVER USED 
 */
namespace Utilities
{
    public class TileAssigner
    {
        // [MenuItem("Tools/Assign Material")]
        // public static void AssignTileMaterial()
        // {
        //     GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        //     Material material = Resources.Load<Material>("Tile");
        //
        //     foreach (GameObject tile in tiles)
        //         tile.GetComponent<Renderer>().material = material;
        // }
        //
        // [MenuItem("Tools/Assign Tile Script")]
        // public static void AssignTileScript()
        // {
        //     GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        //
        //     foreach (GameObject tile in tiles)
        //         tile.AddComponent<Tile>();
        // }
    }
}