using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChinaBeacon.Sdk.Data
{
    /// <summary>
    /// 实体Type配置(此类为兼容原Nop注册实体的方法:OnModelCreating) --由于.net core只有IEntityTypeConfiguration接口 没有之前的EntityTypeConfiguration了
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class EntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
        }
    }
}
