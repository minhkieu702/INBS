using INBS.Application.DTOs.ArtistService;
using INBS.Application.DTOs.ArtistStore;
using INBS.Application.DTOs.Store;
using INBS.Application.DTOs.User;
using System.ComponentModel.DataAnnotations;

namespace INBS.Application.DTOs.Artist
{
    public class ArtistResponse
    {
        [Key]
        public Guid ID { get; set; }

        public virtual UserResponse? User { get; set; }

        public string Username { get; set; } = string.Empty;

        public int YearsOfExperience { get; set; }

        public int Level { get; set; }

        public int AverageRating { get; set; }

        public virtual ICollection<ArtistStoreResponse> ArtistStores { get; set; } = [];

        public virtual ICollection<ArtistServiceResponse> ArtistServices { get; set; } = [];
    }
}
