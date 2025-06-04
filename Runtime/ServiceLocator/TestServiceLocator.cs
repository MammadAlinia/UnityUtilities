using System;
using UnityEngine;
using UnityUtilities.Runtime.ServiceLocator;

public class TestServiceLocator : MonoBehaviour
{
    IGameService gameService;
    ILocalization localization;
    ISerializer serializer;


    private void Awake()
    {
        ServiceLocator.Global.Register<ILocalization>(localization = new MockLocalizationServices());
        ServiceLocator.ForSceneOf(this).Register<IGameService>(gameService = new MockGameService("scene defult"));
        ServiceLocator.For(this).Register<ISerializer>(serializer = new MockSerializer());
    }

    private void Start()
    {
        ServiceLocator.For(this)
            .Get(out serializer)
            .Get(out gameService)
            .Get(out localization);


        serializer.Serialize();
        gameService.Start();
        Debug.Log(localization.GetLocalizedWord("sd"));
    }
}