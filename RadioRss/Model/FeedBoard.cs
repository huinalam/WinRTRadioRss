using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Web.Syndication;
using System.Collections.ObjectModel;
using Windows.Storage;

using System.IO;
using System.Xml;

using System.Text.RegularExpressions;

namespace RadioRss.Model
{
    public class FeedBoard
    {
        public ObservableCollection<FB_Category> Category { private set; get; }
        public ObservableCollection<FB_Blog> Blogs { set; get; }
        public ObservableCollection<FB_Post> Posts { set; get; }

        // ObservableCollection 에 보여지기 위한 원본 데이터
        public static List<FB_Category> Ori_Category { private set; get; }  // static 적용에 유의할 것
        public List<FB_Blog> Ori_Blogs { private set; get; }
        public List<FB_Post> Ori_Posts { private set; get; }    // 사용???

        public static FB_Category SelectCategory;

        /// <summary>
        /// 기본 카테고리 이름
        /// </summary>
        public static readonly string strCommon = "TV 및 영화";

        public FeedBoard()
        {
            Category = new ObservableCollection<FB_Category>();
            Blogs = new ObservableCollection<FB_Blog>();
            Posts = new ObservableCollection<FB_Post>();
            Ori_Category = new List<FB_Category>(); // 원본
            Ori_Blogs = new List<FB_Blog>();    // 원본
            Ori_Posts = new List<FB_Post>();    // 사용???

            TestInit();
        }

        // 초기값
        private void TestInit()
        {
            AddCategoryList(strCommon);
        }

        private bool _b_Error;
        public bool B_Error
        {
            get 
            {
                bool currentBool = _b_Error;
                _b_Error = false;
                return currentBool; 
            }
        }

        /// <summary>
        /// 블로그 리스트 추가
        /// </summary>
        /// <param name="blogUri"></param>
        /// <param name="categoryName"></param>
        public void AddElement(string blogUri, string categoryName)
        {
            AddElement(new Uri(blogUri), categoryName);
        }
        public async void AddElement(Uri blogUri, string categoryName)
        {
            _i_GetBlogs++;
            System.Threading.Tasks.Task task = new System.Threading.Tasks.Task( async() =>{            
                if (categoryName == "" || categoryName == null)
                    categoryName = strCommon;
                else
                    AddCategoryList(categoryName);
                FB_Category fb_cate = CategoryFind(categoryName);

                SyndicationClient client = new SyndicationClient();
                Uri feedUri = blogUri;

                SyndicationFeed feed;// = await client.RetrieveFeedAsync(feedUri);
                try
                {   // 비정상 적으로 받았을 경우
                    feed = await client.RetrieveFeedAsync(feedUri);
                }
                catch (Exception)
                {
                    _b_Error = true;
                    _i_GetBlogs--;
                    return;
                }

                // This code is executed after RetrieveFeedAsync returns the SyndicationFeed.
                // Process it and copy the data we want into our FeedData and FeedItem classes.
                FB_Blog feedData = new FB_Blog();

                feedData.Title = feed.Title.Text;
                feedData.BlogUri = feedUri;
                feedData.ImageUri = feed.ImageUri;
                if (feed.Subtitle != null && feed.Subtitle.Text != null)
                {
                    feedData.Description = feed.Subtitle.Text;
                }
                // Use the date of the latest post as the last updated date.
                feedData.PubDate = feed.Items[0].PublishedDate.DateTime;
                feedData.ReadLastDate = DateTime.Now;
                feedData.CategoryName = fb_cate;

                fb_cate.FB_Blogs.Add(feedData);
                Ori_Blogs.Add(feedData);
                await FeedPost(feedData);
                _i_GetBlogs--;
            });
            task.Start();

            // 동기화 시켜주기 위해서 기다리는 명령줄
            while (_i_GetBlogs != 0)
            {
                new System.Threading.ManualResetEvent(false).WaitOne(10);
            }
        }

