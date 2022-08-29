using System.Collections.Generic;
using UnityEngine;

namespace llagache
{
    public static class RandomExtensions
    {
        public static List<T> Shuffle<T>(this IList<T> list)
        {
            List<T> newList = new List<T>();

            while (list.Count > 0)
            {
                T item = list[Random.Range(0, list.Count)];
                list.Remove(item);

                newList.Add(item);
            }

            return newList;
        }

        public static T RandomItem<T>(this List<T> list)
        {
            if (list.Count == 0)
                throw new System.IndexOutOfRangeException("Cannot select a random item from an empty list");
            return list[Random.Range(0, list.Count)];
        }

        public static T RandomItem<T>(this T[] array)
        {
            if (array.Length == 0)
                throw new System.IndexOutOfRangeException("Cannot select a random item from an empty list");
            return array[Random.Range(0, array.Length)];
        }

        public static T RemoveRandom<T>(this List<T> list)
        {
            if (list.Count == 0)
                throw new System.IndexOutOfRangeException("Cannot remove a random item from an empty list");
            int index = Random.Range(0, list.Count);
            T item = list[index];
            list.RemoveAt(index);
            return item;
        }

        public static void RandomResetState()
        {
            Random.InitState(System.Environment.TickCount);
        }

        public static float RandomWeightedRange(float min, float max, AnimationCurve weight)
        {
            float t = Random.value;
            return weight.Evaluate(t) * (max - min) + min;
        }

    }
}