using Chinook.ClientModels;

namespace Chinook.Services
{
    public interface IPlaylistService
    {
        Task<Playlist?> GetPlaylistAsync(long playlistId, string userId);
        Task<Models.Playlist> GetOrCreateFavoritesPlaylistAsync(string userId);
    }
}
