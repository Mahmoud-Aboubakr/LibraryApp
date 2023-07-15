using Application.DTOs;
using Application.Interfaces;
using Application.Validators;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AuthorService : IAuthorService
    {
        public IUnitOfWork _unitOfWork;

        public AuthorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void CreateProductAsync(Author author)
        {
            _unitOfWork.Authors.InsertAsync(author);
        }

        public void DeleteAuthorAsync(Author author)
        {
            _unitOfWork.Authors.DeleteAsync(author);
        }

        public void UpdateAuthorAsync(Author author)
        {
            _unitOfWork.Authors.DeleteAsync(author);
        }

        public async Task<List<Author>> GetAllAuthorsAsync()
        {
            return await _unitOfWork.Authors.GetAllAsync();
        }

        public async Task<Author> GetAuthorByIdAsync(int id)
        {
            return await _unitOfWork.Authors.GetByIdAsync(id);
        }

    }
}
