using BenchmarkDotNet.Running;
using DietPlanner.Api.Benchmarks;

namespace DietPlanner.Benchmarks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<EfBenchmark>();
        }
    }
}
