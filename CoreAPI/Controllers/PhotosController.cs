using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CoreAPI.Dto;
using CoreAPI.Helpers;
using CoreAPI.Models;
using CoreAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CoreAPI.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig; // this is how to use service imp
        private Cloudinary _cloudinary;
        public PhotosController(IDatingRepository repo, IMapper mapper,
                                 IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _cloudinaryConfig = cloudinaryConfig;
            _mapper = mapper;
            _repo = repo;

            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }
        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepo = await _repo.GetPhoto(id);

            var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);

            return Ok(photo);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId, [FromForm]PhotoForCreationDto photoForCreationDto)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _repo.GetUser(userId);
            var file = photoForCreationDto.File;

            var uploadResult = new ImageUploadResult(); // from cloudinary result.

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }
        // fill result received from cloudinary to photoForCreationDto
            photoForCreationDto.Url = uploadResult.Uri.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoForCreationDto);

            if (!userFromRepo.Photos.Any(u => u.IsMain)) // if photo is first photo of user mark it as main photo
                photo.IsMain = true;

            userFromRepo.Photos.Add(photo);

            if (await _repo.SaveAll())
            {
               
             var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);
             return CreatedAtRoute("GetPhoto", new { userId = userId, id = photo.Id }, photoToReturn);
            };

            return BadRequest("Could not add the photo");
        }

        // since only one property is getting updated we are using httpPost
        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMain(int userId, int id) // id is photo id
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var user = await _repo.GetUser(userId);

            if (!user.Photos.Any(p => p.Id == id))
                return Unauthorized(); 

            var photoFromRepo = await _repo.GetPhoto(id);

            if (photoFromRepo.IsMain)
                return BadRequest("This is already the main photo");

            var currentMainPhoto = await _repo.GetMainPhotoForUser(userId);
            currentMainPhoto.IsMain = false;   // set already main photo to false 

            photoFromRepo.IsMain = true; // set new photo as main

            if (await _repo.SaveAll())
                return NoContent();

            return BadRequest("Could not set photo to main");
        }

        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeletePhoto(int userId, int id)
        // {
        //     if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
        //         return Unauthorized();

        //     var user = await _repo.GetUser(userId);

        //     if (!user.Photos.Any(p => p.Id == id))
        //         return Unauthorized();

        //     var photoFromRepo = await _repo.GetPhoto(id);

        //     if (photoFromRepo.IsMain)
        //         return BadRequest("You cannot delete your main photo");


        //     if (photoFromRepo.PublicId != null)
        //     {
        //         var deleteParams = new DeletionParams(photoFromRepo.PublicId);
        //         var result = _cloudinary.Destroy(deleteParams);

        //         if (result.Result == "ok")
        //         {
        //             _repo.Delete(photoFromRepo);
        //         }
        //     }

        //     if (photoFromRepo.PublicId == null)
        //     {
        //         _repo.Delete(photoFromRepo);
        //     }

        //     if (await _repo.SaveAll())
        //         return Ok();

        //     return BadRequest("Failed to delete photo");
        // }
    }
}