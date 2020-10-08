using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;

namespace ProBoost.Droid
{
    public class Firebase
    {
        public static FirebaseAuth _firebaseAuth;

        public static FirebaseAuth FirebaseAuth
        {
            get
            {
                if (_firebaseAuth == null)
                    _firebaseAuth = new FirebaseAuth(FirebaseApp.Instance);
                return _firebaseAuth;
            }
        }

        public static FirebaseFirestore _firebaseFirestore;

        public static FirebaseFirestore FirebaseFirestore
        {
            get
            {
                if (_firebaseFirestore == null)
                    _firebaseFirestore = FirebaseFirestore.Instance;
                return _firebaseFirestore;
            }
        }
    }
}