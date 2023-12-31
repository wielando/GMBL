﻿using GMBL.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GMBL.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SteamInventoryController : ControllerBase
    {
        private readonly SteamInventoryService _inventoryService;

        public SteamInventoryController(SteamInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }
    }
}