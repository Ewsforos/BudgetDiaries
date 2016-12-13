using Autofac;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP
{
    public static class IoC
    {
        private static IContainer Instance { get; set; }
        private static bool disposed = false;

        public static async Task InitializeAsync()
        {
            var builder = new ContainerBuilder();

            var uow = new UnitOfWork();

            builder.Register(u => new AccountService(uow)).As<IAccountService>().SingleInstance();
            builder.Register(u => new CategoryService(uow)).As<ICategoryService>().SingleInstance();
            builder.Register(u => new TransactionService(uow)).As<ITransactionService>().SingleInstance();

            Instance = builder.Build();

            await Task.Yield();
        }

        public static T Resolve<T>() where T : class
        {
            return Instance?.Resolve<T>();
        }

        public static void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    Instance.Dispose();
                }
            }
            disposed = true;
        }

        public static void Dispose()
        {
            Dispose(true);
        }
    }
}
