using BenchmarkDotNet.Running;

namespace DietPlanner.Api.Benchmarks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<EfBenchmark>();
        }
    }
}
