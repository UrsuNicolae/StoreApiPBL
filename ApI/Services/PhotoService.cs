using ApI.DTOs;
using ApI.Helpers;
using ApI.Models;
using ApI.Models.Data;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApI.Services
{
    public class PhotoService //IPhotoService
    {
        /*private readonly IMapper _mapper;
        private readonly APIContext _context;
        private IOptions<CloudinarySettings> _cloudinaryConfig;

        private Cloudinary _cloudinary;
        public PhotoService(IMapper mapper,
            IOptions<CloudinarySettings> cloudinaryConfig,
            APIContext context)
        {
            _mapper = mapper;
            _cloudinaryConfig = cloudinaryConfig;
            _context = context;

            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);

        }
        public async Task<bool> AddPhotoForProduct(int proudctId, PhotoForCreationDTO photoForCreatingDto)
        {

            var productFromDb = await _context.Products.FirstOrDefaultAsync(x => x.Id == proudctId);

            var file = photoForCreatingDto.File;

            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream)
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            photoForCreatingDto.Url = uploadResult.Url.ToString();
            photoForCreatingDto.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoForCreatingDto);

            productFromDb.Photos.Add(photo);

            await _context.SaveChangesAsync();

            return true;
        }

        public Task<bool> UpdatePhotoForProduct(int productId, PhotoForUpdatingDTO photoForUpdatingDto)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeletePhoto(int productId, int id)
        {
            var product = await _context.Products.Include(i => i.Photos).FirstOrDefaultAsync(x => x.Id == productId);

            if (product.Photos.Any(p => p.Id == id))
                return false;

            var photoFromRepo = await _context.Photos.FirstOrDefaultAsync(x => x.Id == id);

            if (photoFromRepo == null)
                return false;

            if (photoFromRepo.PublicId != null)
            {
                var deleteParams = new DeletionParams(photoFromRepo.PublicId);

                var result = _cloudinary.Destroy(deleteParams);

                if (result.Result == "ok")
                {
                    _context.Photos.Remove(photoFromRepo);
                }
            }

            if (photoFromRepo.PublicId == null)
            {
                _context.Photos.Remove(photoFromRepo);
            }

            var changes = await _context.SaveChangesAsync();
            if (changes != 0)
                return true;

            return false;
        }*/
    }
}
