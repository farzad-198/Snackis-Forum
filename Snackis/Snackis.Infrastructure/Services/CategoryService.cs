using Snackis.Core.Entities;
using Snackis.Core.Interfaces;

namespace Snackis.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category>
            _categoryRepository;

        public CategoryService(
            IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }


        public async Task<List<Category>>
            GetCategoriesAsync()
        {
            return await
                _categoryRepository.GetAllAsync();
        }


        public async Task<Category?>
            GetCategoryByIdAsync(int id)
        {
            return await
                _categoryRepository.GetByIdAsync(id);
        }


        public async Task AddCategoryAsync(
            Category category)
        {
            await _categoryRepository.AddAsync(
                category);

            await _categoryRepository.SaveChangesAsync();
        }


        public async Task UpdateCategoryAsync(
            Category category)
        {
            _categoryRepository.Update(category);

            await _categoryRepository.SaveChangesAsync();
        }


        public async Task<bool> DeleteCategoryAsync(
            int id)
        {
            Category? category =
                await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                return false;
            }

            _categoryRepository.Delete(category);

            await _categoryRepository.SaveChangesAsync();

            return true;
        }
    }
}