using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Chinook.Services
{
    public class ArtistService : IArtistService
    {
        private readonly ChinookContext _dbContext;
        private readonly AuthenticationStateProvider _authenticationState;

        public ArtistService(ChinookContext dbContext, AuthenticationStateProvider authenticationState)
        {
            _dbContext = dbContext;
            _authenticationState = authenticationState;

        }
       
        public async Task<List<Models.Artist>> GetArtistsAsync()
        {
            return await _dbContext.Artists.ToListAsync();
        }
      
        public async Task<Models.Artist> GetArtistAsync(long artistId)
        {
            var artist = await _dbContext.Artists.SingleOrDefaultAsync(a => a.ArtistId == artistId);
            if (artist == null)
            {
                throw new Exception($"Artist with ID {artistId} not found.");
            }
            return artist;
        }
        public async Task<List<ClientModels.Artist>> GetArtistsWithAlbumCount()
        {
            return await _dbContext.Artists
                .Select(a => new ClientModels.Artist
                {
                    ArtistId = a.ArtistId,
                    Name = a.Name,
                    AlbumCount = a.Albums.Count()
                })
                .ToListAsync();
        }


        public async Task<string?> GetUserIdAsync()
        {
            var user = (await _authenticationState.GetAuthenticationStateAsync())?.User;
            var userId = user?.FindFirst(u => u.Type.Contains(ClaimTypes.NameIdentifier))?.Value;
            return userId;
        }

    }

}
