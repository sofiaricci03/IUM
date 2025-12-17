using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template.Infrastructure;

namespace Template.Services.Shared
{
    public class UsersSelectQuery
    {
        public Guid IdCurrentUser { get; set; }
        public string Filter { get; set; }
    }

    public class UsersSelectDTO
    {
        public IEnumerable<User> Users { get; set; }
        public int Count { get; set; }

        public class User
        {
            public Guid Id { get; set; }
            public string Email { get; set; }
        }
    }

    public class UsersIndexQuery
    {
        public Guid IdCurrentUser { get; set; }
        public string Filter { get; set; }
        public Paging Paging { get; set; }
    }

    public class UsersIndexDTO
    {
        public IEnumerable<User> Users { get; set; }
        public int Count { get; set; }

        public class User
        {
            public Guid Id { get; set; }
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public UserRole Role { get; set; }   // ⭐ AGGIUNTO
        }
    }

    public class UserDetailQuery
    {
        public Guid Id { get; set; }
    }

    public class UserDetailDTO
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public UserRole Role { get; set; }   // ⭐ AGGIUNTO
    }

    public class CheckLoginCredentialsQuery
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public partial class SharedService
    {
        public async Task<UsersSelectDTO> Query(UsersSelectQuery qry)
        {
            var queryable = _dbContext.Users
                .Where(x => x.Id != qry.IdCurrentUser);

            if (string.IsNullOrWhiteSpace(qry.Filter) == false)
            {
                queryable = queryable.Where(x =>
                    x.Email.Contains(qry.Filter, StringComparison.OrdinalIgnoreCase));
            }

            return new UsersSelectDTO
            {
                Users = await queryable
                    .Select(x => new UsersSelectDTO.User
                    {
                        Id = x.Id,
                        Email = x.Email
                    })
                    .ToArrayAsync(),
                Count = await queryable.CountAsync(),
            };
        }

        public async Task<UsersIndexDTO> Query(UsersIndexQuery qry)
        {
            var queryable = _dbContext.Users
                .Where(x => x.Id != qry.IdCurrentUser);

            if (string.IsNullOrWhiteSpace(qry.Filter) == false)
            {
                queryable = queryable.Where(x =>
                    x.Email.Contains(qry.Filter, StringComparison.OrdinalIgnoreCase));
            }

            return new UsersIndexDTO
            {
                Users = await queryable
                    .ApplyPaging(qry.Paging)
                    .Select(x => new UsersIndexDTO.User
                    {
                        Id = x.Id,
                        Email = x.Email,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Role = x.Role    // ⭐ AGGIUNTO
                    })
                    .ToArrayAsync(),
                Count = await queryable.CountAsync()
            };
        }

        public async Task<UserDetailDTO> Query(UserDetailQuery qry)
        {
            return await _dbContext.Users
                .Where(x => x.Id == qry.Id)
                .Select(x => new UserDetailDTO
                {
                    Id = x.Id,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    NickName = x.NickName,
                    Role = x.Role      // //agg. per distinguere dipendente da resposabile
                })
                .FirstOrDefaultAsync();
        }

        public async Task<UserDetailDTO> Query(CheckLoginCredentialsQuery qry)
        {
            var user = await _dbContext.Users
                .Where(x => x.Email == qry.Email)
                .FirstOrDefaultAsync();

            if (user == null || user.IsMatchWithPassword(qry.Password) == false)
                throw new LoginException("Email o password errate");

            return new UserDetailDTO
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                NickName = user.NickName,
                Role = user.Role    // agg. per dividere il ruolo dipendente/responsabile
            };
        }
    }
}
