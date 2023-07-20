using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IAppServices
{
    public interface IPublisherServices
    {
        Task<IReadOnlyList<ReadPublisherDto>> SearchWithCriteria(string Name = null, string PhoneNumber = null);
    }
}
