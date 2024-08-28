﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Experion.PickMyBook.Data;

namespace Experion.PickMyBook.Data.IRepository
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllAsync();
        Task<Book> GetByIdAsync(int id);
        Task AddAsync(Book entity);
        Task UpdateAsync(Book entity);
        Task DeleteAsync(int id);
    }
}

