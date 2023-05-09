using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Candy : MonoBehaviour
{
    private static Color selectedColor = new Color(0.5f, .5f, .5f, 1f);
    private static Candy previousSelected = null;

    private SpriteRenderer spriteRenderer;
    private bool isSelected = false;

    public int id;

    private Vector2[] adjacentDirections = new Vector2[] 
    {
        Vector2.up,
        Vector2.down, 
        Vector2.left,
        Vector2.right,
    };

    #region UNITY METHODS
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnMouseDown()
    {
        if (!spriteRenderer.sprite || BoardManager.Instance.isShifting) return;

        if(isSelected)
            DeselectCandy();
        else
        {
            if(!previousSelected)
                SelectCandy();
            else
            {
                if (CanSwipe())
                {
                    SwapSprite(previousSelected);
                    previousSelected.FindAllMatches(); //in case the other causes the destruction
                    previousSelected.DeselectCandy();
                    FindAllMatches(); //in case the current candy causes the destruction
                    //UI
                    UIManager.Instance.MoveCounter--;
                }
                else
                {
                    previousSelected.DeselectCandy();
                    SelectCandy();
                }
            }
        }
    }
    #endregion
    #region PUBLIC METHODS
    public void SetSprite(Sprite sprite)=> spriteRenderer.sprite = sprite;
    public void SwapSprite(Candy newCandy)
    {
        SpriteRenderer newSpriteRenderer = newCandy.GetComponent<SpriteRenderer>();
        if (spriteRenderer.sprite == newSpriteRenderer.sprite)
            return;

        Sprite oldCandy = newCandy.spriteRenderer.sprite;
        newCandy.spriteRenderer.sprite = this.spriteRenderer.sprite;
        this.spriteRenderer.sprite = oldCandy;

        int oldId = newCandy.id;
        newCandy.id = this.id;
        this.id = oldId;
    }
    public void FindAllMatches()
    {
        if (!spriteRenderer.sprite) return;

        bool horizontalMatch = ClearMatch(new Vector2[2]
        {
            Vector2.left, Vector2.right
        });
        bool verticalMatch = ClearMatch(new Vector2[2]
        {
            Vector2.up, Vector2.down
        });

        if(horizontalMatch || verticalMatch)
        {
            //disable candy self visuals
            spriteRenderer.sprite = null;
            StopCoroutine(BoardManager.Instance.FindNullCandies());
            StartCoroutine(BoardManager.Instance.FindNullCandies());
        }
    }
    #endregion
    #region PRIVATE METHODS
    private void SelectCandy()
    {
        isSelected = true;
        spriteRenderer.color = selectedColor;
        previousSelected= this;
    }
    private void DeselectCandy()
    {
        isSelected= false;
        spriteRenderer.color = Color.white;
        previousSelected= null;
    }
    private GameObject GetNeighbor(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction); //throw ray
        if (hit.collider)
            return hit.collider.gameObject;
        else
            return null;
    }
    private List<GameObject> GetAllNeighbors()
    {
        List<GameObject> neighbors = new List<GameObject>();
        foreach (Vector2 direction in adjacentDirections)
        {
            neighbors.Add(GetNeighbor(direction));
        }
        return neighbors;
    }
    private bool CanSwipe() => GetAllNeighbors().Contains(previousSelected.gameObject);
    private List<GameObject> FindMatch(Vector2 direction)
    {
        List<GameObject> matchingCandies = new List<GameObject>();

        //Check neighbors in parameter direction
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction);
        while (hit.collider && (hit.collider.GetComponent<SpriteRenderer>().sprite == spriteRenderer.sprite))
        {
            matchingCandies.Add(hit.collider.gameObject);
            hit = Physics2D.Raycast(hit.collider.transform.position, direction);
        }

        return matchingCandies;
    }
    private bool ClearMatch(Vector2[] directions)
    {
        List<GameObject> matchingCandies = new List<GameObject>();
        foreach (Vector2 direction in directions)
        {
            matchingCandies.AddRange(FindMatch(direction));
        }
        if(matchingCandies.Count>= BoardManager.MinCandiesToMatch)
        {
            foreach (GameObject candy in matchingCandies)
            {
                //disable candy visuals
                candy.GetComponent<SpriteRenderer>().sprite = null;
            }
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
}
