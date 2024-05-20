using Chinook.ClientModels;
using Chinook.Models;

namespace Chinook.Services
{
    public interface IArtistService
    {
        Task<List<Models.Artist>?> GetArtistsAsync();
        Task<Models.Artist>? GetArtistAsync(long artistId);
        Task<List<ClientModels.Artist>?> GetArtistsWithAlbumCount();
        Task<string?> GetUserIdAsync();
        Task<List<ClientModels.Artist>?> SearchByNameAsync(string searchQuery);

    }
}
