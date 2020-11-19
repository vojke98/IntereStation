using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web.Data;
using web.Models;

namespace web.Controllers
{
    public class LikesController : Controller
    {
        private readonly ISDBContext _context;
        private readonly UserManager<AppUser> _userManager;

        public LikesController(ISDBContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Like/Dislike
        public async Task<IActionResult> Create(int PostId)
        {
            var UserId = _userManager.GetUserId(User);
            Like like = new Like{ UserId = UserId, PostId = PostId };

            Like likeExists = await _context.Likes.FindAsync(UserId, PostId);

            if (ModelState.IsValid)
            {
                if(likeExists == null)
                {
                    _context.Add(like);
                }else
                {
                    _context.Remove(likeExists);
                }
                
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Posts");
        }
    }
}
