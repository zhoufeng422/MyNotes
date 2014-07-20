using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.ComponentModel;

namespace MyNotes
{
    public class UserModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 用户编码
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public String UserName { get; set; }

        /// <summary>
        /// 用户描述
        /// </summary>
        public String UserDesc { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        public String Password { get; set; }

        #region 数据库操作

        /// <summary>
        /// 添加
        /// </summary>
        public void Add()
        {
            string strSQL = string.Format(@"insert into 
                NOTE_USER(NT_USER_NAME,NT_USER_DESC,NT_USER_PW) 
                values('{0}','{1}','{2}')", UserName, UserDesc, Password);

            UserID = MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, CommandType.Text, strSQL, null);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns>bool</returns>
        public bool Upate()
        {
            if (UserID < 1)
            {
                throw new ArgumentException("需要更新的用户信息编号未知！");
            }

            string strSQL = string.Format(@"update into NOTE_USER set 
                NT_USER_NAME='{0}',NT_USER_DESC='{1}',NT_USER_PW='{2}' 
                where NT_USER_ID={3}", UserName, UserDesc, Password, UserID);

            int rc = MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, CommandType.Text, strSQL, null);

            if (rc > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns>bool</returns>
        public bool Delete()
        {
            if (UserID < 1)
            {
                throw new ArgumentException("需要删除的用户信息编号未知！");
            }

            string strSQL = string.Format(@"delete from NOTE_USER where NT_USER_ID={0}", UserID);
            int rc = MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, CommandType.Text, strSQL, null);

            if (rc > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取一条记录
        /// </summary>
        /// <param name="Id">用户编号</param>
        public void GetValue(long Id)
        {
            if (Id < 1)
            {
                throw new ArgumentException("查询的用户信息编号不合法！");
            }

            UserID = Id;

            string strSQL = string.Format(@"select NT_USER_NAME,NT_USER_DESC,NT_USER_PW 
                from NOTE_USER where NT_USER_ID={0}", UserID);

            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(
                MySqlHelper.Conn, CommandType.Text, strSQL, null))
            {
                while (dr.Read())
                {
                    UserName = dr.GetString(0);
                    Password = dr.GetString(2);

                    if (dr[1] != DBNull.Value)
                        UserDesc = dr.GetString(1);
                }
            }
        }

        #endregion

        #region INotifyPropertyChanged 成员

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
