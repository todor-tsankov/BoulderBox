using BoulderBox.Data.Common.Repositories;
using BoulderBox.Data.Models;
using BoulderBox.Services.Data.Common;

namespace BoulderBox.Services.Data
{
    public class BouldersService : BaseService<Boulder>, IBouldersService
    {
        private readonly IDeletableEntityRepository<Boulder> bouldersRepository;

        public BouldersService(IDeletableEntityRepository<Boulder> bouldersRepository)
            : base(bouldersRepository)
        {
            this.bouldersRepository = bouldersRepository;
        }
    }
}
