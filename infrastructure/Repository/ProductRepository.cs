using System.Linq;
using CmpNatural.CrmManagment.Product;
using CMPNatural.Application.Responses.Service;
using CMPNatural.Core.Entities;
using CMPNatural.Core.Enums;
using CMPNatural.Core.Repositories;
using infrastructure.Data;
using ScoutDirect.Core.Caching;
using ScoutDirect.infrastructure.Repository;

namespace CMPNatural.infrastructure.Repository
{
    public class ProductRepository : Repository<Product, long>, IProductRepository
    {
        ProductListApi _api;
        ProductPriceApi _priceApi;
        public ProductRepository(ScoutDBContext context, Func<CacheTech, ICacheService> cacheService, ProductListApi api , ProductPriceApi priceApi) : base(context, cacheService)
        {
            _api = api;
            _priceApi = priceApi;
        }

        public void sync()
        {
            var crmProducts = _api.call().Data;
            var localProducts = _dbContext.Product.ToList();

            List<Product> shouldAdd = new List<Product>();
            List<Product> shouldUpdate = new List<Product>();
            foreach (var local in localProducts)
            {
                syncPrice(local.ServiceCrmId, local.Id);
                var matchingCrm = crmProducts.FirstOrDefault(c => c._id == local.ServiceCrmId);

                if (matchingCrm == null)
                {
                    local.Enable = false;
                    shouldUpdate.Add(local);
                }
                else if (!matchingCrm.Equals(local)) 
                {
                    //local.Enable = true;
                    local.Name = matchingCrm.name;
                    local.Type = checkType(matchingCrm.collectionIds);
                    local.ServiceType = checkServiceType(matchingCrm.collectionIds);
                    local.IsEmergency = isEmergency(matchingCrm.collectionIds);
                    shouldUpdate.Add(local);
                }
            }
            if (crmProducts != null)
            {
                foreach (var crm in crmProducts)
                {
                    if (!localProducts.Any(l => l.ServiceCrmId == crm._id))
                    {
                        shouldAdd.Add(new Product
                        {
                            CollectionIds = string.Join(",", crm.collectionIds),
                            Enable = true,
                            ServiceCrmId = crm._id,
                            Description = crm.description,
                            Name = crm.name,
                            ProductType = crm.productType,
                            Type= checkType(crm.collectionIds),
                            ServiceType = checkServiceType(crm.collectionIds),
                            IsEmergency = isEmergency(crm.collectionIds),
                        }
                        );
                    }
                }
            }

            _dbContext.Product.AddRange(shouldAdd);
            _dbContext.Product.UpdateRange(shouldUpdate);
            _dbContext.SaveChanges();

        }

        private int? checkType(List<string> collectionIds)
        {
            if (collectionIds.Contains("671826be39575cddc96abf50"))
            {
                return (int)ProductCollection.Service;
            }
            if (collectionIds.Contains("671826d564d8053aafd1ca63"))
            {
                return (int)ProductCollection.Product;
            }
            return (int)ProductCollection.Service;
        }

        private int? checkServiceType(List<string> collectionIds)
        {
            if (collectionIds.Contains("6720da386747d963d6dc64bd"))
            {
                return (int)ServiceType.Cooking_Oil_Collection;
            }
            if (collectionIds.Contains("6720da4c6747d93389dc64de"))
            {
                return (int)ServiceType.Grease_Trap_Management;
            }
            return (int)ServiceType.Other;
        }

        private bool isEmergency(List<string> collectionIds)
        {
            if (collectionIds.Contains("671826ac39575cbf746abf37"))
            {
                return true;
            }

            return false;
        }

            private void syncPrice(string producCrmtId, long productId)
        {

                var crmProducts = _priceApi.call(producCrmtId).Data;
                var localProducts = _dbContext.ProductPrice.Where(p=>p.ProductId == productId).ToList();

                List<ProductPrice> shouldAdd = new List<ProductPrice>();
                List<ProductPrice> shouldUpdate = new List<ProductPrice>();
                foreach (var local in localProducts)
                {
                    var matchingCrm = crmProducts.FirstOrDefault(c => c._id == local.ServicePriceCrmId);

                    if (matchingCrm == null)
                    {
                        local.Enable = false;
                        shouldUpdate.Add(local);
                    }
                    else if (!matchingCrm.Equals(local))
                    {
                        local.Enable = true;
                        local.Name = matchingCrm.name;
                        local.Amount = double.Parse(matchingCrm.amount);
                        shouldUpdate.Add(local);
                    }
                }
            if (crmProducts != null)
            {
                foreach (var crm in crmProducts)
                {
                    if (!localProducts.Any(l => l.ServicePriceCrmId == crm._id))
                    {
                        shouldAdd.Add(new ProductPrice
                        {
                            ServicePriceCrmId = crm._id,
                            Name = crm.name,
                            Amount = double.Parse(crm.amount),
                            Enable = true,
                            ProductId = productId,
                            ServiceCrmId = producCrmtId
                        }
                        );
                    }
                }
            }
                _dbContext.ProductPrice.AddRange(shouldAdd);
                _dbContext.ProductPrice.UpdateRange(shouldUpdate);
            _dbContext.SaveChanges();
        }
    }
}

