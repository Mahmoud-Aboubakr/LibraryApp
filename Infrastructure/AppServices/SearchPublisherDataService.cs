using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AppServices
{
    public class SearchPublisherDataService : ISearchPublisherDataService
    {
        private readonly LibraryDbContext _context;
        private readonly IMapper _mapper;

        public SearchPublisherDataService(LibraryDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<ReadPublisherDto>> SearchWithCriteria(string Name = null, string PhoneNumber = null)
        {
            var query = _context.Set<Publisher>().AsQueryable();

            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(PhoneNumber))
            {
                query = query.Where(A => A.PublisherName.Contains(Name) && A.PublisherPhoneNumber.Contains(PhoneNumber));
            }
            else if (!string.IsNullOrEmpty(Name))
            {
                query = query.Where(t => t.PublisherName.Contains(Name));
            }
            else if (!string.IsNullOrEmpty(PhoneNumber))
            {
                query = query.Where(t => t.PublisherPhoneNumber.Contains(PhoneNumber));
            }
            var result = await query.ToListAsync();
            return _mapper.Map<IReadOnlyList<Publisher>, IReadOnlyList<ReadPublisherDto>>(result);
        }
    }
}
