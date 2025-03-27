using SpendingManagement.Models;

namespace SpendingManagement.DAOs
{
    public class CategoryDAO
    {
        private readonly SpendingManagementContext _context;
        public CategoryDAO(SpendingManagementContext context)
        {
            _context = context;
        }
        public List<Category> GetIncomeCategories()
        {
            return _context.Categories.Where(c => c.Type.ToLower() == "income").ToList();
        }
        public List<Category> GetExpenseCategories()
        {
            return _context.Categories.Where(c => c.Type.ToLower() == "expense").ToList();
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.FirstOrDefault(c => c.Id == id);
        }
    }
}
