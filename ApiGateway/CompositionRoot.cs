using System;
using System.Collections.Generic;
using System.Linq;
using Bam.Compliance.Infrastructure.Hosting;
using Bam.Compliance.ApiGateway.Http;
using Bam.Compliance.Infrastructure.Logger;
using Bam.Compliance.Infrastructure.Permissions;
using Bam.Compliance.Infrastructure.Threading;
using Bam.Compliance.Infrastructure.Wcf.PermissionService;
using StructureMap;
using System.Web.Http.Dependencies;
using Bam.Compliance.ApiGateway.Services;
using Bam.Oms.Compliance;
using Bam.Oms.Compliance.Services;

namespace Bam.Compliance.ApiGateway
{
    public class CompositionRoot : ICompositionRoot<IService>
    {
        private readonly IContainer _container;
        private readonly ApiGatewaySettings _settings;

        public static CompositionRoot SvcComposite;

        public static CompositionRoot CompositeRootInstanace()
        {
            return SvcComposite ?? (SvcComposite = new CompositionRoot());
        }

        public CompositionRoot()
        {
            _settings = new ApiGatewaySettings();
            _container = new Container(
                    config =>
                    {
                        ConfigureLogger(config, _settings);
                        ConfigureRest(config, _settings);
                        ConfigureAuthorization(config, _settings);
                    }
                );

            _container.Inject(typeof(IDependencyResolver), new StructureMapDependencyResolver(_container));
        }

        protected virtual void ConfigureLogger(ConfigurationExpression config, ApiGatewaySettings settings)
        {
            var logger = new Log4NetLogger(settings.LogPath, settings.LogFilename, settings.LogArchivePath);
            config.For<ILogger>().Use(logger);
            config.Policies.FillAllPropertiesOfType<ILogger>();
            BackgroundWorkerFactory.Logger = logger;
        }
        private void ConfigureRest(ConfigurationExpression config, ApiGatewaySettings settings)
        {
            var httpConfig = config.ForConcreteType<HttpServiceOptions>().Configure
                .Ctor<string>().Is(settings.Http.BaseUrl)
                .Setter(m => m.PermissionedApplications).Is(settings.PermissionedApplications)
                .Setter(m => m.DependencyResolver).IsTheDefault()
                .Setter(m => m.DisableAuthorization).Is(settings.DisableAuthorization)
                .Setter(m => m.DefaultTokenExpiry).Is(settings.Http.DefaultTokenExpiry)
                .Setter(m => m.AllowTokenAsUrlParameter).Is(settings.Http.AllowTokenAsUrlParameter);

            config.For<IService>().Add<HttpService>()
                .Named(nameof(HttpService))
                .Ctor<HttpServiceOptions>().Is(httpConfig);
            
            config
               .ForConcreteType<ApiGatewayService>().Configure
               .Ctor<IService>("httpService").Is(ctx => ctx.GetInstance<IService>(nameof(HttpService)));

            //controllers
            config.ForConcreteType<Http.Controller.PermissionController>()
                .Configure
                .Ctor<string[]>().Is(settings.PermissionedApplications);

            config.For<IHistoricTradeRetriever>().Add<HistoricTradeRetriever>()
                .Ctor<string>().Is(settings.BamCoreLite);

            config.For<IIntradayAggrRetriever>().Add<IntradayAggrRetriever>()
               .Ctor<IEnumerable<string>>().Is(settings.OmsServiceUris);
           
            config.For<IExecutionRetriever>().Add<ExecutionRetriever>()
               .Ctor<string>().Is(settings.BrainConnectionString);

            config.For<IHeadRoomCalculator>().Use((HeadRoomCalculator)BAM.Infrastructure.Ioc.Container.Instance.Resolve(typeof(HeadRoomCalculator)));
            config.For<IPositionSvc>().Use((PositionSvc)BAM.Infrastructure.Ioc.Container.Instance.Resolve(typeof(PositionSvc)));
            config.For<IFirmPositionComplianceSvc>().Use((FirmPositionComplianceSvc)BAM.Infrastructure.Ioc.Container.Instance.Resolve(typeof(FirmPositionComplianceSvc)));
            config.For<IFactSvc>().Use((FactSvc)BAM.Infrastructure.Ioc.Container.Instance.Resolve(typeof(FactSvc)));
            config.For<IRuleSvc>().Use((RuleSvc)BAM.Infrastructure.Ioc.Container.Instance.Resolve(typeof(RuleSvc)));
        }

        protected virtual void ConfigureAuthorization(ConfigurationExpression config, ApiGatewaySettings settings, TimeSpan permissionRefresh = default(TimeSpan))
        {
            config.For<IPermissionedEntityFilter>().Use<PermissionedEntityFilter>().Singleton();

            config
                .For<IPermissionService>()
                .Use<CachedPermissionService>()
                .Ctor<TimeSpan>().Is(permissionRefresh == default(TimeSpan) ? TimeSpan.FromMinutes(30) : permissionRefresh)
                .Ctor<IPermissionService>().Is<Infrastructure.Permissions.PermissionService>(cfg => cfg
                    .Ctor<Infrastructure.Wcf.PermissionService.PermissionService>().Is("service",
                        () => new PermissionServiceClient("TcpPermissionEndpoint")));

            if (settings.DisableAuthorization)
            {
                config.For<IPermissionedEntityFilter>()
                    .Use<NoAuthPermissionedEntityFilter>()
                    .Singleton();
            }
            else
            {
                config.For<IPermissionedEntityFilter>()
                    .Use<PermissionedEntityFilter>()
                    .Singleton();
            }
        }
        public IService Initialize()
        {
            var svc = _container.GetInstance<IService>();
            return svc;
        }

        public void Dispose()
        { }

        private class StructureMapDependencyResolver : IDependencyResolver
        {
            private readonly IContainer _container;

            public StructureMapDependencyResolver(IContainer container)
            {
                _container = container;
            }

            public void Dispose()
            {
                // nothing
            }

            public object GetService(Type serviceType)
            {
                return serviceType.Namespace != null && serviceType.Namespace.Contains("Bam.Oms.Compliance") 
                    ? BAM.Infrastructure.Ioc.Container.Instance.Resolve(serviceType) : _container.TryGetInstance(serviceType);
            }

            public IEnumerable<object> GetServices(Type serviceType)
            {
                return _container.GetAllInstances(serviceType).Cast<object>();
            }

            public IDependencyScope BeginScope()
            {
                return new StructureMapDependencyScope(_container.GetNestedContainer());
            }

            private class StructureMapDependencyScope : IDependencyScope
            {
                private readonly IContainer _container;

                public StructureMapDependencyScope(IContainer container)
                {
                    _container = container;
                }

                public void Dispose()
                {
                    _container.Dispose();
                }

                public object GetService(Type serviceType)
                {
                    return serviceType.Namespace != null && serviceType.Namespace.Contains("Bam.Oms")
                    ? BAM.Infrastructure.Ioc.Container.Instance.Resolve(serviceType) : _container.GetInstance(serviceType);
                }

                public IEnumerable<object> GetServices(Type serviceType)
                {
                    return _container.GetAllInstances(serviceType).Cast<object>();
                }
            }
        }
    }
}
