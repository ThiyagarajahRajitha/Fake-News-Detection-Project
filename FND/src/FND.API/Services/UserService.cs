using FND.API.Data;
using FND.API.Data.Dtos;
using FND.API.Data.Repositories;
using FND.API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FND.API.Services
{
    public class UserService
    {
        private readonly UserRepository _repository;
       
        public UserService(FNDDBContext fNDDBContext)
        {
            _repository = new UserRepository(fNDDBContext);
        }
        public async Task<Users> CreateUser([FromBody] SignUpRequestDto signUpRequest)
        {
            var user = await _repository.CreateUser(signUpRequest);
            return user;
        }
        public async Task<Users> GetUserById(int id)
        {
            var newsList = await _repository.GetUserById(id);
            return newsList;
        }
        public async Task<List<Users>> GetModerators(bool isPendingOnly)
        {
            return await _repository.GetModerators(isPendingOnly);
        }
        public async Task<bool> DeleteModeratorUser(int id)
        {
            bool rslt = await _repository.DeleteModeratorUser(id);
            return rslt;
        }
        public async Task<List<ReviewRequestCountByModeratorDashboardresultDto>> GetReviewRequestCountByModerator(int userId, string fromDate, string toDate)
        {
            var reviewRequestCountByModerator = await _repository.GetReviewRequestCountByModerator(userId, fromDate, toDate);
            return reviewRequestCountByModerator;
        }
    }
}
