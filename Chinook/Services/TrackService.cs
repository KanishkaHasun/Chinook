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
        public async Task<List<PlaylistTrack>?> GetTracksForArtistAsync(long artistId, string currentUserId)
        {
            try
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
                     .Any(p => p.UserPlaylists.Any(up => up.UserId == currentUserId && up.Playlist.Name == "My favorite tracks"))
             })
             .ToListAsync();
            }
            catch (Exception ex)
            {
                //need to log
                return null;
            }

        }
      
        public async Task<bool> FavoriteTrackAsync(long trackId, string userId)
        {
            try
            {
                var playlist = await _playlistService.GetOrCreateFavoritesPlaylistAsync(userId);

                if (playlist != null && !playlist.Tracks.Any(pt => pt.TrackId == trackId))
                {
                        var track = await _dbContext.Tracks.FindAsync(trackId);
                        if (track != null)
                        {
                            playlist.Tracks.Add(track);
                            await _dbContext.SaveChangesAsync();
                            return true;
                        }
                }
                return false;
            }
            catch (Exception ex)
            {
                // I need to log this
                return false;
            }
        }
        public async Task<bool> UnfavoriteTrackAsync(long trackId, string userId)
        {
            try
            {
                var playlist = await _playlistService.GetOrCreateFavoritesPlaylistAsync(userId);

                if (playlist != null)
                {
                    var playlistTrack = playlist.Tracks.FirstOrDefault(pt => pt.TrackId == trackId);

                    if (playlistTrack != null)
                    {
                        playlist.Tracks.Remove(playlistTrack);
                        await _dbContext.SaveChangesAsync();
                        return true;
                    }

                }
                return false;
            }
            catch (Exception ex)
            {
                // Need to log this
                return false;
            }
        }


    }
}
