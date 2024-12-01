namespace RabbitMQ_excelCreate.IoC
{
    public class DependencyResolver : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserManager<IdentityUser>>().As<UserManager<IdentityUser>>().InstancePerLifetimeScope();
            builder.RegisterType<SignInManager<IdentityUser>>().As<SignInManager<IdentityUser>>().InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
