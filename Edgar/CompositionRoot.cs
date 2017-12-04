using AiDollar.Edgar.Service.Service;
using AiDollar.Infrastructure.Database;
using AiDollar.Infrastructure.Hosting;
using AiDollar.Infrastructure.Logger;
using AiDollar.Infrastructure.Threading;
using StructureMap;

namespace AiDollar.Edgar.Service
{
    public class CompositionRoot : ICompositionRoot<IService>
    {
        protected static CompositionRoot SvcComposite;
        private readonly IContainer _container;

        public CompositionRoot()
        {
            var settings = new EdgarSettings();
            _container = new Container(
                config =>
                {
                    ConfigureLogger(config, settings);

                    config.For<IHttpDataAgent>().Use<HttpDataAgent>();

                    config.For<IUtil>().Use<Util>();

                    config.For<IAiPortfolioSvc>().Use<AiPortfolioSvc>();

                    config.For<IAiDbSvc>().Use<AiDbSvc>()
                        .Ctor<string>("connectionString").Is(settings.AiDollarMongo)
                        .Ctor<string>("database").Is(settings.AiDollarDb);

                    config.For<IEdgarIdxSvc>().Use<EdgarIdxSvc>().Ctor<string>().Is(settings.EdgarCrawlerUri)
                        
                        ;
                    config.For<IF4Svc>().Use<F4Svc>();

                    config.For<IDbOperation>().Use<MongoDbOperation>()
                        .Ctor<string>("connectionString").Is(settings.AiDollarMongo)
                        .Ctor<string>("database").Is(settings.AiDollarDb);

                    config.For<IF13Svc>().Use<F13Svc>().Ctor<string>("edgarUri").Is(settings.EdgarArchiveRoot)
                        
                        .Ctor<string>("posPage").Is(settings.PosPage);

                    config.For<IService>().Use<EdgarService>().Ctor<int>().Is(settings.F4InDays)
                    .Ctor<string>("outputPath").Is(settings.DataPath);

                }
            );
        }

        public IService Initialize()
        {
            return _container.GetInstance<IService>();
        }

        public void Dispose()
        {
            _container.Dispose();
        }

        protected virtual void ConfigureLogger(ConfigurationExpression config, EdgarSettings settings)
        {
            var logger = BackgroundWorkerFactory.Logger ??
                         new Log4NetLogger(settings.LogPath, settings.LogFilename, settings.LogArchivePath);
            config.For<ILogger>().Use(logger);
            config.Policies.FillAllPropertiesOfType<ILogger>();
            BackgroundWorkerFactory.Logger = logger;
        }

        public T GetInstance<T>()
        {
            return _container.GetInstance<T>();
        }

        public static CompositionRoot CompositeRootInstanace()
        {
            return SvcComposite ?? (SvcComposite = new CompositionRoot());
        }
    }
}