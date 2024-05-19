﻿using Chinook.ClientModels;

namespace Chinook.Services
{
    public interface ITrackService
    {
        
        Task<List<PlaylistTrack>> GetTracksForArtistAsync(long artistId, string currentUserId);
        Task FavoriteTrackAsync(long trackId, string userId);
        Task UnfavoriteTrackAsync(long trackId, string userId);
        Task RemoveTrackAsync(long trackId);


    }
}
