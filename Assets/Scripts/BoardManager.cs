using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;

    [Header("REFERENCES")]
    public List<Sprite> prefabs = new List<Sprite>();
    public GameObject currentCandy;
    public Vector2Int size;

    private GameObject[,] candies;
    public bool isShifting { get; set; }

    #region UNITY METHODS
    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        Vector2 offset = currentCandy.GetComponent<BoxCollider2D>().size;
        CreateInitialBoard(offset);
    }
    #endregion

    #region PRIVATE METHODS
    private void CreateInitialBoard(Vector2 offset)
    {
        candies = new GameObject[size.x, size.y];

        float startX = this.transform.position.x;
        float startY = this.transform.position.y;

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                GameObject newCandy = Instantiate(
                    currentCandy,
                    new Vector3(startX+(offset.x*x), startY+(offset.y*y), 0),
                    Quaternion.identity
                    );
                newCandy.name = $"Candy[{x}][{y}]";                
                Candy tempCandy = newCandy.GetComponent<Candy>();
                tempCandy.SetSprite(GetRandomSprite());
                candies[x, y] = newCandy;
            }
        }
    }
    private Sprite GetRandomSprite()=> prefabs[Random.Range(0, prefabs.Count)];
    #endregion
}
