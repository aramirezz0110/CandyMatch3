using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
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
    #endregion
    #region PUBLIC METHODS
    public void SetSprite(Sprite sprite)=> spriteRenderer.sprite = sprite;
    #endregion
}
