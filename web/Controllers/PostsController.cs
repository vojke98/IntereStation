using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web.Data;
using web.Models;

namespace web.Controllers
{   
    [Authorize]
    public class PostsController : Controller
    {
        private readonly ISDBContext _context;
        private readonly UserManager<AppUser> _userManager;
        
        public PostsController(ISDBContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            var uId =  _userManager.GetUserId(User);
            ViewData["CurrentUserId"] = uId;            

            var iSDBContext = _context.Posts.Include(p => p.Owner).Include(p => p.Type).OrderByDescending(p => p.DateCreated).Include(p => p.Likes);
            return View(await iSDBContext.ToListAsync());
        }

        // GET: Posts
        public async Task<IActionResult> UserPosts()
        {
            var uId =  _userManager.GetUserId(User);     
            ViewData["CurrentUserId"] = uId;  
            
            var iSDBContext = _context.Posts
                .Include(p => p.Owner)
                .Include(p => p.Type)  
                .Include(p => p.Likes)
                .OrderByDescending(p => p.DateCreated)
                .Where(p => p.OwnerId == uId);
            return View(await iSDBContext.ToListAsync());
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Owner)
                .Include(p => p.Type)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            ViewData["TypeId"] = new SelectList(_context.Interests, "InterestId", "Name");

            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,Text,TypeId,Image")] Post post)
        {
            if (ModelState.IsValid)
            {
                AppUser AppUser = await _userManager.GetUserAsync(HttpContext.User);
                post.Owner = AppUser;
                post.OwnerId = AppUser.Id;
                post.DateCreated = DateTime.Now;
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["TypeId"] = new SelectList(_context.Interests, "InterestId", "Name", post.TypeId);

            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);

            //only owner can edit
            var currentUserId = _userManager.GetUserId(User);
            if(post.OwnerId != currentUserId)
            {
                return RedirectToAction("Index", "Posts");
            }

            if (post == null)
            {
                return NotFound();
            }
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", post.OwnerId);
            ViewData["TypeId"] = new SelectList(_context.Interests, "InterestId", "Name", post.TypeId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,Text,TypeId,DateCreated,Images,OwnerId")] Post post)
        {
            if (id != post.PostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.PostId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            //ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", post.OwnerId);
            ViewData["TypeId"] = new SelectList(_context.Interests, "InterestId", "Name", post.TypeId);
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Owner)
                .Include(p => p.Type)
                .FirstOrDefaultAsync(m => m.PostId == id);

            //only owner can delete
            var currentUserId = _userManager.GetUserId(User);
            if(post.OwnerId != currentUserId)
            {
                return RedirectToAction("Index", "Posts");
            }

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.PostId == id);
        }
    }
}
