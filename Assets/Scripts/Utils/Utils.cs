using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Trout.Utils
{
    public class Utils
    {
        /// <summary>
        /// Removes all children of the passed gameobject
        /// </summary>
        /// <param name="container">The gameobject which holds the objects to remove</param>
        public static void ClearChildren(GameObject container)
        {
            int childCount = container.transform.childCount;

            for (int i = childCount - 1; i >= 0; i--)
            {
                GameObject child = container.transform.GetChild(i).gameObject;
                RemoveObject(child);
            }
        }

        /// <summary>
        /// Finds the object with the passed name and removes it from the game
        /// </summary>
        public static void RemoveObjectByName(string name)
        {
            GameObject obj = GameObject.Find(name);
            RemoveObject(obj);
        }



        /// <summary>
        /// Removes an object from the game
        /// </summary>
        public static void RemoveObject(GameObject obj)
        {

            if (Application.isPlaying)
            {
                GameObject.Destroy(obj);
            }
            else
            {
                GameObject.DestroyImmediate(obj);
            }
        }

        /// <summary>
        /// Returns the difference between the max and the min value of the passed array
        /// </summary>
        public static float GetDifference(float[] values)
        {
            return values.Max() - values.Min();
        }


        /// <summary>
        /// Returns the average value of the list of values
        /// </summary>
        /// <param name="values">array of floats</param>
        public static float GetAverage(float[] values)
        {
            float[] minMax = {
                values.Max(),
                values.Min(),
            };

            float avg = 0;
            foreach (float val in minMax)
            {
                avg += val;
            };

            return avg /= minMax.Length;
        }

        /// <summary>
        /// Groups all vectors who are clustered /chained together. Vectors are chained together if their position is within the threshold of each other.
        /// </summary>
        /// <param name="vectors">The list of vectors to group together</param>
        /// <param name="threshold">The distance betweeen each vector in which they are to be considered a group</param>
        public static List<List<Tile>> GroupAllChainedTiles(List<Tile> tiles, float threshold)
        {
            void DFS(Tile tile, Dictionary<Vector3, List<Tile>> neighbors, HashSet<Vector3> visited, List<Tile> group)
            {
                visited.Add(tile.position);
                group.Add(tile);

                foreach (Tile neighbor in neighbors[tile.position])
                {
                    if (!visited.Contains(neighbor.position))
                    {
                        DFS(neighbor, neighbors, visited, group);
                    }
                }
            }

            bool IsVectorWithinThreshold(Vector3 vectorA, Vector3 vectorB)
            {
                float distanceThreshold = threshold; // Adjust the threshold as needed
                float distance = Vector3.Distance(vectorA, vectorB);
                return distance <= distanceThreshold;
            }

            Dictionary<Vector3, List<Tile>> neighbors = new Dictionary<Vector3, List<Tile>>();
            for (int i = 0; i < tiles.Count; i++)
            {
                Vector3 vector = tiles[i].position;
                neighbors[vector] = new List<Tile>();

                for (int j = 0; j < tiles.Count; j++)
                {
                    if (i != j && IsVectorWithinThreshold(vector, tiles[j].position))
                    {
                        neighbors[vector].Add(tiles[j]);
                    }
                }
            }


            HashSet<Vector3> visited = new HashSet<Vector3>();
            List<List<Tile>> groupedTiles = new List<List<Tile>>();

            foreach (Tile tile in tiles)
            {
                if (!visited.Contains(tile.position))
                {
                    List<Tile> group = new List<Tile>();
                    DFS(tile, neighbors, visited, group);
                    groupedTiles.Add(group);
                }
            }

            return groupedTiles;
        }
    }
}