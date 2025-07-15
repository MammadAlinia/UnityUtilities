using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityUtilities.Runtime.Events
{
    public class ContextBinder
    {
        public object Context;

        public ContextBinder(object context)
        {
            Context = context;
        }

        public ContextBinder SubscribeToEvent<T>(Action<T> command) where T : IEventCommand
        {
            EventBus.SubscribeToEvent(command, Context);
            return this;
        }

        public void PublishEvent<T>(T command) where T : IEventCommand
        {
            EventBus.PublishEvent(command, Context);
        }
    }

    public static class EventBus
    {
        private static Dictionary<Type, Dictionary<object, List<Action<IEventCommand>>>> _events =
            new Dictionary<Type, Dictionary<object, List<Action<IEventCommand>>>>();

        public static object GlobalContext = new object();

        public static ContextBinder ForContext(object context)
        {
            return new ContextBinder(context);
        }


        public static void SubscribeToEvent<T>(Action<T> command, object context = null) where T : IEventCommand
        {
            context ??= GlobalContext;

            if (!_events.ContainsKey(typeof(T)))
            {
                _events[typeof(T)] = new Dictionary<object, List<Action<IEventCommand>>>();
            }

            if (!_events[typeof(T)].TryGetValue(context, out var bindings))
            {
                bindings = new List<Action<IEventCommand>>();
                _events[typeof(T)][context] = bindings;
            }

            bindings.Add(e => command((T)e));
        }

        public static void Unsubscribe<T>(Action<IEventCommand> command)
        {
            _events[typeof(T)].Remove(command);
        }

        public static void PublishEvent(IEventCommand command, object context = null)
        {
            context ??= GlobalContext;

            if (!_events.ContainsKey(command.GetType()))
            {
                return;
            }

            if (_events[command.GetType()].TryGetValue(context, out var bindings))
            {
                foreach (var binding in bindings)
                {
                    try
                    {
                        binding(command);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }
        }


        public static void Initialize()
        {
            _events.Clear();

        }

    }
}