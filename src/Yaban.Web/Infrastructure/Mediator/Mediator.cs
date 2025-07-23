using Yaban.Web.Application.Interfaces.Mediator;

namespace Yaban.Web.Infrastructure.Mediator;

public class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        // Fabrikadan bu isteğe özel, önceden derlenmiş çalıştırıcıyı al.
        var runner = RequestHandlerFactory.GetRunner(request.GetType());

        // Çalıştırıcıyı, o anki isteğe özel serviceProvider ile çalıştır.
        var result = await (Task<TResponse>)runner(request, _serviceProvider, cancellationToken);

        return result;
    }

   
}