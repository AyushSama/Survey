using DataHelper.EFData.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataHelper.EFData.Common.Repositories
{
    public class GenericBaseRepo<T, M> : IGenericBaseRepo<T, M> where T : class, new() where M : DbContext
    {

        public GenericBaseRepo(M dbContext)
        {
            _dbContext = dbContext;
        }

        public M _dbContext { get; }
    }

    public class GenericRepo<T, M> : GenericBaseRepo<T, M>, IGenericRepo<T, M> where T : class, new() where M : DbContext
    {
        public GenericRepo(M dbContext) : base(dbContext)
        {
        }

    }
}
