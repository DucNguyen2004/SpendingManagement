using SpendingManagement.Models;
using SpendingManagement.Repositories;

namespace SpendingManagement.Services
{
    public class CategoryService
    {
        private readonly CategoryRepository categoryRepo;
        public CategoryService(CategoryRepository categoryRepo)
        {
            this.categoryRepo = categoryRepo;
        }
        public List<Category> GetIncomeCategories()
        {
            return categoryRepo.GetIncomeCategories();
        }
        public List<Category> GetExpenseCategories()
        {
            return categoryRepo.GetExpenseCategories();
        }
        public Category GetCategory(int id)
        {
            return categoryRepo.GetCategory(id);
        }
    }
}
