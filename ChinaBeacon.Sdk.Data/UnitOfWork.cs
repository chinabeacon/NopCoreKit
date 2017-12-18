using System;
using System.Collections.Generic;
using System.Text;
using ChinaBeacon.Sdk.Core.Data;

namespace ChinaBeacon.Sdk.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContext _context;

        public UnitOfWork(IDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public int Commit()
        {
            return _context.SaveChanges();
        }
    }
}
