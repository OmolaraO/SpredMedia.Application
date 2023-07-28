namespace SpredMedia.CommonLibrary
{
    public interface IHttpClientService
    {
        Task<TRes> GetRequestAsync<TRes>(string baseUrl, string url, string token = "") where TRes : class;
        Task<TRes> PostRequestAsync<TRes>(string baseUrl, string url, string requestModel, string token = "")
            where TRes : class;
    }
}