using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.ComponentModel;

namespace MyNotes
{
    public class NotesModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 记事编号
        /// </summary>
        public long NoteID { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        /// 记事时间
        /// </summary>
        public DateTime NoteTime { get; set; }

        /// <summary>
        /// 记事内容
        /// </summary>
        public String NoteContent { get; set; }

        #region 数据库操作

        /// <summary>
        /// 添加
        /// </summary>
        public void Add()
        {
            string strSQL = string.Format(@"insert into 
                NOTE_INFO(NT_DATE,NT_CONTENT,NT_USER_ID) 
                values('{0}','{1}',{2})", NoteTime,NoteContent, UserID);

            NoteID = MySqlHelper.ExecuteNonQuery(MySqlHelper.Conn, CommandType.Text, strSQL, null);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns>bool</returns>
        public bool Upate()
        {
            if (NoteID < 1)
            {
                throw new ArgumentException("需要更新的记事编号未知！");
            }

            string strSQL = string.Format(@"update into NOTE_INFO set 
                NT_CONTENT={0} where NT_ID={1}", NoteContent, NoteID);

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
            if (NoteID < 1)
            {
                throw new ArgumentException("需要删除的记事编号未知！");
            }

            string strSQL = string.Format(@"delete from NOTE_INFO where NT_ID={0}", NoteID);
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
        /// <param name="Id">记事编号</param>
        public void GetValue(long Id)
        {
            if (Id < 1)
            {
                throw new ArgumentException("查询的记事编号不合法！");
            }

            NoteID = Id;

            string strSQL = string.Format(@"select NT_DATE,NT_CONTENT,NT_USER_ID 
                from NOTE_INFO where NT_ID={0}", NoteID);

            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(
                MySqlHelper.Conn, CommandType.Text, strSQL, null))
            {
                while (dr.Read())
                {
                    NoteTime = dr.GetDateTime(0);
                    UserID = dr.GetInt64(2);

                    if (dr[1] != DBNull.Value)
                        NoteContent = dr.GetString(1);
                }
            }
        }

        /// <summary>
        /// 获取所有笔记记录
        /// </summary>
        /// <returns></returns>
        public static List<NotesModel> GetValues()
        {
            List<NotesModel> noteList = null;
            string strSQL = string.Format(@"select NT_DATE,NT_CONTENT,NT_USER_ID,
                NT_ID from NOTE_INFO");

            using (MySqlDataReader dr = MySqlHelper.ExecuteReader(
                MySqlHelper.Conn, CommandType.Text, strSQL, null))
            {
                noteList = new List<NotesModel>();
                while (dr.Read())
                {
                    NotesModel noteInfo = new NotesModel();
                    noteInfo.NoteTime = dr.GetDateTime(0);
                    noteInfo.UserID = dr.GetInt64(2);
                    if (dr[1] != DBNull.Value)
                        noteInfo.NoteContent = dr.GetString(1);
                    noteInfo.NoteID = dr.GetInt64(3);

                    noteList.Add(noteInfo);
                }
            }

            return noteList;
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
