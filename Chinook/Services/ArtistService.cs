using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public async Task<List<Models.Artist>?> GetArtistsAsync()
        {
            try
            {
                return await _dbContext.Artists.ToListAsync();
            }
            catch (Exception ex)
            {
                //need to log
                return null;
            }

        }

        public async Task<Models.Artist?> GetArtistAsync(long artistId)
        {
            try
            {
                return await _dbContext.Artists.SingleOrDefaultAsync(a => a.ArtistId == artistId);

            }
            catch (Exception ex)
            {
                //need to log
                return null;
            }
        }
        public async Task<List<ClientModels.Artist>?> GetArtistsWithAlbumCount()
        {
            try
            {
                return await _dbContext.Artists.Select(a => new ClientModels.Artist
                {
                    ArtistId = a.ArtistId,
                    Name = a.Name,
                    AlbumCount = a.Albums.Count()
                })
              .ToListAsync();
            }
            catch (Exception ex)
            {
                //need to log
                return null;
            }

        }

        public async Task<string?> GetUserIdAsync()
        {
            try
            {
                var user = (await _authenticationState.GetAuthenticationStateAsync())?.User;
                var userId = user?.FindFirst(u => u.Type.Contains(ClaimTypes.NameIdentifier))?.Value;
                return userId;
            }
            catch (Exception ex)
            {
                //need to log
                return null;
            }

        }

        public async Task<List<ClientModels.Artist>?> SearchByNameAsync(string searchQuery)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    return await _dbContext.Artists
                        .Where(a => EF.Functions.Like(a.Name, $"%{searchQuery}%")).Select(a => new ClientModels.Artist
                        {
                            ArtistId = a.ArtistId,
                            Name = a.Name,
                            AlbumCount = a.Albums.Count()
                        })
              .ToListAsync();
                }
                return null;
            }
            catch (Exception ex)
            {
                //need to log
                return null;
            }
          
        }

    }

}
