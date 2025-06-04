using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace UnityUtilities.Runtime.ServiceLocator
{
    public class MockLocalizationServices : ILocalization
    {
        readonly List<string> _words = new List<string>() { "hund", "katt", "fisk", "barn", "hus" };
        private readonly System.Random random = new Random();

        public string GetLocalizedWord(string word)
        {
            return _words[random.Next(_words.Count)];
        }
    }

    public class MockSerializer : ISerializer
    {
        public void Serialize()
        {
            Debug.Log("MockSerializer.Serialize");
        }
    }

    public class MockGameService : IGameService
    {
        private readonly string _name;

        public MockGameService(string name)
        {
            this._name = name;
        }

        public void Start()

        {
            Debug.Log($"MockGameService.Start {_name}");
        }
    }


    public class MockMapService : IGameService
    {
        public void Start()
        {
            UnityEngine.Debug.Log(" MockMapService.Start");
        }
    }

    public interface ILocalization
    {
        string GetLocalizedWord(string word);
    }

    public interface ISerializer
    {
        void Serialize();
    }

    public interface IAudioService
    {
        void Play();
    }

    public interface IGameService
    {
        void Start();
    }
}