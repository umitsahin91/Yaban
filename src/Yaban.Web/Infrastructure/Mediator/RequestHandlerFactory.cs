using System.Collections.Concurrent;
using System.Linq.Expressions;
using Yaban.Web.Application.Interfaces.Mediator;

namespace Yaban.Web.Infrastructure.Mediator;

public static class RequestHandlerFactory
{
    // Bu, derlenmiş çalıştırıcılarımızın yaşayacağı merkezi ve statik cache'imizdir.
    private static readonly ConcurrentDictionary<Type, Func<object, IServiceProvider, CancellationToken, Task>> _runners = new();

    // Bir istek tipi için çalıştırıcıyı getiren metot.
    public static Func<object, IServiceProvider, CancellationToken, Task> GetRunner(Type requestType)
    {
        return _runners.GetOrAdd(requestType, t =>
        {
            // Bu kısım, bir istek tipi için SADECE BİR KEZ çalışır.
            var requestInterface = t.GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequest<>));

            if (requestInterface is null)
                throw new InvalidOperationException($"Request type {t.Name} does not implement IRequest<TResponse>.");

            var responseType = requestInterface.GetGenericArguments()[0];
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(t, responseType);

            // (request, sp, ct) => ((IRequestHandler<TRequest, TResponse>)sp.GetService(handlerType)).Handle((TRequest)request, ct)
            // Yukarıdaki lambda'yı kod ile inşa ediyoruz:

            var requestParam = Expression.Parameter(typeof(object), "request");
            var spParam = Expression.Parameter(typeof(IServiceProvider), "sp");
            var ctParam = Expression.Parameter(typeof(CancellationToken), "ct");

            var castRequest = Expression.Convert(requestParam, t);
            var getServiceCall = Expression.Call(typeof(ServiceProviderServiceExtensions),
                nameof(ServiceProviderServiceExtensions.GetRequiredService),
                new[] { handlerType },
                spParam);
            var handlerInstance = Expression.Convert(getServiceCall, handlerType);
            var handleMethod = handlerType.GetMethod(nameof(IRequestHandler<IRequest<object>, object>.Handle))!;
            var handleCall = Expression.Call(handlerInstance, handleMethod, castRequest, ctParam);
            var lambda = Expression.Lambda<Func<object, IServiceProvider, CancellationToken, Task>>(handleCall, requestParam, spParam, ctParam);

            return lambda.Compile();
        });
    }
}
