namespace StatisticalLearning.Api.Host.DTOs.Responses
{
    public class GaussianNaiveBayesResult
    {
        public double[][] Probabilities { get; set; }
        public double[] Classes { get; set; }
    }
}
