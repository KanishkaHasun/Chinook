using Chinook.ClientModels;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Services
{
    public class PlaylistService:IPlaylistService
    {
        private readonly ChinookContext _dbContext;
        public PlaylistService(ChinookContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Playlist?> GetPlaylistAsync(long playlistId, string userId)
        {
            Playlist? playlist = null;

            try
            {
                playlist = await _dbContext.Playlists
                    .Include(a => a.Tracks).ThenInclude(a => a.Album).ThenInclude(a => a.Artist)
                    .Where(p => p.PlaylistId == playlistId)
                    .Select(p => new Playlist()
                    {
                        Name = p.Name,
                        Tracks = p.Tracks.Select(t => new PlaylistTrack()
                        {
                            AlbumTitle = t.Album.Title,
                            ArtistName = t.Album.Artist.Name,
                            TrackId = t.TrackId,
                            TrackName = t.Name,
                            IsFavorite = t.Playlists
                                .Where(p => p.UserPlaylists.Any(up => up.UserId == userId && up.Playlist.Name == "Favorites"))
                                .Any()
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();

                if (playlist == null)
                {
                    throw new Exception($"Playlist with ID {playlistId} not found.");
                }
            }
            catch (Exception ex)
            {
                // Need to log the exception
            }

            return playlist;
        }

     
    }
}
