using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Infrastructure.AppServices
{
    public class SearchAuthorDataService : ISearchAuthorDataService
    {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;

        public SearchAuthorDataService(LibraryDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<ReadAuthorDto>> SearchWithCriteria(string name = null, string phone = null)
        {
            var query = _context.Set<Author>().AsQueryable();

            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(phone))
            {
                query = query.Where(A => A.AuthorName.Contains(name) && A.AuthorPhoneNumber.Contains(phone));
            }
            else if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(t => t.AuthorName.Contains(name));
            }
            else if (!string.IsNullOrEmpty(phone))
            {
                query = query.Where(t => t.AuthorPhoneNumber.Contains(phone));
            }
            var result = await query.ToListAsync();
            return _mapper.Map<IReadOnlyList<Author>, IReadOnlyList<ReadAuthorDto>>(result);
        }
    }

}
