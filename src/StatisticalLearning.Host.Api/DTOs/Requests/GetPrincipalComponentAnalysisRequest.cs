namespace StatisticalLearning.Api.Host.DTOs.Requests
{
    public class GetPrincipalComponentAnalysisRequest
    {
        public GetPrincipalComponentAnalysisRequest()
        {
            NbPrincipalComponents = 2;
        }

        public int NbPrincipalComponents { get; set; }
        public double[][] Matrix { get; set; }
    }
}
