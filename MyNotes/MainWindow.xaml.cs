using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;

namespace MyNotes
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_ContentRendered(object sender, EventArgs e)
        {
            QueryNodes();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (AddNode())
                {
                    QueryNodes();
                }
            }

            //Tile noteTile = new Tile();
            //noteTile.Title = "记事";
            //noteTile.Width = 100;
            //noteTile.Height = 100;

            //noteTile.Content = "测试";

            //txtDisplay.Children.Add(noteTile);
            //return;
        }

        private void QueryNodes()
        {
            List<NotesModel> noteList = NotesModel.GetValues();

            if (noteList != null && noteList.Count > 0)
            {
                txtDisplay.Children.Clear();
                foreach (var noteInfo in noteList)
                {
                    OneNode node = new OneNode();
                    node.DataContext = noteInfo;

                    txtDisplay.Children.Add(node);
                }
            }
        }

        private bool AddNode()
        {
            NotesModel notes = new NotesModel();
            notes.NoteContent = txtContent.Text;
            notes.NoteTime = DateTime.Now;
            notes.UserID = 1;

            notes.Add();

            if (notes.NoteID > 0)
            {
                return true;
            }

            return false;
        }
    }
}
