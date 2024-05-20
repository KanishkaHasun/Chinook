using Chinook.ClientModels;

namespace Chinook.Services
{
    public interface ITrackService
    {
        
        Task<List<PlaylistTrack>?> GetTracksForArtistAsync(long artistId, string currentUserId);
        Task<bool> FavoriteTrackAsync(long trackId, string userId);
        Task<bool> UnfavoriteTrackAsync(long trackId, string userId);


    }
}
