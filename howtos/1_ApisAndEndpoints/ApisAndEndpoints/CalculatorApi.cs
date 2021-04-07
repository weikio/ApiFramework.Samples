namespace ApisAndEndpoints
{
    public class CalculatorApi
    {
        public int Configuration { get; set; }

        public int Sum(int x, int y)
        {
            return x + y + Configuration;
        }
    }
}
