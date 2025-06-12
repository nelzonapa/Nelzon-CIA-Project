using UnityEngine;
public class ImageBoardManager : MonoBehaviour
{
    public Texture2D[] textures;       // 7 texturas
    public GameObject boardPrefab;     // Prefab ImageBoard
    public Vector3[] positions;        // 7 posiciones en la escena

    void Start()
    {
        for (int i = 0; i < textures.Length; i++)
        {
            var board = Instantiate(boardPrefab, positions[i], Quaternion.identity, transform);
            var mat = new Material(Shader.Find("Unlit/Texture")) { mainTexture = textures[i] };
            board.GetComponent<MeshRenderer>().material = mat;
        }
    }
}