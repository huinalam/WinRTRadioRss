using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

using RadioRss.Model;
using Windows.UI.Core;

namespace RadioRss.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<FB_Category> _LV_Categorys;
        public ObservableCollection<FB_Category> LV_Categorys
        {
            get
            {
                return _LV_Categorys;
            }
            set
            {
                _LV_Categorys = value;
                RaisePropertyChanged("LV_Category");
            }
        }

        private ObservableCollection<FB_Blog> _LV_Blogs;
        public ObservableCollection<FB_Blog> LV_Blogs
        {
            get
            {
                return _LV_Blogs;
            }
            set
            {
                _LV_Blogs = value;
                // More ��ư���� ���� �ÿ�, post�κ��� �ʱ�ȭ ���ֱ� �����̴�.
                _LV_Posts = new ObservableCollection<FB_Post>();
                RaisePropertyChanged("LV_Blogs");
            }
        }

        private ObservableCollection<FB_Post> _LV_Posts;
        public ObservableCollection<FB_Post> LV_Posts
        {
            get
            {
                return _LV_Posts;
            }
            set
            {
                _LV_Posts = value;
                RaisePropertyChanged("LV_Posts");
            }
        }

        public RelayCommand CMD_MovePodCastItemsPage { get; private set; }

        public static FeedBoard feedBoard;
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            _LV_Categorys = new ObservableCollection<FB_Category>();
            _LV_Blogs = new ObservableCollection<FB_Blog>();
            _LV_Posts = new ObservableCollection<FB_Post>();

            CMD_MovePodCastItemsPage = new RelayCommand(_CMD_MovePodCastItemsPage);

            feedBoard = new FeedBoard();

            // �׽�Ʈ�뵵
            //feedBoard.Blogs = LV_Blogs;

//             feedBoard.AddElement("http://old.ddanzi.com/appstream/drama.xml", FeedBoard.strCommon);
//             feedBoard.AddElement("http://pod.ssenhosting.com/rss/ingyeoin.xml", FeedBoard.strCommon);
//             feedBoard.AddElement("http://pod.ssenhosting.com/rss/ingyeoin.xml", "ī�װ�2");
//             feedBoard.AddElement("http://pod.ssenhosting.com/rss/ingyeoin.xml", "ī�װ�3");
//             feedBoard.AddElement("http://pod.ssenhosting.com/rss/ingyeoin.xml", "ī�װ�4");
//             feedBoard.AddElement("http://pod.ssenhosting.com/rss/ingyeoin.xml", "ī�װ�5");
//             feedBoard.AddElement("http://pod.ssenhosting.com/rss/ingyeoin.xml", "ī�װ�6");
//             feedBoard.AddElement("http://pod.ssenhosting.com/rss/ingyeoin.xml", "ī�װ�7");
//             feedBoard.AddElement("http://pod.ssenhosting.com/rss/ingyeoin.xml", "ī�װ�8");
//             feedBoard.AddElement("http://pod.ssenhosting.com/rss/ingyeoin.xml", "ī�װ�9");
            //http://old.ddanzi.com/appstream/drama.xml
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }



        private async void GetBlogs(string categoryName)
        {
            LV_Blogs = await feedBoard.GetBlogs(categoryName);
        }
        private async void GetPosts(int idx)
        {
            var blog = feedBoard.Blogs[idx];
            LV_Posts = await feedBoard.GetPosts(blog);
            this.RaisePropertyChanged("LV_Idx_Blogs");
        }

        private async void _CMD_MovePodCastItemsPage()
        {
            string Cate_name = Model.StaticVar.CategoryName;
            GetBlogs(Cate_name);
        }

        private async void _CMD_MovePostTablePage()
        {
            string Cate_name = Model.StaticVar.CategoryName;
            GetBlogs(Cate_name);
        }
    }
}