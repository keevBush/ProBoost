using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ProBoost.Droid.DependencyServices;
using ProBoost.Models;
using ProBoost.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(AuthService))]
namespace ProBoost.Droid.DependencyServices
{
    public class AuthService : IAuthServices
    {
        public User CurrentUser => new User
        {
            UId = Firebase.FirebaseAuth.CurrentUser?.Uid,
            Email = Firebase.FirebaseAuth.CurrentUser?.Email,
        };

        public async Task Login(string email, string password)
        {
            try
            {
                await Firebase.FirebaseAuth.SignInWithEmailAndPasswordAsync(email, password);
            }
            catch (Exception e)
            {
                Toast.MakeText(Android.App.Application.Context, $"{e.Message}", ToastLength.Long);
                throw e;
            }
        }

        public void Logout()
        {
            Firebase.FirebaseAuth.SignOut();
        }

        public async Task Register(string email, string fullname, string password)
        {
            var user = await Firebase.FirebaseAuth.CreateUserWithEmailAndPasswordAsync(email, password);
            var dictionary = new JavaDictionary<string, object>();
            dictionary.Add("email", email);
            dictionary.Add("uid", user.User.Uid);
            dictionary.Add("password", password);
            dictionary.Add("fullname", fullname);
            dictionary.Add("phone", "");
            Firebase.FirebaseFirestore.Document($"users/{user.User.Uid}/").Set(dictionary);
        }
    }
}