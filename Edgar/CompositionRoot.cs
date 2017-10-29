using AiDollar.Infrastructure.Database;
using AiDollar.Infrastructure.Hosting;
using AiDollar.Infrastructure.Logger;
using AiDollar.Infrastructure.Threading;
using StructureMap;
using StructureMap.Building;

namespace AiDollar.Edgar.Service
{
    public class CompositionRoot : ICompositionRoot<IService>
    {
        private readonly IContainer _container;
        
        protected static CompositionRoot SvcComposite;

        public CompositionRoot()
        {
            var settings = new EdgarSettings();
            _container = new Container(
                config =>
                {
                    ConfigureLogger(config, settings);

                    config.For<IHttpDataAgent>().Use<HttpDataAgent>();

                    config.For<IUtil>().Use<Util>();

                    config.For<IEdgarApi>().Use<EdgarApi>()
                        .Ctor<string>("connectionString").Is(settings.AiDollarMongo)
                        .Ctor<string>("database").Is(settings.AiDollarDb);

                    config.For<IDbOperation>().Use<MongoDbOperation>()
                    .Ctor<string>("connectionString").Is(settings.AiDollarMongo)
                    .Ctor<string>("database").Is(settings.AiDollarDb);

                    config.For<IService>().Use<EdgarService>()
                        .Ctor<string[]>("ciks").Is(settings.Ciks)
                        .Ctor<string>("edgarUri").Is(settings.EdgarArchiveRoot)
                        .Ctor<string>("outputPath").Is(settings.DataPath)
                        .Ctor<string>("posPage").Is(settings.PosPage);

                   

                }
            );
 
        }

        protected virtual void ConfigureLogger(ConfigurationExpression config, EdgarSettings settings)
        {
            var logger = BackgroundWorkerFactory.Logger ?? new Log4NetLogger(settings.LogPath, settings.LogFilename, settings.LogArchivePath);
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
        public IService Initialize()
        {
            return _container.GetInstance<IService>();
        }

        public void Dispose()
        {
            _container.Dispose();
        }
    }
}
