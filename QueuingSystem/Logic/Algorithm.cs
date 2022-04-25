using System;
using System.Collections.Generic;
using System.Linq;

namespace QueuingSystem.Logic
{
    public static class Algorithm
    {
        private static int Factorial(int i) => Enumerable.Range(1,i<1?1:i).Aggregate((f,x)=>f*x);
        
        public static Dictionary<string, float> CalculateRejectsOneChannel(
            float applicationFlow, 
            float averageTimeServicing) 
        {
            var serviceFlow = 1 / averageTimeServicing;
            var result = new Dictionary<string, float>()
            {
                {"Q", MathF.Round((serviceFlow)/(serviceFlow + applicationFlow), 2)},
                {"Reject chance", MathF.Round((applicationFlow)/(serviceFlow + applicationFlow), 2)},
                {"A", MathF.Round((serviceFlow)/(serviceFlow + applicationFlow) * applicationFlow, 2)}
            };
            return result;
        }
        
        public static Dictionary<string, float> CalculateRejectsSeveralChannel(
            int channelCount,
            float applicationFlow, 
            float averageTimeServicing) 
        {
            var serviceFlow = 1 / averageTimeServicing;
            var ro = applicationFlow / serviceFlow;

            var result = new Dictionary<string, float>();

            var p0 = 1f;
            for (var n = 1; n <= channelCount; n++)
            {
                p0 += (MathF.Pow(applicationFlow, n)) / (MathF.Pow(serviceFlow, n) * Factorial(n));
            }
            result.Add($"P0", MathF.Round(MathF.Pow(p0, -1), 2));
            
            for (var p = 1; p <= channelCount; p++)
            {
                var pCal = (MathF.Pow(ro, p) / Factorial(p)) * result["P0"];
                result.Add($"P{p}", MathF.Round(pCal, 2));
            }
            
            result.Add("Reject chance", result[$"P{channelCount}"]);
            result.Add("Q", 1 - result[$"P{channelCount}"]);
            result.Add("A", result["Q"] - applicationFlow);
            result.Add("K", result["A"] / serviceFlow);
            
            return result;
        }

        public static Dictionary<string, float>? CalculateQueueUnlimitedOneChannel(
            float applicationFlow, 
            float averageTimeServicing,
            params int[] queuePosition) 
        {
            var serviceFlow = 1 / averageTimeServicing;
            var ro = applicationFlow / serviceFlow;
            if (ro >= 1) return null;

            var result = new Dictionary<string, float>()
            {
                {"L system", MathF.Round(ro / (1 - ro), 2)},
                {"T system", MathF.Round((ro / (1 - ro)) / applicationFlow, 2)},
                {"L queue", MathF.Round((ro * ro) / (1 - ro), 2)},
                {"T queue", MathF.Round(((ro * ro) / (1 - ro)) / applicationFlow, 2)},
                {"P workload", MathF.Round(ro, 2)}
            };
            
            foreach (var position in queuePosition)
            {
                result.Add($"P{position}", MathF.Round(MathF.Pow(ro, position) * (1 - ro), 2));
            }
            
            return result;
        } 
        
        public static Dictionary<string, float>? CalculateQueueUnlimitedSeveralChannel(
            int channelCount,
            float applicationFlow, 
            float averageTimeServicing,
            params int[] queuePosition) 
        {
            var serviceFlow = 1 / averageTimeServicing;
            var ro = applicationFlow / serviceFlow;
            if (ro / channelCount >= 1) return null;

            var result = new Dictionary<string, float>()
            {
                {"K", MathF.Round(ro, 2)}
            };

            var p0 = 1f;
            for (var n = 1; n <= channelCount; n++)
            {
                p0 += (MathF.Pow(ro, n)) / Factorial(n);
            }
            p0 += MathF.Pow(ro, channelCount + 1) / (Factorial(channelCount) * (channelCount - ro));
            result.Add($"P0", MathF.Round(MathF.Pow(p0, -1), 2));
            
            for (var p = 1; p <= channelCount; p++)
            {
                var pCal = (MathF.Pow(ro, p) / Factorial(p)) * result["P0"];
                result.Add($"P{p}", MathF.Round(pCal, 2));
            }

            foreach (var pos in queuePosition)
            {
                result.Add($"P{pos} Queue", 
                    (MathF.Pow(ro, channelCount + pos) 
                    / 
                    (MathF.Pow(channelCount, pos) * Factorial(channelCount))) * result["P0"]);
            }
            
            result.Add("P queue", 
                (MathF.Pow(ro, channelCount + 1) 
                 / 
                 (Factorial(channelCount) * (channelCount - ro))) 
                * result["P0"]);
            
            result.Add("L queue", 
                (MathF.Pow(ro, channelCount + 1) 
                 / 
                 (channelCount * Factorial(channelCount) * MathF.Pow(1 - (ro / channelCount), 2))) 
                * result["P0"]);
            
            result.Add("L system", result["L queue"] + ro);
            
            result.Add("T queue", result["L queue"] / applicationFlow);
            
            result.Add("T system", result["L system"] / applicationFlow);

            foreach (var (key, _) in result)
            {
                result[key] = MathF.Round(result[key], 3);
            }
            
            return result;
        }

