using Snackis.Core.Entities;

namespace Snackis.Core.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetCategoriesAsync();

        Task<Category?> GetCategoryByIdAsync(int id);

        Task AddCategoryAsync(Category category);

        Task UpdateCategoryAsync(Category category);

        Task<bool> DeleteCategoryAsync(int id);
    }
}