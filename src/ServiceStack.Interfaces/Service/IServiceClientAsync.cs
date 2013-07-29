using System;
using System.Threading.Tasks;
using ServiceStack.ServiceHost;

namespace ServiceStack.Service
{
	public interface IServiceClientAsync : IRestClientAsync
	{
		void SendAsync<TResponse>(object request, Action<TResponse> onSuccess, Action<TResponse, Exception> onError);
        Task<TResponse> SendAsync<TResponse>(IReturn<TResponse> request);
	}
}