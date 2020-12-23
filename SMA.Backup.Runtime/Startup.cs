using Autofac;

namespace SMA.Backup.Runtime
{
    public static class Startup
    {
        public static ContainerBuilder ConfigurationBuilder(ContainerBuilder builder)
        {
            builder.RegisterModule(new BackupModule());
            builder.RegisterModule(new BackupSourceModule());
            builder.RegisterModule(new UtilModule());
            builder.RegisterModule(new CommonModule());
            builder.RegisterModule(new DestinationModule());

            return builder;
        }

        public static IContainer Configure(ContainerBuilder builder)
        {
            return ConfigurationBuilder(builder).Build();
        }

        public static IContainer Configure()
        {
            return Configure(new ContainerBuilder());
        }
    }
}
