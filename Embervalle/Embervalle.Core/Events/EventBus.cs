using System;
using System.Collections.Generic;

namespace Embervalle.Core.Events
{
    /// <summary>Publicação/assinatura em memória (padrão observer) para desacoplar sistemas.</summary>
    public static class EventBus
    {
        private static readonly Dictionary<Type, List<Delegate>> Subscribers = new();

        /// <summary>Regista um handler para o tipo de evento <typeparamref name="T"/>.</summary>
        public static void Subscribe<T>(Action<T> handler)
            where T : class
        {
            Type t = typeof(T);
            if (!Subscribers.TryGetValue(t, out List<Delegate>? list))
            {
                list = new List<Delegate>();
                Subscribers[t] = list;
            }

            list.Add(handler);
        }

        /// <summary>Notifica todos os subscritores de <typeparamref name="T"/> com a instância dada.</summary>
        public static void Publish<T>(T evt)
            where T : class
        {
            Type t = typeof(T);
            if (!Subscribers.TryGetValue(t, out List<Delegate>? list))
            {
                return;
            }

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] is Action<T> action)
                {
                    action(evt);
                }
            }
        }
    }
}
