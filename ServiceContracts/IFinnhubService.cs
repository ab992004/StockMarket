namespace ServiceContracts
{
    public interface IFinnhubService
    {
        Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSympol);
        Task<Dictionary<string, object>?> GetCompanyProfile(string stockSympol);

    }
}
