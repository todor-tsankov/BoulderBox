using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using BoulderBox.Data.Common.Models;
using BoulderBox.Data.Common.Repositories;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Services.Data.Common
{
    public class BaseService<TModel> : IBaseService<TModel>
        where TModel : class, IDeletableEntity
    {
        private readonly IDeletableEntityRepository<TModel> entityRepository;

        public BaseService(IDeletableEntityRepository<TModel> entityRepository)
        {
            this.entityRepository = entityRepository;
        }

        public int Count(Expression<Func<TModel, bool>> predicate)
        {
            this.NullCheck(predicate, nameof(predicate));

            var entities = this.entityRepository
               .AllAsNoTracking();

            if (predicate != null)
            {
                entities = entities.Where(predicate);
            }

            return entities.Count();
        }

        public bool Exists(Expression<Func<TModel, bool>> predicate)
        {
            this.NullCheck(predicate, nameof(predicate));

            var exists = this.entityRepository
                .AllAsNoTracking()
                .Any(predicate);

            return exists;
        }

        public TViewModel GetSingle<TViewModel>(Expression<Func<TModel, bool>> predicate)
        {
            this.NullCheck(predicate, nameof(predicate));

            var entity = this.entityRepository
                .AllAsNoTracking()
                .Where(predicate)
                .To<TViewModel>()
                .FirstOrDefault();

            return entity;
        }

        public IEnumerable<T> GetMany<T>(
            Expression<Func<TModel, bool>> predicate = null,
            Expression<Func<TModel, object>> orderBySelector = null,
            int? skip = null,
            int? take = null)
        {
            var entities = this.entityRepository
                .AllAsNoTracking();

            if (predicate != null)
            {
                entities = entities.Where(predicate);
            }

            if (orderBySelector != null)
            {
                entities = entities.OrderBy(orderBySelector);
            }

            if (skip != null)
            {
                entities = entities.Skip((int)skip);
            }

            if (take != null)
            {
                entities = entities.Take((int)take);
            }

            var mapped = entities
                .To<T>()
                .ToList();

            return mapped;
        }

        public async Task<bool> DeleteAsync(Expression<Func<TModel, bool>> predicate)
        {
            this.NullCheck(predicate, nameof(predicate));

            var entity = this.entityRepository
                .All()
                .Where(predicate)
                .FirstOrDefault();

            if (entity == null)
            {
                return false;
            }

            this.entityRepository.Delete(entity);
            await this.entityRepository.SaveChangesAsync();

            return true;
        }

        private void NullCheck(object predicate, string parameter)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }
        }
    }
}
