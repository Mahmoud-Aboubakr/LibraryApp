using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAuthorService
    {
        void CreateProductAsync(Author author);
        Task<List<Author>> GetAllAuthorsAsync();
        Task<Author> GetAuthorByIdAsync(int id);
        void DeleteAuthorAsync(Author entity);
        void UpdateAuthorAsync(Author entity);
        

        //Task<bool> CreateAuthor(Author author);

        //Task<IEnumerable<Author>> GetAllAuthors();

        //Task<Author> GetAuthorById(int authorId);

        //Task<bool> UpdateAuthor(Author authorDetails);

        //Task<bool> DeleteAuthor(int authorId);
    }
}
