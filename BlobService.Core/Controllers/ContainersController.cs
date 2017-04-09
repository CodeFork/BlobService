﻿using BlobService.Core.Entities;
using BlobService.Core.Models;
using BlobService.Core.Stores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlobService.Core.Controllers
{
    public class ContainersController : Controller
    {
        protected readonly BlobServiceOptions _options;
        protected readonly ILogger _logger;
        protected readonly IContainerMetaStore _containerMetaStore;

        public ContainersController(
            BlobServiceOptions options,
            ILogger<ContainersController> logger,
            IContainerMetaStore containerMetaStore)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _containerMetaStore = containerMetaStore ?? throw new ArgumentNullException(nameof(containerMetaStore));
        }

        [HttpGet("/containers")]
        public async Task<IEnumerable<ContainerViewModel>> GetAllContainersAsync()
        {
            var containersMetas = await _containerMetaStore.GetAllAsync();

            var containerModels = ModelMapper.ToViewModelList(containersMetas);

            return containerModels;
        }

        [HttpGet("/containers/{id}")]
        public async Task<IActionResult> GetContainerByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var containerMeta = await _containerMetaStore.GetAsync(id);

            if (containerMeta == null) return NotFound();

            var containerModel = ModelMapper.ToViewModel(containerMeta);

            return Ok(containerModel);
        }

        [HttpGet("/containers?name={name}")]
        public async Task<IActionResult> GetContainerByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name)) return NotFound();

            var containerMeta = await _containerMetaStore.GetByNameAsync(name);

            if (containerMeta == null) return NotFound();

            var containerModel = ModelMapper.ToViewModel(containerMeta);

            return Ok(containerModel);
        }

        [HttpPost("/containers")]
        public async Task<IActionResult> AddContainerAsync([FromBody]ContainerCreateModel model)
        {
            if (model == null) return BadRequest();

            var containerMeta = await _containerMetaStore.AddAsync(model);

            var containerModel = ModelMapper.ToViewModel(containerMeta);

            return Ok(containerModel);
        }

        [HttpPut("/containers/{id}")]
        public async Task<IActionResult> UpdateContainerAsync(string id, [FromBody]ContainerCreateModel model)
        {
            if (model == null) return BadRequest();

            var container = await _containerMetaStore.GetAsync(id);

            if (container == null) return NotFound();

            container.Name = model.Name;

            var containerMeta = await _containerMetaStore.UpdateAsync(id, container);
            var containerModel = ModelMapper.ToViewModel(containerMeta);

            return Ok(containerModel);
        }

        [HttpDelete("/containers/{id}")]
        public async Task<IActionResult> DeleteContainerAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var containerMeta = _containerMetaStore.GetAsync(id);

            if (containerMeta != null)
            {
                await _containerMetaStore.RemoveAsync(id);
            }

            return Ok();
        }


        [HttpGet("/containers/{id}/blobs")]
        public async Task<IActionResult> ListBlobsAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var blobsMetas = await _containerMetaStore.GetBlobsAsync(id);

            if (blobsMetas == null) return NotFound();

            var blobsModel = ModelMapper.ToViewModelList(blobsMetas);

            return Ok(blobsModel);
        }
    }
}
