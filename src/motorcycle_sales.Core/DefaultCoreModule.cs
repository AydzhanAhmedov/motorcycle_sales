using Autofac;
using motorcycle_sales.Core.Interfaces;
using motorcycle_sales.Core.Services;

namespace motorcycle_sales.Core;

public class DefaultCoreModule : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    builder.RegisterType<ToDoItemSearchService>()
        .As<IToDoItemSearchService>().InstancePerLifetimeScope();
  }
}
