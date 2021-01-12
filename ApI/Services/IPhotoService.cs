using ApI.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApI.Services
{
    public interface IPhotoService
    {
        Task<bool> AddPhotoForProduct(int proudctId, PhotoForCreationDTO photoForCreatingDto);

        Task<bool> UpdatePhotoForProduct(int productId, PhotoForUpdatingDTO photoForUpdatingDto);

        Task<bool> DeletePhoto(int productId, int id);
    }
}
