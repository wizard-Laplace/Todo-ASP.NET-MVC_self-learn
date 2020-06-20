using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;

namespace TodoApp.Models
{
    public class CustomMembershipProvider : MembershipProvider
    {
        public override bool EnablePasswordRetrieval => throw new NotImplementedException();

        public override bool EnablePasswordReset => throw new NotImplementedException();

        public override bool RequiresQuestionAndAnswer => throw new NotImplementedException();

        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override int MaxInvalidPasswordAttempts => throw new NotImplementedException();

        public override int PasswordAttemptWindow => throw new NotImplementedException();

        public override bool RequiresUniqueEmail => throw new NotImplementedException();

        public override MembershipPasswordFormat PasswordFormat => throw new NotImplementedException();

        public override int MinRequiredPasswordLength => throw new NotImplementedException();

        public override int MinRequiredNonAlphanumericCharacters => throw new NotImplementedException();

        public override string PasswordStrengthRegularExpression => throw new NotImplementedException();

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            //usingディレクティブを使用することで変数"db"はusingディレクティブ内でのみ参照できる。
            //また、usingディレクティブの処理を抜けるタイミングで自動で変数"db"に対してdisposeメソッドが実行され、
            //メモリの管理が行われる。
            using (var db = new TodoesContext())
            {
                //ハッシュ値を取得する
                string hash = this.GeneratePasswordHash(username, password);

                var user = db.Users
                    .Where(u => u.UserName == username && u.Password == hash)
                    //FirstOrDefaultはWhereの抽出結果リストの先頭を返す。
                    //或いはリストが空ならnullを返す
                    .FirstOrDefault();

                if(user != null)
                {
                    return true;
                }
            }

            ///TODO 仮の処理の為、後で削除する
            //現在dbに格納されているパスワードはハッシュ化されており、
            //ログイン方法が無い為、adminユーザーのみログオン方法を担保する
            if("admin".Equals(username) && "passowrd".Equals(password))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// ユーザ名とパスワードを引数で受けて、
        /// ハッシュ化されたパスワードを返す
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>Convert.ToBase64String(hash)</returns>
        public string GeneratePasswordHash(string username, string password)
        {
            //引数usernameの頭に適当な文字列を連結
            string rawSalt = $"secret_{username}";
            
            //ここからハッシュ化処理
            var sha256 = new SHA256CryptoServiceProvider();
            //rasSaltをバイト文字列へエンコーディングする
            var salt = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(rawSalt));
            //パスワードをpbkdf2でハッシュ化する
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            //Rfcインスタンス生成後ハッシュ化したパスワードの取得
            var hash = pbkdf2.GetBytes(32);

            //変数hashがbyt[]なので文字列へ変換し返す
            return Convert.ToBase64String(hash);
        }
    }
}