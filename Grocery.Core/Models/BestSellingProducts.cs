
using CommunityToolkit.Mvvm.ComponentModel;

namespace Grocery.Core.Models
{
    public partial class BestSellingProducts : Model
    {
        public int Stock { get; set; }
        [ObservableProperty]
        public int nrOfSells;
        [ObservableProperty]
        public int ranking;
        [ObservableProperty]
        public int productId;
        [ObservableProperty]
        public string name;

        public BestSellingProducts(int productId, string name, int stock, int nrOfSells, int ranking) : base(productId, name)
        {
            ProductId=productId;
            Name=name;
            Stock=stock;
            NrOfSells=nrOfSells;
            Ranking=ranking;
        }
    }
}
