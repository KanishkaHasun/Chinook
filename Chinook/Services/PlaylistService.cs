using Chinook.ClientModels;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly ChinookContext _dbContext;
        public PlaylistService(ChinookContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Playlist?> GetPlaylistAsync(long playlistId, string userId)
        {
            try
            {
                return await _dbContext.Playlists
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


            }
            catch (Exception ex)
            {
                // Need to log the exception
                return null;
            }

        }

        public async Task<Models.Playlist?> GetOrCreateFavoritesPlaylistAsync(string userId)
        {
            try
            {
                var playlist = await _dbContext.Playlists
            .Include(p => p.Tracks)
            .Include(p => p.UserPlaylists)
            .FirstOrDefaultAsync(p => p.Name == "My favorite tracks" && p.UserPlaylists.Any(up => up.UserId == userId));

                if (playlist == null)
                {
                    playlist = new Models.Playlist
                    {
                        Name = "My favorite tracks",
                        UserPlaylists = new List<Models.UserPlaylist> { new Models.UserPlaylist { UserId = userId } }
                    };
                    _dbContext.Playlists.Add(playlist);
                    await _dbContext.SaveChangesAsync();
                }

                return playlist;
            }
            catch (Exception ex)
            {
                //need to log
                return null;
            }


        }

    }
}
