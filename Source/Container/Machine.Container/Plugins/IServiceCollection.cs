namespace Machine.Container.Plugins
{
  public interface IServiceCollection
  {
    void RegisterServices(ContainerRegisterer register);
  }
}