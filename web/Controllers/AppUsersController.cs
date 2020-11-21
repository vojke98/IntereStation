using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web.Data;
using web.Models;

namespace web.Controllers
{
    [Authorize]
    public class AppUsersController : Controller
    {
        private readonly ISDBContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _hostEnvironment;

        public AppUsersController(ISDBContext context, UserManager<AppUser> userManager, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var users = from u in _context.Users select u;
            var uId = _userManager.GetUserId(User);
            users = users.Except(users.Where(u => u.Id.Equals(uId)));

            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.UserName.Contains(searchString) || u.FirstName.Contains(searchString) || u.LastName.Contains(searchString));
            }

            return View(await users.ToListAsync());
        }

        // GET: AppUsers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            var uId =  _userManager.GetUserId(User);
            ViewData["CurrentUserId"] = uId;
            
            if (id == null)
            {
                id = uId;
            } 

            var appUser = await _context.Users.Include(r => r.ReceievedFriendRequests).Include(s => s.SentFriendRequests)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appUser == null)
            {
                return NotFound();
            }

            return View(appUser);
        }

        // GET: AppUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AppUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(appUser);
        }


        public async Task<IActionResult> SendFriendRequest(string UserId, bool accept = true)
        {
            var CurrentUserId = _userManager.GetUserId(User);

            Friend requestSent = await _context.Friends.FindAsync(CurrentUserId, UserId);
            Friend requestRecieved = await _context.Friends.FindAsync(UserId, CurrentUserId);
    
            if (ModelState.IsValid)
            {
                
                if(requestSent == null && requestRecieved == null){
                    Friend friend = new Friend{ RequestedById = CurrentUserId, RequestedToId = UserId, RequestTime = DateTime.Now, FriendRequestFlag = FriendRequestFlag.Pending };
                    _context.Add(friend);
                }else if(requestSent != null){
                    _context.Remove(requestSent);
                }else if(requestRecieved != null && accept){
                    requestRecieved.FriendRequestFlag = FriendRequestFlag.Approved;
                    _context.Update(requestRecieved);
                }else if(requestRecieved != null && !accept){
                    _context.Remove(requestRecieved);
                }else{
                    //mislim da su to sve opcije
                }
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction("Details", "AppUsers", new {id = UserId});
        }


        // GET: AppUsers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appUser = await _context.Users.FindAsync(id);
            if (appUser == null)
            {
                return NotFound();
            }
            return View(appUser);
        }

        // POST: AppUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("FirstName,LastName,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] AppUser appUser)
        {
            if (id != appUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppUserExists(appUser.Id))
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
            return View(appUser);
        }

        // GET: AppUsers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appUser = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appUser == null)
            {
                return NotFound();
            }

            return View(appUser);
        }

        // POST: AppUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var appUser = await _context.Users.FindAsync(id);
            _context.Users.Remove(appUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppUserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }


        // GET: AppUsers/Edit/5
        public async Task<IActionResult> SetProfilePicture()
        {
            string id = _userManager.GetUserId(User);
            if (id == null)
            {
                return NotFound();
            }

            var appUser = await _context.Users.FindAsync(id);
            if (appUser == null)
            {
                return NotFound();
            }
            return View(appUser);
        }

        // POST: AppUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetProfilePicture(string id, [Bind("FirstName,LastName,ImageFile,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] AppUser appUser)
        {
            //string id = _userManager.GetUserId(User);
            
            if (ModelState.IsValid)
            {
                try
                {
                    string wwwroot = _hostEnvironment.WebRootPath;

                    if(appUser.ImageFile != null){
                        string fileName = Path.GetFileNameWithoutExtension(appUser.ImageFile.FileName);
                        string extension = Path.GetExtension(appUser.ImageFile.FileName);
                        appUser.profilePic = "profilePic" + extension;

                        string path = Path.Combine(wwwroot,"userFiles" , appUser.Id, "profilePic" + extension);
                        Directory.CreateDirectory(Path.GetDirectoryName(path));
                        using(var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await appUser.ImageFile.CopyToAsync(fileStream);
                        }
                    }

                    _context.Update(appUser);
                    await _context.SaveChangesAsync();


                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppUserExists(appUser.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("SetProfilePicture", "AppUsers");
                //return RedirectToAction(nameof(Index));
            }  
            return View(appUser);
        }
    }
}
