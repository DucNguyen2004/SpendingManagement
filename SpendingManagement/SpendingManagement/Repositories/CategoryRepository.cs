using SpendingManagement.DAOs;
using SpendingManagement.Models;

namespace SpendingManagement.Repositories
{
    public class CategoryRepository
    {
        private readonly CategoryDAO categoryDAO;
        public CategoryRepository(CategoryDAO context)
        {
            categoryDAO = context;
        }
        public List<Category> GetIncomeCategories()
        {
            return categoryDAO.GetIncomeCategories();
        }
        public List<Category> GetExpenseCategories()
        {
            return categoryDAO.GetExpenseCategories();
        }
        public Category GetCategory(int id)
        {
            return categoryDAO.GetCategory(id);
        }
    }
}
