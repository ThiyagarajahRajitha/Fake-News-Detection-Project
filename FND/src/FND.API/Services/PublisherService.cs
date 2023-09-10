using FND.API.Data.Dtos;
using FND.API.Data.Repositories;
using FND.API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FND.API.Services
{
    public interface PublisherService
    {

        public Task<List<Users>> GetApprovedPublication();

        public Task<List<Users>> GetPublishers([FromQuery(Name = "PendingApprovalOnly")] bool IsPendingOnly);

        public Task UpdatePublisherAsync(int id, ActivatePublisherDto activatePublisher);
        public Task<bool> RejectPublisher(int id, RejectPublisherDto rejectPublisherDto);
        public void updateLastFetchedNews(int publication_Id, string newsUrl);
        public Task<bool> DeletePublisher(int id);
    }
}