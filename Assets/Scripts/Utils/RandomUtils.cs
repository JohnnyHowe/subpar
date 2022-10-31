using UnityEngine;
using System;


namespace Utils {
    public class RandomUtils {
        public static T RandomChoice<T>(T[] items) {
            if (items.Length == 0) {
                throw new ArgumentException("Items list for random choice empty");
            }
            return items[Mathf.Min(items.Length - 1, UnityEngine.Random.Range(0, items.Length))];
        }
        
        public static T RandomWeightedChoice<T>(T[] items, float[] weights) {
            if (items.Length == 0) {
                throw new ArgumentException("Items list for random choice empty");
            }

            if (items.Length != weights.Length) {
                throw new ArgumentException("Number of weights and items should be equal");
            }

            float totalWeight = 0;
            foreach (float weight in weights) totalWeight += weight;

            float r = Mathf.Min(UnityEngine.Random.value, 0.999f) * totalWeight;

            float rollingSum = 0;
            for (int i = 0; i < items.Length; i++) {
                rollingSum += weights[i];
                if (rollingSum > r) {
                    return items[i];
                }
            }
            return items[items.Length - 1];
        }

        public static bool RandomChance(float chance=0.5f) {
            return Mathf.Min(UnityEngine.Random.value, 0.999f) < chance;
        }

        /// <summary>
        /// [0, 1)
        /// </summary>
        /// <returns></returns>
        public static float Value() {
            return Mathf.Min(UnityEngine.Random.value, 0.9999999f);
        }

        public static int RandomInt(int minInclusive, int maxInclusive) {
            return Mathf.FloorToInt(Value() * (maxInclusive - minInclusive + 1)) + minInclusive;
        } 
    }
}