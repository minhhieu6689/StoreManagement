using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StoreManagement.Infrastructure;
using StoreManagement.Repository.Repository;
using StoreManagement.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StoreManangement.Service.BaseService
{
    public abstract class BaseService<TEntity, TDto> : IBaseService<TEntity, TDto>
        where TEntity : class
        where TDto : class
    {
        protected readonly IUnitOfWork _unitOfWork;

        protected abstract IGenericRepository<TEntity> _repository { get; }

        public BaseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public virtual async Task<TDto> CreateAsync(TDto dto)
        {
            var entity = DtoToEntity(dto);

            _repository.Add(entity);

            await _unitOfWork.SaveAsync();

            return EntityToDto(entity);
        }

        public virtual async Task<TDto> UpdateAsync(TDto dto)
        {
            var entity = DtoToEntity(dto);

            _repository.Update(entity);

            await _unitOfWork.SaveAsync();

            return EntityToDto(entity);

        }

        public virtual async Task DeleteAsync(object id)
        {
            var entity = await _repository.FindByIdAsync(id);

            if (entity == null) throw new Exception("Not found entity object with id: " + id);

            _repository.Delete(entity);

            await _unitOfWork.SaveAsync();
        }

        public virtual async Task<TDto> FindByIdAsync(object id)
        {
            return EntityToDto(await _repository.FindByIdAsync(id));
        }

        public async Task<PaginatedList<TEntity, TDto>> FindAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int page = 1)
        {
            var query = _repository.Find(filter);

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            const int PageSize = 10;
            return await PaginatedList<TEntity, TDto>.CreateAsync(query.AsNoTracking(), page, PageSize);
        }


        protected TDto EntityToDto(TEntity entity)
        {
            return Mapper.Map<TDto>(entity);
        }

        protected TEntity DtoToEntity(TDto dto)
        {
            return Mapper.Map<TEntity>(dto);
        }

        protected TEntity DtoToEntity(TDto dto, TEntity entity)
        {
            return Mapper.Map(dto, entity);
        }

        protected IEnumerable<TDto> EntityToDto(IEnumerable<TEntity> entities)
        {
            return Mapper.Map<IEnumerable<TDto>>(entities);
        }

        protected IEnumerable<TEntity> DtoToEntity(IEnumerable<TDto> dto)
        {
            return Mapper.Map<IEnumerable<TEntity>>(dto);
        }

        public async Task<IEnumerable<TDto>> GetAll()
        {
            return EntityToDto(await _repository.GetAll());
        }
    }
}
