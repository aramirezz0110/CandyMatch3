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
    private Candy selectedCandy;

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

        int idx = -1;
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
                do
                {
                    idx = GetRandomIndex();
                } while ((x>0 && idx==candies[x-1,y].GetComponent<Candy>().id) ||
                    (y>0 && idx == candies[x, y-1].GetComponent<Candy>().id));
                
                Candy tempCandy = newCandy.GetComponent<Candy>();
                tempCandy.SetSprite(prefabs[idx]);
                tempCandy.id = idx;

                newCandy.transform.SetParent(this.transform);
                candies[x, y] = newCandy;
            }
        }
    }
    private int GetRandomIndex()=> Random.Range(0, prefabs.Count);
    #endregion
}
