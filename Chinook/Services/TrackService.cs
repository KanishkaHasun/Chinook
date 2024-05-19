using Chinook.ClientModels;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Services
{
    public class TrackService: ITrackService
    {
        private readonly ChinookContext _dbContext;
        private readonly IPlaylistService _playlistService;
        public TrackService(ChinookContext dbContext, IPlaylistService playlistService)
        {
            _dbContext = dbContext;
            _playlistService = playlistService;
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
            var playlist = await _playlistService.GetOrCreateFavoritesPlaylistAsync(userId);

            if (!playlist.Tracks.Any(pt => pt.TrackId == trackId))
            {
                playlist.Tracks.Add(new Models.Track { TrackId = trackId });
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task UnfavoriteTrackAsync(long trackId, string userId)
        {

            var playlist = await _playlistService.GetOrCreateFavoritesPlaylistAsync(userId);

            var playlistTrack = playlist.Tracks.FirstOrDefault(pt => pt.TrackId == trackId);
            if (playlistTrack != null)
            {
                playlist.Tracks.Remove(playlistTrack);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task RemoveTrackAsync(long trackId)
        {
            // Implement remove track logic
        }
      

      

    }
}