        public bool AddCategory(string categoryName)
        {
            if (categoryName == "" || categoryName == null)
                return false;

            FB_Category fb_cate = CategoryFind(categoryName);
            if (fb_cate == null)
            {
                Ori_Category.Add(new FB_Category { Name = categoryName });
                return true;
            }
            return false;
        }

        // 블로그에 올라온 포스트들을 다시 읽어들인다.
        public async void RefreshBlogPost()
        {
            foreach (FB_Blog blog in Ori_Blogs)
            {
                await FeedPost(blog);
            }
        }
        private async Task FeedPost(FB_Blog blog)
        {
            // using Windows.Web.Syndication;
            SyndicationClient client = new SyndicationClient();
            Uri feedUri = blog.BlogUri;            
            try
            {
                SyndicationFeed feed = await client.RetrieveFeedAsync(feedUri);

                blog.Title = feed.Title.Text;
                if (feed.Subtitle != null && feed.Subtitle.Text != null)
                {
                    blog.Description = feed.Subtitle.Text;
                }
                // Use the date of the latest post as the last updated date.
                blog.PubDate = feed.Items[0].PublishedDate.DateTime;
                blog.ImageUri = Search_ImageUri(feed);

                foreach (SyndicationItem item in feed.Items)
                {
                    FB_Post feedItem = new FB_Post();
                    feedItem.Title = item.Title.Text;
                    feedItem.PubDate = item.PublishedDate.DateTime;
                    //feedItem.Author = item.Authors[0].Name.ToString();
                    // Handle the differences between RSS and Atom feeds.
                    if (feed.SourceFormat == SyndicationFormat.Atom10)
                    {
                        feedItem.Content = item.Content.Text;
                        // 주소를 읽어들인다.
                        try
                        {
                            feedItem.Link = new Uri(feed.Links[0].Uri.AbsoluteUri + item.Id);
                        }
                        catch (Exception)
                        {
                            try
                            {
                                feedItem.Link = item.Links[1].Uri;
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                    else if (feed.SourceFormat == SyndicationFormat.Rss20)
                    {
                        //feedItem.Content = item.Summary.Text;
                        try
                        {   // 이미지를 추가한다.
                            feedItem.Img = blog.ImageUri;
                        }
                        catch (Exception)
                        {
                            feedItem.Img = null;
                        }
                        feedItem.Link = item.Links[1].Uri;
                    }
                    blog.FB_Posts.Add(feedItem);
                }
            }
            catch (Exception)
            {
                int a;
            }            
        }

        public async void InitRefreshSet_Blogs(string categoryName)
        {
            Blogs = new ObservableCollection<FB_Blog>();
            foreach (FB_Category category in Ori_Category)
            {
                if (category.Name == categoryName)
                {
                    foreach (FB_Blog blog in category.FB_Blogs)
                    {
                        //await FeedPost(blog);                        
                        Blogs.Add(blog);
                    }
                    return;
                }
            }
        }
        public async void InitSet_Posts(FB_Blog blog)
        {
            Posts = new ObservableCollection<FB_Post>();
            //await FeedPost(blog);
            foreach (FB_Post post in blog.FB_Posts)
            {
                Posts.Add(post);
            }
        }

        private int _i_GetBlogs = 0;
        private int _i_GetPosts = 0;
        public async Task<ObservableCollection<FB_Blog>> GetBlogs(string categoryName)
        {
            while (_i_GetBlogs!=0)
            {
                new System.Threading.ManualResetEvent(false).WaitOne(100);
            }
            InitRefreshSet_Blogs(categoryName);
            return Blogs;
        }
        public async Task<ObservableCollection<FB_Post>> GetPosts(FB_Blog blog)
        {
            InitSet_Posts(blog);
            return Posts;
        }

        // 삭제 예정 메쏘드
        protected FB_Category FindEqualCategory(string cate_name)
        {
            foreach (FB_Category cate in Ori_Category)
            {
                if (cate.Name == cate_name)
                    return cate;
            }
            // 에러, 이럴일은 없뜸 -_-)
            return null;
        }

        // 카테고리 추가
        protected void AddCategoryList(string str)
        {
            var category = CategoryFind(str);
            if (category == null)    // 카테고리가 없으면 추가해준다.
            {
                Ori_Category.Add(new FB_Category { Name = str });
                //RefreshCollectionFromCategoryList(OriCategoryList);
            }
        }
        protected FB_Category CategoryFind(string str)
        {
            var category = new FB_Category { Name = str };
            return Ori_Category.Find(   // 추가할 카테고리에 같은 것이 있는지?
                (FB_Category cate) => { return (cate.Name == category.Name); }
            );
        }

        // static으로 view, control 접근 위함
        public static FB_Category SearchCategory(string str)
        {
            var category = new FB_Category { Name = str };
            return Ori_Category.Find(   // 추가할 카테고리에 같은 것이 있는지?
                (FB_Category cate) => { return (cate.Name == category.Name); }
            );
        }

        private Uri Search_ImageUri(SyndicationFeed feed)
        {
            foreach (var ele in feed.ElementExtensions)
            {
                if (ele.NodeName.ToLower() == "image")
                {
                    try
                    {
                        return new Uri(ele.AttributeExtensions[0].Value);
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            }
            return null;
        }
    }

    public static class Method
    {
        /// <summary>
        /// html소스를 이용하여 이미지를 여러개 받아낸다.
        /// </summary>
        /// <param name="htmlSource"></param>
        /// <returns></returns>
        public static List<Uri> FetchLinksFromSource(string htmlSource)
        {
            List<Uri> links = new List<Uri>();
            string regexImgSrc = @"<img[^>]*?src\s*=\s*[""']?([^'"" >]+?)[ '""][^>]*?>";
            MatchCollection matchesImgSrc = Regex.Matches(htmlSource, regexImgSrc, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            foreach (Match m in matchesImgSrc)
            {
                string href = m.Groups[1].Value;
                links.Add(new Uri(href));
            }
            return links;
        }

        public static Uri FetchLinksFromSource_ForFirst(string htmlSource)
        {
            List<Uri> list = FetchLinksFromSource(htmlSource);
            int val = new Random().Next(0, list.Count);
            return list[val];
        }
    }

    public class FB_Category
    {
        public string Name { get; set; }
        private List<FB_Blog> _FB_Blogs = new List<FB_Blog>();
        public List<FB_Blog> FB_Blogs
        {
            get { return _FB_Blogs; }
            set { _FB_Blogs = value; }
        }
    }

    public class FB_Blog
    {
        public Uri BlogUri { get; set; }
        public Uri ImageUri { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// 최신 업데이트 날짜
        /// </summary>
        public DateTime PubDate { get; set; }
        public string StrPubDate
        {
            get
            {
                return PubDate.ToString();
            }
        }
        /// <summary>
        /// 사용자가 읽은 마지막 시간
        /// </summary>
        public DateTime ReadLastDate { get; set; }
        /// <summary>
        /// 자신을 list하고 있는 category를 가리킨다, 이후 카테고리 수정을 위한 변수
        /// </summary>
        public FB_Category CategoryName { get; set; }

        private List<FB_Post> _FB_Posts = new List<FB_Post>();
        public List<FB_Post> FB_Posts
        {
            get { return _FB_Posts; }
            set { _FB_Posts = value; }
        }
    }

    public class FB_Post
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Content { get; set; }
        public DateTime PubDate { get; set; }
        public string StrPubDate
        {
            get
            {
                return PubDate.ToString();
            }
        }
        public Uri Link { get; set; }
        public Uri Img { get; set; }
        public bool ReadStatus { get; set; }
    }
}
