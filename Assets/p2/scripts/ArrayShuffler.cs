using UnityEngine;
using System; // Required for Random class

public class ArrayShuffler : MonoBehaviour
{
    /// <summary>
    /// Shuffles an array using the Fisher-Yates (Knuth) shuffle algorithm.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="array">The array to be shuffled.</param>
    public static void Shuffle<T>(T[] array)
    {
        // Use Unity's Random class for consistency within Unity projects
        // Alternatively, System.Random can be used for non-Unity specific randomness
        System.Random rng = new System.Random(); 

        int n = array.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1); // Get a random index from 0 to n
            T value = array[k];      // Store the value at the random index
            array[k] = array[n];     // Move the last unshuffled element to the random index
            array[n] = value;        // Place the stored value at the current end of the unshuffled portion
        }
    }
}