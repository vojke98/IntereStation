using web.Models;
using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using System.IO;
using System.Text;

namespace web.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ISDBContext context)
        {
            context.Database.EnsureCreated();
            // Look for any posts.
            /*if (context.UserRoles.Any())
            {
                return;   // DB has been seeded
            }*/

            /*var students = new Student[]
            {
            new Student{FirstMidName="Carson",LastName="Alexander",EnrollmentDate=DateTime.Parse("2005-09-01")},
            new Student{FirstMidName="Meredith",LastName="Alonso",EnrollmentDate=DateTime.Parse("2002-09-01")},
            new Student{FirstMidName="Arturo",LastName="Anand",EnrollmentDate=DateTime.Parse("2003-09-01")},
            new Student{FirstMidName="Gytis",LastName="Barzdukas",EnrollmentDate=DateTime.Parse("2002-09-01")},
            new Student{FirstMidName="Yan",LastName="Li",EnrollmentDate=DateTime.Parse("2002-09-01")},
            new Student{FirstMidName="Peggy",LastName="Justice",EnrollmentDate=DateTime.Parse("2001-09-01")},
            new Student{FirstMidName="Laura",LastName="Norman",EnrollmentDate=DateTime.Parse("2003-09-01")},
            new Student{FirstMidName="Nino",LastName="Olivetto",EnrollmentDate=DateTime.Parse("2005-09-01")}
            };
            foreach (Student s in students)
            {
                context.Students.Add(s);
            }
            context.SaveChanges();*/

            if (!context.Interests.Any())
            {
                var interests = new Interest[]
                {
                    new Interest{Name="Sports"},
                    new Interest{Name="Art"},
                    new Interest{Name="Music"},
                    new Interest{Name="Movie"},
                    new Interest{Name="IT"},
                    new Interest{Name="Nature"},
                    new Interest{Name="Gaming"}
                };

                foreach (Interest i in interests)
                {
                    context.Interests.Add(i);
                }
                context.SaveChanges();
            }

            var roles = new IdentityRole[] {
                new IdentityRole{Id="1", Name="Administrator"},
                new IdentityRole{Id="2", Name="Manager"},
                new IdentityRole{Id="3", Name="Staff"}
            };

            foreach (IdentityRole r in roles)
            {
                context.Roles.Add(r);
            }

            var user = new AppUser
            {
                FirstName = "Bob",
                LastName = "Dilon",
                Email = "bob@example.com",
                NormalizedEmail = "BOB@EXAMPLE.COM",
                UserName = "KingBOB!",
                NormalizedUserName = "KingBOB!",
                PhoneNumber = "+111111111111",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };


            if (!context.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<AppUser>();
                var hashed = password.HashPassword(user,"Testni123!");
                user.PasswordHash = hashed;
                context.Users.Add(user);
                context.SaveChanges();
                
            }

            user = new AppUser
            {
                FirstName = "Dejan",
                LastName = "Vojinović",
                Email = "dejanvojinovic@yahoo.com",
                NormalizedEmail = "DEJANVOJINOVIC@YAHOO.COM",
                UserName = "_vojke",
                NormalizedUserName = "_vojke",
                PhoneNumber = "+38670118252",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            if (!context.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<AppUser>();
                var hashed = password.HashPassword(user,"#Pass123");
                user.PasswordHash = hashed;
                context.Users.Add(user);
                context.SaveChanges();
                
            }

            user = new AppUser
            {
                FirstName = "Dino",
                LastName = "Čeliković",
                Email = "nodi@gmail.com",
                NormalizedEmail = "NODI@GMAIL.COM",
                UserName = "nodi",
                NormalizedUserName = "nodi",
                PhoneNumber = "+38670111111",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            if (!context.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<AppUser>();
                var hashed = password.HashPassword(user,"#Pass123");
                user.PasswordHash = hashed;
                context.Users.Add(user);
                context.SaveChanges();
                
            }

            var UserRoles = new IdentityUserRole<string>[]
            {
                new IdentityUserRole<string>{RoleId = roles[0].Id, UserId=user.Id},
                new IdentityUserRole<string>{RoleId = roles[1].Id, UserId=user.Id},
            };

            foreach (IdentityUserRole<string> r in UserRoles)
            {
                context.UserRoles.Add(r);
            }

            if(!context.UserRoles.Any())
            {
                context.SaveChanges();
            }
        }
    }
}