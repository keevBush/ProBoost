using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Extensions;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Firestore;
using Java.Interop;
using Java.Lang;
using ProBoost.Droid.DependencyServices;
using ProBoost.Models;
using ProBoost.Services;
using Xamarin.Forms;


[assembly: Dependency(typeof(AuthService))]
namespace ProBoost.Droid.DependencyServices
{
    public class AuthService : Java.Lang.Object, IAuthServices,IOnSuccessListener
    {
        public User CurrentUser => new User
        {
            UId = Firebase.FirebaseAuth.CurrentUser?.Uid,
            Email = Firebase.FirebaseAuth.CurrentUser?.Email,
            Fullname = Firebase.FirebaseAuth.CurrentUser?.DisplayName,
            Phone = Firebase.FirebaseAuth.CurrentUser?.PhoneNumber,
        };

        public event EventHandler GetUser;

        public void UserPhone() {
            Firebase.FirebaseFirestore.Collection($"users").Document(CurrentUser.UId).Get().AddOnCompleteListener(
                new OnCompleteEventHandleListener((obj) =>
                {
                    if (obj.IsSuccessful)
                    {
                        var snapshot = (DocumentSnapshot)(obj.Result);
                        GetUser.Invoke(this, new UserEventArgs
                        {
                            EventType = EventType.User,
                            User = new User
                            {
                                UId = snapshot.GetString("uid"),
                                Email = snapshot.GetString("email"),
                                Fullname = snapshot.GetString("fullname"),
                                Phone = snapshot.GetString("phone"),
                            }
                        });
                    }
                }));
        }


        public async System.Threading.Tasks.Task Login(string email, string password)
        {
            try
            {
                await Firebase.FirebaseAuth.SignInWithEmailAndPasswordAsync(email, password);
            }
            catch (System.Exception e)
            {
                Toast.MakeText(Android.App.Application.Context, $"{e.Message}", ToastLength.Long);
                throw e;
            }
        }

        public void Logout()
        {
            Firebase.FirebaseAuth.SignOut();
        }

        public async System.Threading.Tasks.Task Register(string email, string fullname, string password)
        {
            var user = await Firebase.FirebaseAuth.CreateUserWithEmailAndPasswordAsync(email, password);
            var dictionary = new JavaDictionary<string, object>();
            dictionary.Add("email", email);
            dictionary.Add("uid", user.User.Uid);
            dictionary.Add("fullname", fullname);
            dictionary.Add("phone", "");
            Firebase.FirebaseFirestore.Document($"users/{user.User.Uid}/").Set(dictionary);
        }

        public void OnSuccess(Java.Lang.Object result)
        {

        }

        public async System.Threading.Tasks.Task UpdateUser(User user)
        {
            if(user.Email.Trim() != CurrentUser.Email)
                await Firebase.FirebaseAuth.CurrentUser.UpdateEmailAsync(user.Email);
            var dictionary = new JavaDictionary<string, object>();
            dictionary.Add("email", user.Email.Trim());
            dictionary.Add("uid", user.UId);
            dictionary.Add("fullname", user.Fullname.Trim());
            dictionary.Add("phone", user.Phone.Trim());
            await Firebase.FirebaseFirestore.Document($"users/{user.UId}").Set(dictionary);
        }

        public void GetOneUser(string uid)
        {
            Firebase.FirebaseFirestore.Collection($"users").Document(uid).Get().AddOnCompleteListener(
                new OnCompleteEventHandleListener((obj) =>
                {
                    if (obj.IsSuccessful)
                    {
                        var snapshot = (DocumentSnapshot)(obj.Result);
                        GetUser.Invoke(this, new UserEventArgs
                        {
                            EventType = EventType.UserGet,
                            User = new User
                            {
                                UId = snapshot.GetString("uid"),
                                Email = snapshot.GetString("email"),
                                Fullname = snapshot.GetString("fullname"),
                                Phone = snapshot.GetString("phone"),
                            }
                        });
                    }
                }));
        }
    }

    

    public class OnCompleteEventHandleListener : Java.Lang.Object, IOnCompleteListener
    {
        private readonly Action<Android.Gms.Tasks.Task> _completeAction;

        public OnCompleteEventHandleListener(Action<Android.Gms.Tasks.Task> completeAction)
        {
            _completeAction = completeAction;
        }

        public void OnComplete(Android.Gms.Tasks.Task task) => _completeAction(task);
    }
}