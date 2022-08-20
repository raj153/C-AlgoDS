using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlGoLiv.ProbSol.Logic
{
    public static class Logic
    {
        public static int FetchRainWaterTrapBetweenTowers(int[] towerHeight)
        {
            int rainWater = 0;

            int[] maxSeenRight = new int[towerHeight.Length];
            int[] maxSeenLeft = new int[towerHeight.Length];
            int maxSeenSoFar = 0;

            for(int i=towerHeight.Length-1; i>=0; i--)
            {
                if(towerHeight[i] > maxSeenSoFar)
                {
                    maxSeenSoFar = towerHeight[i];                    
                }
                maxSeenRight[i] = maxSeenSoFar;
                
            }
            maxSeenSoFar = 0;
            for(int i=0; i<towerHeight.Length; i++)
            {
                if (towerHeight[i] > maxSeenSoFar)
                {
                    maxSeenSoFar = towerHeight[i];
                }
                maxSeenLeft[i] = maxSeenSoFar;

            }       

            for(int i=0; i<towerHeight.Length; i++)
            {
                rainWater += Math.Max(Math.Min(maxSeenLeft[i], maxSeenRight[i])-towerHeight[i],0);
            }

            return rainWater;
        }
        public static int FetchRainWaterTrapBetweenTowersOpt(int[] towerHeight)
        {
            int rainWater = 0;

            int[] maxSeenRight = new int[towerHeight.Length];
            int[] maxSeenLeft = new int[towerHeight.Length];
            int maxSeenSoFar = 0;

            for (int i = towerHeight.Length - 1; i >= 0; i--)
            {
                if (towerHeight[i] > maxSeenSoFar)
                {
                    maxSeenSoFar = towerHeight[i];
                }
                maxSeenRight[i] = maxSeenSoFar;

            }
            maxSeenSoFar = 0;
            for (int i = 0; i < towerHeight.Length; i++)
            {
                if (towerHeight[i] > maxSeenSoFar)
                {
                    maxSeenSoFar = towerHeight[i];
                }
                rainWater += Math.Max(Math.Min(maxSeenSoFar, maxSeenRight[i]) - towerHeight[i], 0);

            }


            return rainWater;
        }
        /// <summary>
        /// https://www.geeksforgeeks.org/trapping-rain-water/
        /// </summary>
        /// <param name="towerHeight"></param>
        /// <returns></returns>
        public static int FetchRainWaterTrapBetweenTowersOptNoSpace(int[] towerHeight)
        {
            int rainWater = 0;
            

            return rainWater;
        }
    }
}
