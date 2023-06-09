using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;

    [Header("REFERENCES")]
    public List<Sprite> prefabs = new List<Sprite>();
    public Transform candyParent;
    public GameObject currentCandy;
    public Vector2Int size;


    private GameObject[,] candies;
    public bool isShifting { get; set; }
    public const int MinCandiesToMatch = 2;//Means min neighbors to match
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

                newCandy.transform.SetParent(candyParent);
                candies[x, y] = newCandy;
            }
        }
    }
    private int GetRandomIndex()=> Random.Range(0, prefabs.Count);
    private IEnumerator MakeCandiesFall(int x, int yStart, float shiftDelay = 0.02f)
    {
        isShifting= true;

        List<SpriteRenderer> renderes = new List<SpriteRenderer>();
        int nullCandies = 0;
        //Figure out how many candies visuals are null
        for(int y = yStart; y < size.y; y++)
        {
            SpriteRenderer spriteRenderer = candies[x, y].GetComponent<SpriteRenderer>();
            if (!spriteRenderer.sprite)
            {
                nullCandies++;
            }
            renderes.Add(spriteRenderer);
        }
        //Fill candies in null positions
        for (int i=0; i<nullCandies; i++)
        {
            for (int j=0; j<renderes.Count-1; j++)
            {
                //UI
                UIManager.Instance.AddScore(10);

                yield return new WaitForSeconds(shiftDelay);
                renderes[j].sprite = renderes[j + 1].sprite;
                renderes[j + 1].sprite= GetNewCandy(x, size.y-1);
            }
        }
        isShifting= false;
    }
    private Sprite GetNewCandy(int x, int y)
    {
        List<Sprite> possibleCandies = new List<Sprite>();
        possibleCandies.AddRange(prefabs);
        if (x > 0)
        {
            possibleCandies.Remove(candies[x-1, y].GetComponent<SpriteRenderer>().sprite);
        }
        if (x <size.x-1)
        {
            possibleCandies.Remove(candies[x+1, y].GetComponent<SpriteRenderer>().sprite);
        }
        if (y > 0)
        {
            possibleCandies.Remove(candies[x, y-1].GetComponent<SpriteRenderer>().sprite);
        }
        if (y <size.y-1)
        {
            possibleCandies.Remove(candies[x, y+1].GetComponent<SpriteRenderer>().sprite);
        }
        return possibleCandies[Random.Range(0, possibleCandies.Count)];
    }
    #endregion

    #region PUBLIC METHODS
    public IEnumerator FindNullCandies()
    {
        for (int x=0; x< size.x; x++)
        {
            for(int y=0; y< size.y; y++)
            {
                if (!candies[x, y].GetComponent<SpriteRenderer>().sprite)
                {
                    yield return StartCoroutine(MakeCandiesFall(x,y));
                    break;
                }
            }
        }
        //Check if new candies have coincidences
        for(int x=0; x<size.x; x++)
        {
            for(int y=0; y<size.y; y++)
            {
                candies[x, y].GetComponent<Candy>().FindAllMatches();
            }
        }
    }
    
    #endregion
}
