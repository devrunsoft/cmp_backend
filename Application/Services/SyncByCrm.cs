using System;
using CMPNatural.Core.Repositories;

namespace CMPNatural.Application.Services
{
	public class SyncByCrm
	{
		IProductRepository _repository;
        public SyncByCrm(IProductRepository repository)
		{
			_repository = repository;
        }

		public void sync()
		{
			_repository.sync();

        }
	}
}

