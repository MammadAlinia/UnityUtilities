using UnityEngine;
using UnityUtilities.Runtime.ServiceLocator;

public class TestServiceLocator2 : MonoBehaviour
{
    private IGameService _gameService;

    private void Awake()
    {
        ServiceLocator.ForSceneOf(this).Register(_gameService = new MockGameService("scene 2"));
    }

    private void Start()
    {
        ServiceLocator.For(this).Get(out _gameService);
        _gameService.Start();
    }
}