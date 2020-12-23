using Autofac;
using SMA.Backup.Util;

namespace SMA.Backup.Runtime
{
    public class UtilModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<CommonUtil>().As<ICommonUtil>().InstancePerLifetimeScope();
        }
    }
}
