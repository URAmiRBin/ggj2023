using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    public bool followPlayer = true;
    public float zoom = 2;
    public Vector2 offset = new Vector3(1, 0);

    GameManager _gameManager;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _gameManager.followPlayer = followPlayer;
            _gameManager.camZoom = zoom;
            _gameManager.camOffset = offset;
        }
    }
}
