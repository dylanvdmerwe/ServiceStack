using System;
using System.IO;
using System.Threading.Tasks;
using ServiceStack.ServiceHost;

namespace ServiceStack.Service
{
	public interface IRestClientAsync : IDisposable
	{
		void SetCredentials(string userName, string password);

        void GetAsync<TResponse>(IReturn<TResponse> request, Action<TResponse> onSuccess, Action<TResponse, Exception> onError);
        void GetAsync<TResponse>(string relativeOrAbsoluteUrl, Action<TResponse> onSuccess, Action<TResponse, Exception> onError);
        Task<TResponse> GetAsync<TResponse>(IReturn<TResponse> request);
        
        void DeleteAsync<TResponse>(string relativeOrAbsoluteUrl, Action<TResponse> onSuccess, Action<TResponse, Exception> onError);
        void DeleteAsync<TResponse>(IReturn<TResponse> request, Action<TResponse> onSuccess, Action<TResponse, Exception> onError);
        Task<TResponse> DeleteAsync<TResponse>(IReturn<TResponse> request);

        void PostAsync<TResponse>(IReturn<TResponse> request, Action<TResponse> onSuccess, Action<TResponse, Exception> onError);
		void PostAsync<TResponse>(string relativeOrAbsoluteUrl, object request, Action<TResponse> onSuccess, Action<TResponse, Exception> onError);
        Task<TResponse> PostAsync<TResponse>(IReturn<TResponse> request);

        void PutAsync<TResponse>(IReturn<TResponse> request, Action<TResponse> onSuccess, Action<TResponse, Exception> onError);
        void PutAsync<TResponse>(string relativeOrAbsoluteUrl, object request, Action<TResponse> onSuccess, Action<TResponse, Exception> onError);
        Task<TResponse> PutAsync<TResponse>(IReturn<TResponse> request);

        void CustomMethodAsync<TResponse>(string httpVerb, IReturn<TResponse> request, Action<TResponse> onSuccess, Action<TResponse, Exception> onError);
	    void CancelAsync();
	}

}