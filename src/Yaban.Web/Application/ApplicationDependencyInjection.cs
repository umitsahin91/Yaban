using Yaban.Web.Application.Interfaces.Mediator;
using Yaban.Web.Infrastructure.Mediator;

namespace Yaban.Web.Application;

public static class ApplicationDependencyInjection
{
    // AddModules yerine, bizim yapımıza uygun yeni bir metot
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // 1. ADIM: Mediator Implementasyonunu Kaydetme
        //===============================================
        services.AddScoped<IMediator, Mediator>();


        // 2. ADIM: Handler'ları Otomatik Olarak Bulma ve Kaydetme (Basitleştirilmiş Hali)
        //=============================================================================
        // Dinamik olarak DLL taramak yerine, iş mantığımızın bulunduğu 'Application' katmanının
        // Assembly'sini (projesini) doğrudan referans alıyoruz.
        var applicationAssembly = typeof(IMediator).Assembly; // IMediator'ın bulunduğu Assembly'yi (Application katmanı) al

        services.Scan(selector => selector
            .FromAssemblies(applicationAssembly) // Sadece bu Assembly'yi tara
            .AddClasses(c => c.AssignableTo(typeof(IRequestHandler<,>))) // IRequestHandler implementasyonlarını bul
                .AsImplementedInterfaces()        // Arayüzleriyle eşleştirerek kaydet
                .WithScopedLifetime()             // Scoped olarak kaydet
        );


        // 3. ADIM: Cache Isıtma (İsteğe Bağlı, Gelişmiş Özellik)
        //========================================================
        // Bu kısım, RequestHandlerFactory adında bir sınıfınız olduğunu varsayar.
        // Eğer böyle bir yapınız yoksa bu kısmı tamamen silebilirsiniz.
        // Eğer varsa, mantık yine aynı, sadece tek bir assembly'yi tarıyoruz.


        var handlerTypes = applicationAssembly
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)))
            .ToList();

        foreach (var handlerType in handlerTypes)
        {
            var requestType = handlerType.GetInterfaces()
                .First(i => i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
                .GetGenericArguments()[0];

            RequestHandlerFactory.GetRunner(requestType); // Kendi fabrikanızı burada çağırın
        }


        return services;
    }
}
