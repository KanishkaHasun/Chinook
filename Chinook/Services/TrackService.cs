using Chinook.ClientModels;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Services
{
    public class TrackService: ITrackService
    {
        private readonly ChinookContext _dbContext;
        public TrackService(ChinookContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<PlaylistTrack>> GetTracksForArtistAsync(long artistId, string currentUserId)
        {
            return await _dbContext.Tracks
                .Where(t => t.Album.ArtistId == artistId)
                .Include(t => t.Album)
                .Select(t => new PlaylistTrack
                {
                    AlbumTitle = (t.Album == null ? "-" : t.Album.Title),
                    TrackId = t.TrackId,
                    TrackName = t.Name,
                    IsFavorite = t.Playlists
                        .Any(p => p.UserPlaylists.Any(up => up.UserId == currentUserId && up.Playlist.Name == "Favorites"))
                })
                .ToListAsync();
        }
        public async Task FavoriteTrackAsync(long trackId, string userId)
        {
            // Implement favorite track logic
        }

        public async Task UnfavoriteTrackAsync(long trackId, string userId)
        {
            // Implement unfavorite track logic
        }

        public async Task RemoveTrackAsync(long trackId)
        {
            // Implement remove track logic
        }

    }
}
