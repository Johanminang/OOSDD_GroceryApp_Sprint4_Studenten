using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class GroceryListItemsService : IGroceryListItemsService
    {
        private readonly IGroceryListItemsRepository _groceriesRepository;
        private readonly IProductRepository _productRepository;

        public GroceryListItemsService(IGroceryListItemsRepository groceriesRepository, IProductRepository productRepository)
        {
            _groceriesRepository = groceriesRepository;
            _productRepository = productRepository;
        }

        public List<GroceryListItem> GetAll()
        {
            List<GroceryListItem> groceryListItems = _groceriesRepository.GetAll();
            FillService(groceryListItems);
            return groceryListItems;
        }

        public List<GroceryListItem> GetAllOnGroceryListId(int groceryListId)
        {
            List<GroceryListItem> groceryListItems = _groceriesRepository.GetAll().Where(g => g.GroceryListId == groceryListId).ToList();
            FillService(groceryListItems);
            return groceryListItems;
        }

        public GroceryListItem Add(GroceryListItem item)
        {
            return _groceriesRepository.Add(item);
        }

        public GroceryListItem? Delete(GroceryListItem item)
        {
            throw new NotImplementedException();
        }

        public GroceryListItem? Get(int id)
        {
            return _groceriesRepository.Get(id);
        }

        public GroceryListItem? Update(GroceryListItem item)
        {
            return _groceriesRepository.Update(item);
        }

        public List<BestSellingProducts> GetBestSellingProducts(int topX = 5)
        {
            // Haalt alle boodschappenlijst items op
            List<GroceryListItem> allItems = _groceriesRepository.GetAll(); 

            // Groepeer items per product
            List<BestSellingProducts> bestSellers = allItems 
                .GroupBy(item => item.ProductId)
                .Select(group => CreateBestSellingProduct(group))
                .OrderByDescending(product => product.NrOfSells)
                .Take(topX) 
                .ToList(); 

            AssignRankings(bestSellers);

            return bestSellers;
        }


        private BestSellingProducts CreateBestSellingProduct(IGrouping<int, GroceryListItem> group)
        {
            // Haalt productinformatie op 
            Product? product = _productRepository.Get(group.Key);
            int totalSells = group.Sum(item => item.Amount);

            // Maakt een BestSellingProduct object aan met fallback waardes 
            return new BestSellingProducts(
                productId: group.Key,
                name: product?.Name ?? "Onbekend product",
                stock: product?.Stock ?? 0,
                nrOfSells: totalSells,
                ranking: 0
            );
        }

        private void AssignRankings(List<BestSellingProducts> products)
        {
            // Loopt door alle producten en maakt een ranking 
            for (int i = 0; i < products.Count; i++)
            {
                products[i].Ranking = i + 1;
            }
        }

        private void FillService(List<GroceryListItem> groceryListItems)
        {
            foreach (GroceryListItem g in groceryListItems)
            {
                g.Product = _productRepository.Get(g.ProductId) ?? new(0, "", 0);
            }
        }
    }
}