        public static Dictionary<string, float>? CalculateQueueLimitedOneChannel(
            int queueCount,
            float applicationFlow,
            float averageTimeServicing) 
        {
            var serviceFlow = 1 / averageTimeServicing;
            var ro = applicationFlow / serviceFlow;
            var result = new Dictionary<string, float>()
            {
                {"P0", (1 - ro) / (1 - MathF.Pow(ro, queueCount + 2))}
            };

            for (var k = 1; k < queueCount; k++)
                result.Add($"P{k}", MathF.Pow(ro, k) * result["P0"]);
            
            result.Add("P reject", MathF.Pow(ro, queueCount + 1) * result["P0"]);
            result.Add("A", applicationFlow * (1 - MathF.Pow(ro, queueCount + 1) * result["P0"]));
            result.Add("Q", 1 - MathF.Pow(ro, queueCount + 1) * result["P0"]);
            result.Add("L queue", (ro * ro) * (
                (1 - MathF.Pow(ro, queueCount) * (queueCount + 1 - queueCount * ro)) 
                / 
                ((1 - MathF.Pow(ro, queueCount + 2)) * (1 - ro))));
            result.Add("L system", result["L queue"] + (1 - result["P0"]));
            
            foreach (var (key, _) in result)
                result[key] = MathF.Round(result[key], 3);
            return result;
        }
        
        public static Dictionary<string, float>? CalculateQueueLimitedSeveralChannel(
            int channelCount,
            int queueCount,
            float applicationFlow, 
            float averageTimeServicing) 
        {
            var serviceFlow = 1 / averageTimeServicing;
            var ro = applicationFlow / serviceFlow;
            var result = new Dictionary<string, float>();

            float p0;
            if (Math.Abs(ro - channelCount) < 0.1)
            {
                p0 = 1f;
                for (var n = 1; n <= channelCount; n++)
                {
                    p0 += (MathF.Pow(channelCount, n)) / Factorial(n);
                }
                p0 += (MathF.Pow(channelCount, channelCount) / Factorial(channelCount)) * queueCount;
                result.Add($"P0", MathF.Round(MathF.Pow(p0, -1), 2));
            }
            else
            {
                p0 = 1f;
                for (var n = 1; n <= channelCount; n++)
                {
                    p0 += (MathF.Pow(ro, n)) / Factorial(n);
                }

                for (var m = 1; m <= queueCount; m++)
                {
                    p0 += (MathF.Pow(ro, channelCount + 1) * (1 - MathF.Pow(ro / channelCount, m))) 
                          / 
                          (channelCount * Factorial(channelCount) * (1 - ro / channelCount));
                }
                result.Add($"P0", MathF.Round(MathF.Pow(p0, -1), 2));
            }
            
            result.Add("P reject", 
                (MathF.Pow(ro, channelCount + queueCount) 
                 / 
                 (MathF.Pow(channelCount, queueCount) * Factorial(channelCount))) 
                * result["P0"]);
            
            result.Add("A", 
                applicationFlow * (1 - (MathF.Pow(ro, queueCount + 1) 
                / 
                (MathF.Pow(channelCount, queueCount) * Factorial(channelCount))) 
                    * result["P0"]));
            
            result.Add("Q", 1 - result["P reject"]);
            result.Add("L queue", (ro * ro) * (
                (1 - MathF.Pow(ro, queueCount) * (queueCount + 1 - queueCount * ro)) 
                / 
                ((1 - MathF.Pow(ro, queueCount + 2)) * (1 - ro))));
            result.Add("K", result["L queue"] + (1 - result["P0"]));
            
            foreach (var (key, _) in result)
                result[key] = MathF.Round(result[key], 3);
            return result;
        }
    }
}