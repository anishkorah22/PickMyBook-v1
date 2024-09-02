using Experion.PickMyBook.Infrastructure.Models;

﻿using Experion.PickMyBook.Infrastructure.Models.DTO;


public interface IRequestService
{
    Task<IEnumerable<RequestDTO>> GetAllRequestsAsync();
    Task<Request> CreateBorrowRequestAsync(int bookId, int userId);
    Task<Request> CreateReturnRequestAsync(int bookId, int userId);
    Task<Request> ApproveRequestAsync(int requestId);
    Task<Request> DeclineRequestAsync(int requestId, string message);
    Task<IEnumerable<UserRequestsDTO>> GetRequestsByUserAsync(int userId);
}
