using PRN221.SalesManagement.Repo.Interfaces;
using PRN221.SalesManagement.Repo.Models;
using PRN221.SalesManagement.Repo.Persistences;
using System.ComponentModel.DataAnnotations;

namespace PRN221.SalesManagement.Repo.Impl
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private SalesManagementContext context;
        private GenericRepository<Category> categoryRepository;
        private GenericRepository<Product> productRepository;
        private GenericRepository<OrderDetail> orderDetailRepository;
        private GenericRepository<SaleOrder> saleOrderRepository;
        public UnitOfWork(SalesManagementContext _context) { 
             context = _context;
        }


        public IGenericRepository<Product> ProductRepository
        {
            get
            {
                return productRepository ??= new GenericRepository<Product>(context);
            }
        }

        public IGenericRepository<Category> CategoryRepository
        {
            get
            {
                return categoryRepository ??= new GenericRepository<Category>(context);
            }
        }

        public IGenericRepository<OrderDetail> OrderDetailRepository
        {
            get
            {
                return orderDetailRepository ??= new GenericRepository<OrderDetail>(context);
            }
        }

        public IGenericRepository<SaleOrder> SaleOrderRepository
        {
            get
            {
                return saleOrderRepository ??= new GenericRepository<SaleOrder>(context);
            }
        }

        public void Save()
        {
            var validationErrors = context.ChangeTracker.Entries<IValidatableObject>()
                .SelectMany(e => e.Entity.Validate(null))
                .Where(e => e != ValidationResult.Success)
                .ToArray();
            if (validationErrors.Any())
            {
                var exceptionMessage = string.Join(Environment.NewLine,
                    validationErrors.Select(error => $"Properties {error.MemberNames} Error: {error.ErrorMessage}"));
                throw new Exception(exceptionMessage);
            }
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
