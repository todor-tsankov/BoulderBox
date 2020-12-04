using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using AutoMapper;
using BoulderBox.Data.Common.Models;
using BoulderBox.Data.Common.Repositories;
using BoulderBox.Services.Mapping;

namespace BoulderBox.Services.Data.Common
{
    public class BaseService<TModel> : IBaseService<TModel>
        where TModel : class, IDeletableEntity
    {
        private readonly IDeletableEntityRepository<TModel> entityRepository;
        private readonly IMapper mapper;

        public BaseService(IDeletableEntityRepository<TModel> entityRepository, IMapper mapper)
        {
            this.NullCheck(entityRepository, nameof(entityRepository));
            this.NullCheck(mapper, nameof(mapper));

            this.entityRepository = entityRepository;
            this.mapper = mapper;
        }

        public int Count(Expression<Func<TModel, bool>> predicate = null)
        {
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

        public async Task AddAsync(object inputModel)
        {
            this.NullCheck(inputModel, nameof(inputModel));

            var entity = this.mapper.Map<TModel>(inputModel);

            await this.entityRepository.AddAsync(entity);
            await this.entityRepository.SaveChangesAsync();
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
            bool asc = true,
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
                if (asc)
                {
                    entities = entities.OrderBy(orderBySelector);
                }
                else
                {
                    entities = entities.OrderByDescending(orderBySelector);
                }
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

        protected void NullCheck(object obj, string name)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(name);
            }
        }
    }
}
