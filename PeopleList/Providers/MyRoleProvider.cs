﻿using PeopleList.Helpers;
using PeopleList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace PeopleList.Providers
{
    public class MyRoleProvider : RoleProvider
    {
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string id)
        {
            People people = HelperConnect.GetPeople(int.Parse(id));
           if(people.Role == Models.Roles.SuperAdmin)
            {
                return new string[] { people.Role.ToString(), "Admin" };
            }
            else
            {
                return new string[] { people.Role.ToString() };
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string id, string roleName)
        {
            People people = HelperConnect.GetPeople(int.Parse(id));

            if (people != null && (people.Role.ToString() == roleName || people.Role == Models.Roles.SuperAdmin && roleName == "Admin"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}